#nullable enable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SimpleCore.CommandLine
{
	/// <summary>
	/// Represents an interactive console/shell option
	/// </summary>
	public class NConsoleOption
	{
		/// <summary>
		/// Default <see cref="Color"/>
		/// </summary>
		public static readonly Color DefaultOptionColor = System.Drawing.Color.White;


		public NConsoleOption() { }

		/// <summary>
		/// Represents a <see cref="NConsoleOption"/> which is not yet available or in progress
		/// </summary>
		public static readonly NConsoleOption Wait = new NConsoleOption()
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
		/// Display name
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// Function to execute when selected
		/// </summary>
		public virtual Func<object?> Function { get; set; }

		/// <summary>
		/// Function to execute when selected with modifiers (<see cref="NConsole.IO.ALT_FUNC_MODIFIER"/>)
		/// </summary>
		public virtual Func<object?>? AltFunction { get; set; }


		/// <summary>
		/// Function to execute when selected with modifiers (<see cref="NConsole.IO.CTRL_FUNC_MODIFIER"/>)
		/// </summary>
		public virtual Func<object?>? CtrlFunction { get; set; }

		/// <summary>
		/// Information about this <see cref="NConsoleOption"/>
		/// </summary>
		public virtual string? Data { get; set; }

		/// <summary>
		/// Display color
		/// </summary>
		public virtual Color Color { get; set; } = DefaultOptionColor;


		public static void EnsureOption(ref NConsoleOption option)
		{
			option ??= NConsoleOption.Wait;
		}

		public static NConsoleOption[] CreateOptionsFromEnum<TEnum>() where TEnum : Enum
		{
			var options = (TEnum[]) Enum.GetValues(typeof(TEnum));
			var rg = new NConsoleOption[options.Length];

			for (int i = 0; i < rg.Length; i++) {
				var option = options[i];
				string name = Enum.GetName(typeof(TEnum), option)!;

				rg[i] = new NConsoleOption()
				{
					Name = name,
					Function = () => option
				};
			}


			return rg;


		}
	}
}