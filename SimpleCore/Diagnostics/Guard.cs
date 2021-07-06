// ReSharper disable IdentifierTypo

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using static SimpleCore.Internal.Common;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
#pragma warning disable IDE0051
#pragma warning disable IDE0005
#nullable enable

#region Aliases

using AC = JetBrains.Annotations.AssertionConditionAttribute;
using ACT = JetBrains.Annotations.AssertionConditionType;
using DNR = System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute;
using DNRI = System.Diagnostics.CodeAnalysis.DoesNotReturnIfAttribute;
using AM = JetBrains.Annotations.AssertionMethodAttribute;
using NNV = JetBrains.Annotations.NonNegativeValueAttribute;
using NN = JetBrains.Annotations.NotNullAttribute;
using NN2 = System.Diagnostics.CodeAnalysis.NotNullAttribute;
using CBN = JetBrains.Annotations.CanBeNullAttribute;
using NotNull = JetBrains.Annotations.NotNullAttribute;
using DH = System.Diagnostics.DebuggerHiddenAttribute;
using CA = JetBrains.Annotations.ContractAnnotationAttribute;
using SFM = JetBrains.Annotations.StringFormatMethodAttribute;

#endregion

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

		[DH]
		[DNR]
		[AM]
		[CA(UNCONDITIONAL_HALT)]
		[SFM(STRING_FORMAT_ARG)]
		public static void Fail(string? msg = null, params object[] args) => Fail<Exception>(msg, args);


		/// <summary>
		/// Root fail function
		/// </summary>
		[DH]
		[DNR]
		[AM]
		[CA(UNCONDITIONAL_HALT)]
		[SFM(STRING_FORMAT_ARG)]
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

		[DH]
		[AM]
		[CA(COND_FALSE_HALT)]
		public static void Assert([AC(ACT.IS_TRUE)] [DNRI(false)] bool condition,
		                          string? msg = null, params object[] args)
		{
			Assert<Exception>(condition, msg, args);
		}


		/// <summary>
		/// Root assertion function
		/// </summary>
		[DH]
		[AM]
		[CA(COND_FALSE_HALT)]
		[SFM(STRING_FORMAT_ARG)]
		public static void Assert<TException>([AC(ACT.IS_TRUE)] [DNRI(false)] bool condition,
		                                      string? msg = null, params object[] args)
			where TException : Exception, new()
		{
			if (!condition) {
				Fail<TException>(msg, args);
			}
		}

		[DH]
		[AM]
		[CA(COND_FALSE_HALT)]
		public static void AssertArgument([AC(ACT.IS_TRUE)] [DNRI(false)] bool condition,
		                                  string? name = null)
		{
			Assert<ArgumentException>(condition, name);
		}

		[DH]
		[AM]
		[CA(VALUE_NULL_HALT)]
		public static void AssertArgumentNotNull([NN] [AC(ACT.IS_NOT_NULL)] object? value,
		                                         string? name = null)
		{
			Assert<ArgumentNullException>(value != null, name);
		}


		[DH]
		[AM]
		[CA(VALUE_NULL_HALT)]
		public static void AssertNotNull([NN] [AC(ACT.IS_NOT_NULL)] object? value, string? name = null)
		{
			Assert<NullReferenceException>(value != null, name);
		}

		[DH]
		[AM]
		public static void AssertEqual(object a, object b)
		{
			Assert<Exception>(a.Equals(b));
		}


		[DH]
		[AM]
		public static void AssertEqual<T>(T a, T b) where T : IEquatable<T>
		{
			Assert<Exception>(a.Equals(b));
		}


		[DH]
		[AM]
		public static void AssertContains<T>(IEnumerable<T> enumerable, T name)
		{
			Assert(enumerable.Contains(name));
		}

		[DH]
		[AM]
		public static void AssertPositive([NNV] long value, string? name = null)
		{
			Assert<ArgumentException>(value > 0, name);
		}

		[DH]
		[AM]
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

		[DH]
		[AM]
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