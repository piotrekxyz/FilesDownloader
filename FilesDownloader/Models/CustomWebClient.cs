using System.Diagnostics;
using System.Net;

namespace FilesDownloader.Models
{
	[System.ComponentModel.DesignerCategory("")]
	public class CustomWebClient : WebClient
	{
		public object DownloadingFile { get; set; }
		public Stopwatch Timer { get; set; }
	}
}
