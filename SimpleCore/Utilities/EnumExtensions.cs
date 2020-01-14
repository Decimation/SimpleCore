using SimpleCore.Formatting;

namespace SimpleCore.Utilities
{
	public static class EnumExtensions
	{
		public static bool HasFlagFast(this HexOptions value, HexOptions flag) => (value & flag) != 0;
	}
}