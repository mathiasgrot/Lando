namespace Lando.Watcher
{
	internal class WatcherCardreaderEventArgs
	{
		public string CardreaderName { get; set; }

		public string ReaderId { get; set; }

		public WatcherCardreaderEventArgs(string cardreaderName)
		{
			CardreaderName = cardreaderName;
			ReaderId = null;
		}

		public WatcherCardreaderEventArgs(string cardreaderName, string readerId)
		{
			CardreaderName = cardreaderName;
			ReaderId = readerId;
		}
	}
}