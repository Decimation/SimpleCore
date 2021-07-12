using System;
using JetBrains.Annotations;

namespace SimpleCore.Utilities.Configuration
{
	[AttributeUsage(AttributeTargets.Field)]
	[MeansImplicitUse(ImplicitUseTargetFlags.Default)]
	public sealed class ConfigFieldAttribute : Attribute
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


		public ConfigFieldAttribute(string id, string parameterName, object defaultValue,
		                                bool setDefaultIfNull)
		{
			Id               = id;
			DefaultValue     = defaultValue;
			SetDefaultIfNull = setDefaultIfNull;
			ParameterName    = parameterName;
		}

		public ConfigFieldAttribute(string id, [CanBeNull] string parameterName, object defaultValue) : this(
			id,
			parameterName, defaultValue, false) { }


		public ConfigFieldAttribute(string id, object defaultValue) : this(id, null, defaultValue) { }
	}
}