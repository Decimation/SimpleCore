#nullable enable
using System;
using System.Drawing;
using static SimpleCore.CommandLine.NConsoleOption;

// ReSharper disable UnusedMember.Global

namespace SimpleCore.CommandLine
{
	public struct NConsoleKeyIO
	{
		public ConsoleKey Key { get; }
		public NConsoleFunction Function { get; }
	}

	/// <summary>
	///     Represents an interactive console/shell option
	/// </summary>
	public class NConsoleOption
	{
		public delegate object? NConsoleFunction();

		/// <summary>
		///     <see cref="NConsoleOption.AltFunction" />
		/// </summary>
		public const ConsoleModifiers NC_ALT_FUNC_MODIFIER = ConsoleModifiers.Alt;

		/// <summary>
		///     <see cref="NConsoleOption.CtrlFunction" />
		/// </summary>
		public const ConsoleModifiers NC_CTRL_FUNC_MODIFIER = ConsoleModifiers.Control;

		/// <summary>
		///     Default <see cref="Color" />
		/// </summary>
		public static readonly Color DefaultOptionColor = Color.White;

		/// <summary>
		///     Represents a <see cref="NConsoleOption" /> which is not yet available or in progress
		/// </summary>
		public static readonly NConsoleOption Wait = new NConsoleOption
		{
			Name = "Wait",

			Color = Color.Yellow,

			Function = () =>
			{
				//SystemSounds.Exclamation.Play();

				return null;
			},
			AltFunction = () => null,
			CtrlFunction = () => null
		};

		/// <summary>
		///     Display name
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		///     Function to execute when selected
		/// </summary>
		public virtual NConsoleFunction Function { get; set; }

		/// <summary>
		///     Function to execute when selected with modifiers (<see cref="NC_ALT_FUNC_MODIFIER" />)
		/// </summary>
		public virtual NConsoleFunction? AltFunction { get; set; }

		/// <summary>
		///     Function to execute when selected with modifiers (<see cref="NC_CTRL_FUNC_MODIFIER" />)
		/// </summary>
		public virtual NConsoleFunction? CtrlFunction { get; set; }

		/// <summary>
		///     Information about this <see cref="NConsoleOption" />
		/// </summary>
		public virtual string? Data { get; set; }

		/// <summary>
		///     Display color
		/// </summary>
		public virtual Color Color { get; set; } = DefaultOptionColor;

		public static void EnsureOption(ref NConsoleOption? option)
		{
			option ??= Wait;
		}

		public static NConsoleOption[] CreateOptionsFromEnum<TEnum>() where TEnum : Enum
		{
			var options = (TEnum[]) Enum.GetValues(typeof(TEnum));
			var rg = new NConsoleOption[options.Length];

			for (int i = 0; i < rg.Length; i++) {
				var option = options[i];
				string name = Enum.GetName(typeof(TEnum), option)!;

				rg[i] = new NConsoleOption
				{
					Name = name,
					Function = () => option
				};
			}

			return rg;

		}
	}
}