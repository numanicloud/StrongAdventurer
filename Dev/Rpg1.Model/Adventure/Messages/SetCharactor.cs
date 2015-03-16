namespace Rpg1.Model.Adventure.Messages
{
	public class SetCharactor : IMessage
	{
		public string ImagePath { get; private set; }

		public SetCharactor(string imagePath)
		{
			ImagePath = imagePath;
		}
	}
}