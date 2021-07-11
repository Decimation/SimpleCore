using System;
using JetBrains.Annotations;

namespace SimpleCore.Model
{
	/// <summary>
	/// <seealso cref="SearchConfig"/>
	/// <seealso cref="ConfigComponents"/>
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public sealed class ConfigComponentAttribute : Attribute
	{
		public object DefaultValue { get; set; }

		/// <summary>
		/// Component name
		/// </summary>
		public string Id { get; set; }


		public bool SetDefaultIfNull { get; set; }


		/// <summary>
		/// Parameter name
		/// </summary>
		[CanBeNull]
		public string ParameterName { get; set; }


		public ConfigComponentAttribute(string id, string parameterName, object defaultValue,
		                                bool setDefaultIfNull)
		{
			Id               = id;
			DefaultValue     = defaultValue;
			SetDefaultIfNull = setDefaultIfNull;
			ParameterName    = parameterName;
		}

		public ConfigComponentAttribute(string id, [CanBeNull] string parameterName, object defaultValue) : this(
			id,
			parameterName, defaultValue, false) { }


		public ConfigComponentAttribute(string id, object defaultValue) : this(id, null, defaultValue) { }
	}
}