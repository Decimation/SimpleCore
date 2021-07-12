using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

// ReSharper disable UnusedMember.Global

namespace SimpleCore.Utilities.Configuration
{
	public static class ConfigHandler
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

		

		public static void ReadFromFile(IConfig obj)
		{

			// create cfg with default options if it doesn't exist
			if (!File.Exists(obj.ConfigFile)) {

				var f = File.Create(obj.ConfigFile);
				f.Close();
			}

			var cfg = Collections.ReadDictionary(obj.ConfigFile);

			var tuples = obj.GetType().GetAnnotated<ConfigFieldAttribute>();

			foreach (var (_, member) in tuples) {
				string memberName = member.Name;

				var (attr, field) = GetField<ConfigFieldAttribute>(obj, memberName);

				var    defaultValue     = (object) attr.DefaultValue;
				bool   setDefaultIfNull = attr.SetDefaultIfNull;
				string name             = attr.Id;


				var v = ReadMapValue(cfg, name, setDefaultIfNull, defaultValue);
				//Debug.WriteLine($"{v} -> {name} {field.Name}");
				string? valStr = v.ToString();

				var fi = member.GetBackingField();

				var val = ParseValue(valStr, fi.FieldType);

				fi.SetValue(obj, val);
			}


			Collections.WriteDictionary(ConvertToMap(obj), obj.ConfigFile);

		}

		/// <summary>
		///     Read config from command line arguments
		/// </summary>
		public static void ReadFromArguments(IConfig cfg, string[] args)
		{
			//string[] args = cfg.CliArguments;

			if (!args.Any()) {
				return;
			}

			var argQueue = new Queue<string>(args);

			using var argEnumerator = argQueue.GetEnumerator();

			while (argEnumerator.MoveNext()) {
				string parameterName = argEnumerator.Current;

				var parameterName1 = argEnumerator.Current;

				var members = GetMembers<ConfigFieldAttribute>(cfg);

				// Corresponding component
				var component = members.FirstOrDefault(y => y.Attribute.ParameterName == parameterName1);

				if (component == default)
				{ }
				else {
					argEnumerator.MoveNext();

					string argValueRaw = argEnumerator.Current;

					var field = component.Member.GetBackingField();

					var value = ParseValue(argValueRaw, field.FieldType);
					Debug.WriteLine($"{field.Name} -> {value}");
					field.SetValue(cfg, value);
				}

			}
		}

		public static void Reset(IConfig obj)
		{
			var tuples = GetFields<ConfigFieldAttribute>(obj);

			foreach (var (attr, field) in tuples) {
				var dv = attr.DefaultValue;
				field.SetValue(obj, dv);

				//Debug.WriteLine($"Reset {dv} -> {field.Name}");
			}
		}


		public static IDictionary<string, string> ConvertToMap(IConfig obj)
		{
			var cfgFields = GetFields<ConfigFieldAttribute>(obj);

			var keyValuePairs = cfgFields.Select(f =>
				                                     new KeyValuePair<string, string>(
					                                     f.Attribute.Id, f.Field.GetValue(obj).ToString()));

			return new Dictionary<string, string>(keyValuePairs);
		}


		private static T ReadMapValue<T>(IDictionary<string, string> cfg, string id,
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

				rawValue = ReadMapValue<string>(cfg, id);
			}

			var parse = (T) ParseValue(rawValue, typeof(T));
			return parse;
		}

		#region

		private static object ParseValue(string rawValue, Type t)
		{
			if (t.IsEnum) {
				Enum.TryParse(t, rawValue, out var e);
				return e;
			}

			if (t == typeof(bool)) {
				Boolean.TryParse(rawValue, out bool b);
				return b;
			}

			if (t == typeof(int)) {
				int.TryParse(rawValue, out int b);
				return b;
			}

			return rawValue;
		}

		public static FieldInfo GetResolvedField(this Type t, string fname)
		{
			var member = t.GetMember(fname, ALL_FLAGS).First();


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

		public static (TAttribute Attribute, MemberInfo Member)[] GetAnnotated<TAttribute>(this Type t)
			where TAttribute : Attribute
		{
			return (from member in t.GetMembers(ALL_FLAGS)
			        where Attribute.IsDefined(member, typeof(TAttribute))
			        select (member.GetCustomAttribute<TAttribute>(), member)).ToArray();
		}

		/// <summary>
		///     Get all members annotated with <see cref="ConfigFieldAttribute" /> in <paramref name="obj" />.
		/// </summary>
		public static (TAttribute Attribute, MemberInfo Member)[] GetMembers<TAttribute>(object obj)
			where TAttribute : Attribute
		{
			var tuples = obj.GetType().GetAnnotated<TAttribute>();

			return tuples.Select(y => (y.Attribute, y.Member)).ToArray();
		}

		/// <summary>
		///     Get all fields annotated with <see cref="ConfigFieldAttribute" /> in <paramref name="obj" />.
		/// </summary>
		public static (TAttribute Attribute, FieldInfo Field)[] GetFields<TAttribute>(object obj)
			where TAttribute : Attribute
		{
			var tuples = GetMembers<TAttribute>(obj);

			return tuples.Select(y => (y.Attribute, y.Member.GetBackingField())).ToArray();
		}

		/// <summary>
		///     Get field of name <paramref name="mname" /> annotated with <see cref="ConfigFieldAttribute" /> in
		///     <paramref name="obj" />.
		/// </summary>
		public static (T Attribute, FieldInfo Field) GetField<T>(object obj, string mname) where T : Attribute
		{
			var t     = obj.GetType();
			var field = t.GetResolvedField(mname);

			var attr = field.GetCustomAttribute<T>();

			return (attr, field);
		}

		#endregion
	}
}