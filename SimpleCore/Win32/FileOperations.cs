﻿using System.IO;
using SimpleCore.Utilities;

// ReSharper disable UnusedMember.Global

#pragma warning disable HAA0502, HAA0301, HAA0302, HAA0501

namespace SimpleCore.Win32
{
	/// <summary>
	///     Utilities for working with the file system, files, etc.
	/// </summary>
	/// <seealso cref="File" />
	/// <seealso cref="FileInfo" />
	/// <seealso cref="Directory" />
	/// <seealso cref="DirectoryInfo" />
	/// <seealso cref="Path" />
	public static class FileOperations
	{
		/// <summary>
		///     Determines the file size (not size on disk) of <paramref name="file" />
		/// </summary>
		/// <param name="file">File location</param>
		/// <returns>Size of the file, in bytes</returns>
		public static long GetFileSize(string file)
		{
			var f = new FileInfo(file);

			return f.Length;
		}

		/// <summary>
		///     Attempts to determine the file format (type) given a file.
		/// </summary>
		/// <param name="file">File whose type to resolve</param>
		/// <returns>
		///     The best <see cref="FileFormat" /> match; <see cref="FileFormat.Unknown" /> if the type could not be
		///     determined.
		/// </returns>
		public static FileFormat ResolveFileType(string file)
		{
			return ResolveFileType(File.ReadAllBytes(file));
		}

		/// <summary>
		///     Attempts to determine the file format (type) given the raw bytes of a file
		///     by comparing file format magic bytes.
		/// </summary>
		/// <param name="fileBytes">Raw file bytes</param>
		/// <returns>
		///     The best <see cref="FileFormat" /> match; <see cref="FileFormat.Unknown" /> if the type could not be
		///     determined.
		/// </returns>
		public static FileFormat ResolveFileType(byte[] fileBytes)
		{
			// todo: FileIdentity, FileSequence, etc


			/*
			 * JPEG RAW
			 */

			var jpegStart = new byte[] {0xFF, 0xD8, 0xFF, 0xDB};
			var jpegEnd = new byte[] {0xFF, 0xD9};

			if (fileBytes.StartsWith(jpegStart) && fileBytes.EndsWith(jpegEnd)) {
				return FileFormat.JPEG_RAW;
			}

			/*
			 * JPEG JFIF
			 */

			var jpegJfif = new byte[] {0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46, 0x00, 0x01};

			if (fileBytes.StartsWith(jpegJfif)) {
				return FileFormat.JPEG_JFIF;
			}

			/*
			 * PNG
			 */

			var png = new byte[] {0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A};

			if (fileBytes.StartsWith(png)) {
				return FileFormat.PNG;
			}

			/*
			 * GIF
			 */

			var gif = new byte[] {0x47, 0x49, 0x46, 0x38};

			if (fileBytes.StartsWith(gif)) {
				return FileFormat.GIF;
			}


			/*
			 * BMP
			 */

			var bmp = new byte[] {0x42, 0x4D};

			if (fileBytes.StartsWith(bmp)) {
				return FileFormat.BMP;
			}

			/*
			 * Unknown
			 */

			return FileFormat.Unknown;
		}

		public static string CreateRandomName()
		{
			return Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
		}
	}
}