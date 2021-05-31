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

namespace SimpleCore.Diagnostics
{
	/// <summary>
	/// Diagnostic utilities, conditions, contracts
	/// </summary>
	public static class Guard
	{
		/*
		 * https://www.jetbrains.com/help/resharper/Contract_Annotations.html
		 * https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/attributes/nullable-analysis
		 */


		private const string VALUE_NULL_HALT = "value:null => halt";

		private const string VALUE_NOTNULL_HALT = "value:notnull => halt";

		private const string COND_FALSE_HALT = "condition:false => halt";

		private const string UNCONDITIONAL_HALT = "=> halt";

		[DebuggerHidden]
		[DoesNotReturn]
		[AssertionMethod]
		[ContractAnnotation(UNCONDITIONAL_HALT)]
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void Fail(string? msg = null, params object[] args)
			=> Fail<Exception>(msg, args);


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
				string? s = String.Format(msg, args);

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
		public static void Assert([AssertionCondition(AssertionConditionType.IS_TRUE)] [DoesNotReturnIf(false)]
		                          bool condition,
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
		public static void Assert<TException>(
			[AssertionCondition(AssertionConditionType.IS_TRUE)] [DoesNotReturnIf(false)]
			bool condition,
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
		public static void AssertArgumentNotNull([NotNull] [AssertionCondition(AssertionConditionType.IS_NOT_NULL)]
		                                         object? value,
		                                         string? name = null)
		{
			Assert<ArgumentNullException>(value != null, name);
		}


		[DebuggerHidden]
		[AssertionMethod]
		[ContractAnnotation(VALUE_NULL_HALT)]
		public static void AssertNotNull([NotNull] [AssertionCondition(AssertionConditionType.IS_NOT_NULL)]
		                                 object? value, string? name = null)
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
			// todo

			Assert<Exception>(a.Equals(b));
		}


		[DebuggerHidden]
		[AssertionMethod]
		public static void AssertEqual<T>(T a, T b) where T : IEquatable<T>
		{
			// todo

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


		// [DebuggerHidden]
		// [AssertionMethod]
		// public static void AssertEqual<T>(T a, T b) where T : IEquatable<T> => Assert<Exception>(a.Equals(b));


		[DebuggerHidden]
		[AssertionMethod]
		public static void AssertAll([DoesNotReturnIf(false)] [AssertionCondition(AssertionConditionType.IS_TRUE)]
		                             params bool[] conditions)
		{
			foreach (bool condition in conditions) {
				Assert(condition);
			}
		}
	}
}