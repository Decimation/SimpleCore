#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using SimpleCore.Model;

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
		public virtual IViewable? Data { get; set; }


		public virtual Color? Color { get; set; }

		public static List<NConsoleOption> FromList<T>(IList<T> values) =>
			FromArray(values, arg => arg!.ToString()!).ToList();

		protected bool Equals(NConsoleOption other)
		{
			return Name == other.Name && Function.Equals(other.Function) && Equals(AltFunction, other.AltFunction) &&
			       Equals(CtrlFunction, other.CtrlFunction) && Equals(ShiftFunction, other.ShiftFunction) &&
			       Equals(ComboFunction, other.ComboFunction) && Equals(Data, other.Data) &&
			       Nullable.Equals(Color, other.Color);
		}

		public override bool Equals(object? obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((NConsoleOption) obj);
		}

		public override int GetHashCode()
		{
			unchecked {
				int hashCode = Name.GetHashCode();
				hashCode = (hashCode * 397) ^ Function.GetHashCode();
				hashCode = (hashCode * 397) ^ (AltFunction   != null ? AltFunction.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (CtrlFunction  != null ? CtrlFunction.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (ShiftFunction != null ? ShiftFunction.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (ComboFunction != null ? ComboFunction.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Data          != null ? Data.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ Color.GetHashCode();
				return hashCode;
			}
		}

		public static NConsoleOption[] FromArray<T>(T[] values) => FromArray(values, arg => arg!.ToString()!);

		public static NConsoleOption[] FromArray<T>(IList<T> values, Func<T, string> getName)
		{
			var rg = new NConsoleOption[values.Count];

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