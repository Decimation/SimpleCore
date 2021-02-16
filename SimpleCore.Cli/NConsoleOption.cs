#nullable enable
using System;
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
	public interface NConsoleOption
	{
		/// <summary>
		///     <see cref="NConsoleOption.AltFunction" />
		/// </summary>
		public const ConsoleModifiers NC_ALT_FUNC_MODIFIER = ConsoleModifiers.Alt;

		/// <summary>
		///     <see cref="NConsoleOption.CtrlFunction" />
		/// </summary>
		public const ConsoleModifiers NC_CTRL_FUNC_MODIFIER = ConsoleModifiers.Control;

		/// <summary>
		///     <see cref="NConsoleOption.ComboFunction" />
		/// </summary>
		public const ConsoleModifiers NC_COMBO_FUNC_MODIFIER = NC_ALT_FUNC_MODIFIER | NC_CTRL_FUNC_MODIFIER;

		/// <summary>
		///     Default <see cref="Color" />
		/// </summary>
		public static readonly Color DefaultOptionColor = Color.White;


		/// <summary>
		///     Display name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///     Function to execute when selected
		/// </summary>
		public NConsoleFunction Function { get; set; }

		/// <summary>
		///     Function to execute when selected with modifiers (<see cref="NC_ALT_FUNC_MODIFIER" />)
		/// </summary>
		public NConsoleFunction? AltFunction { get; set; }

		/// <summary>
		///     Function to execute when selected with modifiers (<see cref="NC_CTRL_FUNC_MODIFIER" />)
		/// </summary>
		public NConsoleFunction? CtrlFunction { get; set; }

		/// <summary>
		///     Function to execute when selected with modifiers (<see cref="NC_COMBO_FUNC_MODIFIER" />)
		/// </summary>
		public NConsoleFunction? ComboFunction { get; set; }

		/// <summary>
		///     Information about this <see cref="NConsoleOption" />
		/// </summary>
		public string? Data { get; set; }

		/// <summary>
		///     Display color
		/// </summary>
		public Color Color { get; set; }

		public static NConsoleOption[] FromArray<T>(T[] values, Func<T, string> getName)
		{
			var rg = new NConsoleOption[values.Length];

			for (int i = 0; i < rg.Length; i++) {
				var option = values[i];

				var name = getName(option);

				rg[i] = new NConsoleOptionBasic
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

	public class NConsoleOptionBasic : NConsoleOption
	{
		public virtual string Name { get; set; }

		public virtual NConsoleFunction Function { get; set; }

		public virtual NConsoleFunction? AltFunction { get; set; }

		public virtual NConsoleFunction? CtrlFunction { get; set; }

		public virtual NConsoleFunction? ComboFunction { get; set; }

		public virtual string? Data { get; set; }

		public virtual Color Color { get; set; } = NConsoleOption.DefaultOptionColor;
	}
}