namespace Lando
{
	public class CardreaderEventArgs
	{
		public ContactlessCard Card { get; set; }

		public string CardreaderName { get; set; }

		public string ReaderId { get; set; }

		public CardreaderEventArgs(ContactlessCard card)
		{
			Card = card;
		}

		public CardreaderEventArgs(string cardreaderName)
		{
			CardreaderName = cardreaderName;
			ReaderId = null;
		}

		public CardreaderEventArgs(string cardreaderName, string readerId)
		{
			CardreaderName = cardreaderName;
			ReaderId = readerId;
		}

		public CardreaderEventArgs(ContactlessCard card, string cardreaderName, string readerId)
		{
			Card = card;
			CardreaderName = cardreaderName;
			ReaderId = readerId;
		}
	}
}