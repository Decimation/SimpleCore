﻿// ReSharper disable IdentifierTypo

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using static SimpleCore.Internal.Common;
using NotNull = JetBrains.Annotations.NotNullAttribute;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
#pragma warning disable IDE0051
#nullable enable
using AC = JetBrains.Annotations.AssertionConditionAttribute;
using ACT = JetBrains.Annotations.AssertionConditionType;
using DNRI = System.Diagnostics.CodeAnalysis.DoesNotReturnIfAttribute;
using AM = JetBrains.Annotations.AssertionMethodAttribute;

namespace SimpleCore.Diagnostics
{
	/// <summary>
	/// Diagnostic utilities, conditions, contracts
	/// </summary>
	/// <seealso cref="Debug"/>
	/// <seealso cref="Trace"/>
	/// <seealso cref="Debugger"/>
	public static class Guard
	{
		/*
		 * https://www.jetbrains.com/help/resharper/Contract_Annotations.html
		 * https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/attributes/nullable-analysis
		 */


		#region Contract annotations

		private const string VALUE_NULL_HALT = "value:null => halt";

		private const string VALUE_NOTNULL_HALT = "value:notnull => halt";

		private const string COND_FALSE_HALT = "condition:false => halt";

		private const string UNCONDITIONAL_HALT = "=> halt";

		#endregion

		[DebuggerHidden]
		[DoesNotReturn]
		[AssertionMethod]
		[ContractAnnotation(UNCONDITIONAL_HALT)]
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void Fail(string? msg = null, params object[] args) => Fail<Exception>(msg, args);


		/// <summary>
		/// Root fail function
		/// </summary>
		[DebuggerHidden]
		[DoesNotReturn]
		[AssertionMethod]
		[ContractAnnotation(UNCONDITIONAL_HALT)]
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void Fail<TException>(string? msg = null, params object[] args)
			where TException : Exception, new()
		{
			TException exception;

			if (msg != null) {
				string s = String.Format(msg, args);

				exception = (TException) Activator.CreateInstance(typeof(TException), s)!;
			}
			else {
				exception = new TException();
			}

			throw exception;
		}

		[DebuggerHidden]
		[AssertionMethod]
		[ContractAnnotation(COND_FALSE_HALT)]
		public static void Assert([AC(ACT.IS_TRUE)] [DNRI(false)] bool condition,
		                          string? msg = null, params object[] args)
		{
			Assert<Exception>(condition, msg, args);
		}

		/// <summary>
		/// Root assertion function
		/// </summary>
		[DebuggerHidden]
		[AssertionMethod]
		[ContractAnnotation(COND_FALSE_HALT)]
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void Assert<TException>([AC(ACT.IS_TRUE)] [DNRI(false)] bool condition,
		                                      string? msg = null, params object[] args)
			where TException : Exception, new()
		{
			if (!condition) {
				Fail<TException>(msg, args);
			}
		}

		[DebuggerHidden]
		[AssertionMethod]
		[ContractAnnotation(VALUE_NULL_HALT)]
		public static void AssertArgumentNotNull([NotNull] [AC(ACT.IS_NOT_NULL)] object? value,
		                                         string? name = null)
		{
			Assert<ArgumentNullException>(value != null, name);
		}


		[DebuggerHidden]
		[AssertionMethod]
		[ContractAnnotation(VALUE_NULL_HALT)]
		public static void AssertNotNull([NotNull] [AC(ACT.IS_NOT_NULL)] object? value, string? name = null)
		{
			Assert<NullReferenceException>(value != null, name);
		}

		[DebuggerHidden]
		[AssertionMethod]
		public static void AssertPositive(long value, string? name = null)
		{
			Assert<ArgumentException>(value > 0, name);
		}

		[DebuggerHidden]
		[AssertionMethod]
		public static void AssertEqual(object a, object b)
		{
			Assert<Exception>(a.Equals(b));
		}


		[DebuggerHidden]
		[AssertionMethod]
		public static void AssertEqual<T>(T a, T b) where T : IEquatable<T>
		{
			Assert<Exception>(a.Equals(b));
		}


		[DebuggerHidden]
		[AssertionMethod]
		public static void AssertThrows<T>(Action f) where T : Exception
		{
			bool throws = false;

			try {
				f();
			}
			catch (T) {
				throws = true;
			}

			if (!throws) {
				Fail();
			}
		}

		[DebuggerHidden]
		[AssertionMethod]
		public static void AssertAll([DNRI(false)] [AC(ACT.IS_TRUE)] params bool[] conditions)
		{
			foreach (bool condition in conditions) {
				Assert(condition);
			}
		}
	}

	public sealed class GuardException : Exception
	{
		public GuardException() { }

		public GuardException([CanBeNull] string? message) : base(message) { }
	}
}