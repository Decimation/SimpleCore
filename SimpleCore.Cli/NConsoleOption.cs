#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

// ReSharper disable InconsistentNaming

// ReSharper disable UnusedMember.Global


#pragma warning disable CS8618

namespace SimpleCore.Cli
{
	public delegate object? NConsoleFunction();


	/// <summary>
	///     Represents an interactive console/shell option
	/// </summary>
	public class NConsoleOption
	{
		/// <summary>
		///     Display name
		/// </summary>
		[MaybeNull]
		public virtual string Name { get; set; }

		/// <summary>
		///     Function to execute when selected
		/// </summary>
		public virtual NConsoleFunction Function { get; set; }

		/// <summary>
		///     Function to execute when selected with modifiers (<see cref="NConsole.NC_ALT_FUNC_MODIFIER" />)
		/// </summary>
		public virtual NConsoleFunction? AltFunction { get; set; }

		/// <summary>
		///     Function to execute when selected with modifiers (<see cref="NConsole.NC_CTRL_FUNC_MODIFIER" />)
		/// </summary>
		public virtual NConsoleFunction? CtrlFunction { get; set; }

		/// <summary>
		///     Function to execute when selected with modifiers (<see cref="NConsole.NC_SHIFT_FUNC_MODIFIER" />)
		/// </summary>
		public virtual NConsoleFunction? ShiftFunction { get; set; }

		/// <summary>
		///     Function to execute when selected with modifiers (<see cref="NConsole.NC_COMBO_FUNC_MODIFIER" />)
		/// </summary>
		public virtual NConsoleFunction? ComboFunction { get; set; }

		/// <summary>
		///     Information about this <see cref="NConsoleOption" />
		/// </summary>
		public virtual string? Data { get; set; }


		public virtual Color? Color { get; set; }

		public static NConsoleOption[] FromArray<T>(T[] values) => FromArray(values, arg => arg!.ToString()!);

		public static NConsoleOption[] FromArray<T>(T[] values, Func<T, string> getName)
		{
			var rg = new NConsoleOption[values.Length];

			for (int i = 0; i < rg.Length; i++) {
				var option = values[i];

				string name = getName(option);

				rg[i] = new NConsoleOption
				{
					Name     = name,
					Function = () => option
				};
			}

			return rg;
		}

		public static NConsoleOption[] FromEnum<TEnum>() where TEnum : Enum
		{
			var options = (TEnum[]) Enum.GetValues(typeof(TEnum));
			return FromArray(options, e => Enum.GetName(typeof(TEnum), e) ?? throw new InvalidOperationException());
		}
	}
}