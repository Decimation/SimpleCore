#nullable enable
using System;
using System.Drawing;

// ReSharper disable UnusedMember.Global


#pragma warning disable HAA0601 // Value type to reference type conversion causing boxing allocation
#pragma warning disable HAA0602 // Delegate on struct instance caused a boxing allocation
#pragma warning disable HAA0603 // Delegate allocation from a method group
#pragma warning disable HAA0604 // Delegate allocation from a method group

#pragma warning disable HAA0501 // Explicit new array type allocation
#pragma warning disable HAA0502 // Explicit new reference type allocation
#pragma warning disable HAA0503 // Explicit new reference type allocation
#pragma warning disable HAA0504 // Implicit new array creation allocation
#pragma warning disable HAA0505 // Initializer reference type allocation
#pragma warning disable HAA0506 // Let clause induced allocation

#pragma warning disable HAA0301 // Closure Allocation Source
#pragma warning disable HAA0302 // Display class allocation to capture closure
#pragma warning disable HAA0303 // Lambda or anonymous method in a generic method allocates a delegate instance


#pragma warning disable CS8618

namespace SimpleCore.Console.CommandLine
{
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


		public static NConsoleOption[] FromArray<T>(T[] values, Func<T, string> getName)
		{
			var rg = new NConsoleOption[values.Length];

			for (int i = 0; i < rg.Length; i++) {
				var option = values[i];

				var name = getName(option);

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