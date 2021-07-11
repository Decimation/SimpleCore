using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SimpleCore.Utilities;
// ReSharper disable UnusedMember.Global

namespace SimpleCore.Model
{
	public static class ConfigComponents
	{
		
		/*
		 * Handles config components (properties or fields).
		 *
		 *
		 * A config component is a setting/option (i.e. field, parameter, etc).
		 * - Its value can be stored and later retrieved from a config file.
		 * - Its value can also be specified through the command line.
		 *
		 *
		 * mname - Member name
		 * id - Map name (Id)
		 */

		/// <summary>
		///     Update all fields annotated with <see cref="ConfigComponentAttribute" /> using <paramref name="obj" />.
		/// </summary>
		/// <param name="obj">Object</param>
		/// <param name="cfg">Map of values</param>
		public static void UpdateFields(IConfig obj)
		{
			bool newCfg = false;
			
			// create cfg with default options if it doesn't exist
			if (!File.Exists(obj.FileLocation))
			{

				var f = File.Create(obj.FileLocation);
				f.Close();
				newCfg = true;
			}
			
			var cfg = Collections.ReadDictionary(obj.FileLocation);

			var tuples = obj.GetType().GetAnnotated<ConfigComponentAttribute>();

			foreach (var (_, member) in tuples) {
				string  memberName = member.Name;
				
				var (attr, field) = GetField(obj, memberName);

				var    defaultValue     = (object) attr.DefaultValue;
				bool   setDefaultIfNull = attr.SetDefaultIfNull;
				string name             = attr.Id;


				var v = ReadComponentMapValue(cfg, name, setDefaultIfNull, defaultValue);
				//Debug.WriteLine($"{v} -> {name} {field.Name}");
				string? valStr     = v.ToString();

				var fi = member.GetBackingField();

				var val = ParseValue(valStr, fi.FieldType);

				fi.SetValue(obj, val);
			}

			if (newCfg) {
				Collections.WriteDictionary(ToMap(obj), obj.FileLocation);
			}
		}

		/// <summary>
		///     Reset all members annotated with <see cref="ConfigComponentAttribute" /> within <paramref name="obj" /> to their
		///     respective <see cref="ConfigComponentAttribute.DefaultValue" />.
		/// </summary>
		public static void Reset(IConfig obj)
		{
			var tuples = GetFields<ConfigComponentAttribute>(obj);

			foreach (var (attr, field) in tuples) {
				var dv = attr.DefaultValue;
				field.SetValue(obj, dv);

				//Debug.WriteLine($"Reset {dv} -> {field.Name}");
			}
		}


		/// <summary>
		///     Converts all members annotated with <see cref="ConfigComponentAttribute" /> in <paramref name="obj" /> to a
		///     <see cref="Dictionary{TKey,TValue}" />.
		/// </summary>
		public static IDictionary<string, string> ToMap(IConfig obj)
		{
			var cfgFields = GetFields<ConfigComponentAttribute>(obj);

			var keyValuePairs = cfgFields.Select(f =>
				                                     new KeyValuePair<string, string>(
					                                     f.Attribute.Id, f.Field.GetValue(obj).ToString()));

			return new Dictionary<string, string>(keyValuePairs);
		}


		public static object ParseValue(string rawValue, Type t)
		{
			if (t.IsEnum) {
				Enum.TryParse(t, rawValue, out var e);
				return e;
			}

			if (t == typeof(bool)) {
				Boolean.TryParse(rawValue, out bool b);
				return b;
			}

			if (t==typeof(int)) {
				int.TryParse(rawValue, out int b);
				return b;
			}

			return rawValue;
		}
		


		public static T ReadComponentMapValue<T>(IDictionary<string, string> cfg, string id,
		                                         bool setDefaultIfNull = false, 
		                                         T defaultValue = default)
		{

			if (!cfg.ContainsKey(id)) {
				cfg.Add(id, String.Empty);
			}

			string rawValue = cfg[id];

			if (setDefaultIfNull && String.IsNullOrWhiteSpace(rawValue)) {
				string valStr = defaultValue.ToString();

				if (!cfg.ContainsKey(id)) {
					cfg.Add(id, valStr);
				}
				else {
					cfg[id] = valStr;
				}

				rawValue = ReadComponentMapValue<string>(cfg, id);
			}

			var parse = (T) ParseValue(rawValue, typeof(T));
			return parse;
		}


		public static void ReadComponentFromArgument(IConfig obj, IEnumerator<string> argEnumerator)
		{
			var parameterName = argEnumerator.Current;

			var members = GetMembers<ConfigComponentAttribute>(obj);

			// Corresponding component
			var component = members.FirstOrDefault(y => y.Attribute.ParameterName == parameterName);

			if (component == default) {
				return;
			}

			argEnumerator.MoveNext();

			string argValueRaw = argEnumerator.Current;

			var field = component.Member.GetBackingField();

			var value = ParseValue(argValueRaw, field.FieldType);

			field.SetValue(obj, value);
		}

		#region

		public static FieldInfo GetResolvedField(this Type t, string fname)
		{
			var member = t.GetAnyMember(fname).First();


			var field = member.MemberType == MemberTypes.Property
				? t.GetResolvedField(fname)
				: member as FieldInfo;

			return field;
		}

		public static FieldInfo GetBackingField(this MemberInfo m)
		{
			var fv = m.DeclaringType.GetResolvedField(m.Name);

			return fv;
		}

		/// <summary>
		///     <see cref="ALL_INSTANCE_FLAGS" /> and <see cref="BindingFlags.Static" />
		/// </summary>
		public const BindingFlags ALL_FLAGS = ALL_INSTANCE_FLAGS | BindingFlags.Static;

		/// <summary>
		///     <see cref="BindingFlags.Public" />, <see cref="BindingFlags.Instance" />,
		///     and <see cref="BindingFlags.NonPublic" />
		/// </summary>
		public const BindingFlags ALL_INSTANCE_FLAGS =
			BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;

		public static IEnumerable<MemberInfo> GetAnyMember(this Type t, string name) => t.GetMember(name, ALL_FLAGS);

		public static IEnumerable<MemberInfo> GetAllMembers(this Type t) => t.GetMembers(ALL_FLAGS);

		public static (TAttribute Attribute, MemberInfo Member)[] GetAnnotated<TAttribute>(this Type t)
			where TAttribute : Attribute
		{
			return (from member in t.GetAllMembers()
			        where Attribute.IsDefined(member, typeof(TAttribute))
			        select (member.GetCustomAttribute<TAttribute>(), member)).ToArray();
		}

		/// <summary>
		///     Get all members annotated with <see cref="ConfigComponentAttribute" /> in <paramref name="obj" />.
		/// </summary>
		public static (TAttribute Attribute, MemberInfo Member)[] GetMembers<TAttribute>(object obj) where TAttribute:Attribute
		{
			var tuples = obj.GetType().GetAnnotated<TAttribute>();

			return tuples.Select(y => (y.Attribute, y.Member)).ToArray();
		}

		/// <summary>
		///     Get all fields annotated with <see cref="ConfigComponentAttribute" /> in <paramref name="obj" />.
		/// </summary>
		public static (TAttribute Attribute, FieldInfo Field)[] GetFields<TAttribute>(object obj) where TAttribute:Attribute
		{
			var tuples = GetMembers<TAttribute>(obj);

			return tuples.Select(y => (y.Attribute, y.Member.GetBackingField())).ToArray();
		}

		/// <summary>
		///     Get field of name <paramref name="mname" /> annotated with <see cref="ConfigComponentAttribute" /> in
		///     <paramref name="obj" />.
		/// </summary>
		public static (ConfigComponentAttribute Attribute, FieldInfo Field) GetField(object obj, string mname)
		{
			var t     = obj.GetType();
			var field = t.GetResolvedField(mname);

			var attr = field.GetCustomAttribute<ConfigComponentAttribute>();

			return (attr, field);
		}

		#endregion
	}
}