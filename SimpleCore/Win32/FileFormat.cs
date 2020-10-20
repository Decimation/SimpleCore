namespace SimpleCore.Win32
{
	/// <summary>
	/// Represents a file format or type.
	/// </summary>
	public enum FileFormat
	{
		/// <summary>
		/// Joint Photographic Experts Group
		/// </summary>
		/// <remarks>JPEG/JPG</remarks>
		JPEG_RAW,

		/// <summary>
		/// Joint Photographic Experts Group
		/// </summary>
		/// <remarks>JPEG/JPG/JFIF</remarks>
		JPEG_JFIF,

		/// <summary>
		/// Portable Network Graphics
		/// </summary>
		/// <remarks>PNG</remarks>
		PNG,

		/// <summary>
		/// Graphical Interchange Format
		/// </summary>
		/// <remarks>GIF</remarks>
		GIF,

		/// <summary>
		/// Bitmap
		/// </summary>
		/// <remarks>BMP</remarks>
		BMP,

		/// <summary>
		/// (Unknown)
		/// </summary>
		Unknown
	}
}