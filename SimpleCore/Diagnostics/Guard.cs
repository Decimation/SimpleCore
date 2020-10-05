using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using JetBrains.Annotations;
using static SimpleCore.Internal.Common;
using BindingFlags = System.Reflection.BindingFlags;

// ReSharper disable UnusedMember.Global
#pragma warning disable HAA0502, HAA0101
#nullable enable
namespace SimpleCore.Diagnostics
{
	public static class Guard
	{
		private const string VALUE_NULL_HALT = "value:null => halt";

		private const string VALUE_NOTNULL_HALT = "value:notnull => halt";

		private const string COND_FALSE_HALT = "condition:false => halt";

		private const string UNCONDITIONAL_HALT = "=> halt";


		[AssertionMethod]
		[ContractAnnotation(UNCONDITIONAL_HALT)]
		[DebuggerHidden]
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void Fail<TException>(string? msg = null, params object[] args)
			where TException : Exception, new()
		{
			TException exception;

			if (msg != null) {
				var s = string.Format(msg, args);

				exception = (TException) Activator.CreateInstance(typeof(TException), s)!;

			}
			else {
				exception = new TException();
			}

			throw exception;
		}

		[AssertionMethod]
		[ContractAnnotation(COND_FALSE_HALT)]
		[DebuggerHidden]
		public static void Assert(bool condition, string? msg = null, params object[] args) =>
			Assert<Exception>(condition, msg, args);


		/// <summary>
		/// Root assertion function
		/// </summary>
		[AssertionMethod]
		[ContractAnnotation(COND_FALSE_HALT)]
		[DebuggerHidden]
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void Assert<TException>(bool condition, string? msg = null, params object[] args)
			where TException : Exception, new()
		{
			if (!condition) {
				Fail<TException>(msg, args);
			}
		}


		[AssertionMethod]
		[ContractAnnotation(VALUE_NULL_HALT)]
		[DebuggerHidden]
		public static void AssertArgumentNotNull(object? value, string? name = null) =>
			Assert<ArgumentNullException>(value != null, name);

		[AssertionMethod]
		[ContractAnnotation(VALUE_NULL_HALT)]
		[DebuggerHidden]
		public static void AssertNotNull(object? value, string? name = null) =>
			Assert<NullReferenceException>(value != null, name);
	}
}