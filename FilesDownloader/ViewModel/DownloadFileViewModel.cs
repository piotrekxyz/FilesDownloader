using FilesDownloader.Models;
using System;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Collections.ObjectModel;

namespace FilesDownloader.ViewModel
{
	public class DownloadFileViewModel
	{
		public string FileAddress { get; set; }
		public ObservableCollection<DownloadFile> DownloadFiles { get; } = new ObservableCollection<DownloadFile>();

		public RelayCommand DownloadCommand { get; }
		public RelayCommand OpenFileCommand { get; }
		public RelayCommand AddFileToListCommand { get; }

		public DownloadFileViewModel()
		{
			DownloadCommand = new RelayCommand(Download, CanDownload);
			OpenFileCommand = new RelayCommand(OpenFile, CanOpenFile);
			AddFileToListCommand = new RelayCommand(AddFileToList, CanAddFileToList);
		}

		private void OpenFile(object obj)
		{
			DownloadFile file = obj as DownloadFile;
			Process.Start("explorer.exe", string.Format("/select,\"{0}\"", file.DownloadLocation));
		}

		private bool CanOpenFile(object obj)
		{
			return true;
		}

		private void AddFileToList(object obj)
		{
			try
			{
				if (!DownloadFiles.Any(a => a.Uri == FileAddress))
					DownloadFiles.Add(new DownloadFile { Uri = FileAddress });
				else
					throw new DuplicateFileException();
			}
			catch (DuplicateFileException)
			{
			}
		}

		private bool CanAddFileToList(object obj)
		{
			return !string.IsNullOrEmpty(FileAddress) && !string.IsNullOrWhiteSpace(FileAddress) && FileAddress.Length > 10 && (FileAddress.StartsWith("http") || FileAddress.StartsWith("www"));
		}

		public static bool CanDownload(object obj)
		{
			DownloadFile file = obj as DownloadFile;
			return obj != null && file.DownloadStatus == DownloadFileStatus.NotDownloaded;
		}

		private void Download(object obj)
		{
			DownloadFile file = obj as DownloadFile;
			file.DownloadStatus = DownloadFileStatus.Downloading;

			CustomWebClient webClient = new CustomWebClient();
			webClient.DownloadingFile = file;
			webClient.Timer = new Stopwatch();
			webClient.DownloadProgressChanged += webClient_DownloadProgressChanged;
			webClient.DownloadFileCompleted += webClient_DownloadFileCompleted;
			webClient.Timer.Start();
			webClient.DownloadFileAsync(new Uri(file.Uri), file.DownloadLocation = string.Format("{0}\\{1}", Directory.GetCurrentDirectory(), file.Filename));
		}

		void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			CustomWebClient customWebClient = sender as CustomWebClient;
			DownloadFile downloadFile = customWebClient.DownloadingFile as DownloadFile;

			var bytesReceived = double.Parse(e.BytesReceived.ToString());
			var totalBytesToReceive = double.Parse(e.TotalBytesToReceive.ToString());
			var downloadPercentage = int.Parse(Math.Truncate(bytesReceived / totalBytesToReceive * 100).ToString());
			var totalSeconds = customWebClient.Timer.Elapsed.TotalSeconds;

			downloadFile.DownloadPercentage = downloadPercentage;
			downloadFile.DownloadPercentageString = $"{downloadPercentage} %";
			downloadFile.DownloadTime = string.Format("{0} s", int.Parse(Math.Truncate(totalSeconds).ToString()));
			var downloadSpeedInKb = e.BytesReceived / 1024d / totalSeconds;
			var downloadSpeed = e.BytesReceived / totalSeconds;
			downloadFile.DownloadSpeed = string.Format("{0} kb/s", downloadSpeedInKb.ToString("0.0"));
			downloadFile.RemainingTime = string.Format("{0} s", ((e.TotalBytesToReceive - e.BytesReceived) / downloadSpeed).ToString("0."));
		}

		void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			CustomWebClient customWebClient = sender as CustomWebClient;
			(customWebClient.DownloadingFile as DownloadFile).DownloadStatus = DownloadFileStatus.Downloaded;
			customWebClient.DownloadFileCompleted -= webClient_DownloadFileCompleted;
			customWebClient.DownloadProgressChanged -= webClient_DownloadProgressChanged;
		}
	}
}