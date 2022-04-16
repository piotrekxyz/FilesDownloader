using FilesDownloader.ViewModel;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;

namespace FilesDownloader.Models
{
	public class DownloadFile : INotifyPropertyChanged
	{
		private string _filename;
		public string Filename
		{
			get { return _filename; }
			set
			{
				_filename = value;
				OnPropertyChanged(nameof(Filename));
			}
		}

		private string _downloadLocation;
		public string DownloadLocation
		{
			get { return _downloadLocation; }
			set
			{
				_downloadLocation = value;
				OnPropertyChanged(nameof(DownloadLocation));
			}
		}

		private string _uri;
		public string Uri
		{
			get { return _uri; }
			set
			{
				FileSize = GetFileSizeString(value);
				_uri = value;
				Filename = value.Split('/').Last();
				OnPropertyChanged(nameof(Uri));
			}
		}

		private string _downloadPercentageString;
		public string DownloadPercentageString
		{
			get { return _downloadPercentageString; }
			set
			{
				_downloadPercentageString = value;
				OnPropertyChanged(nameof(DownloadPercentageString));
			}
		}

		private int _downloadPercentage;
		public int DownloadPercentage
		{
			get { return _downloadPercentage; }
			set
			{
				_downloadPercentage = value;
				OnPropertyChanged(nameof(DownloadPercentage));
			}
		}

		private string _downloadTime;
		public string DownloadTime
		{
			get { return _downloadTime; }
			set
			{
				_downloadTime = value;
				OnPropertyChanged(nameof(DownloadTime));
			}
		}

		private string _remainingTime;
		public string RemainingTime
		{
			get { return _remainingTime; }
			set
			{
				_remainingTime = value;
				OnPropertyChanged(nameof(RemainingTime));
			}
		}

		private string _downloadSpeed;
		public string DownloadSpeed
		{
			get { return _downloadSpeed; }
			set
			{
				_downloadSpeed = value;
				OnPropertyChanged(nameof(DownloadSpeed));
			}
		}

		private string _fileSize;
		public string FileSize
		{
			get { return _fileSize; }
			set
			{
				_fileSize = value;
				OnPropertyChanged(nameof(FileSize));
			}
		}

		private DownloadFileStatus _downloadStatus;
		public DownloadFileStatus DownloadStatus
		{
			get { return _downloadStatus; }
			set
			{
				_downloadStatus = value;
				DownloadFileViewModel.CanDownload(this);
				OnPropertyChanged(nameof(DownloadStatus));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private string GetFileSizeString(string uri)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
				request.Method = "HEAD";
				HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
				var bytesToMBbytes = 1048576;
				return string.Format("{0} MB", Math.Round((double)resp.ContentLength / bytesToMBbytes, 2));
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return null;
			}
		}
	}
}
