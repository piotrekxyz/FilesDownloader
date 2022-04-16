using System;
using System.Windows;

namespace FilesDownloader.Models
{
	class DuplicateFileException : Exception
	{
		string info = "This file is already in the list";

		public DuplicateFileException()
		{
			MessageBox.Show(info);
		}
	}
}
