namespace Rpg1.Model.Battle.Messages
{
	public class DeathOfBattler : IMessage
	{
		public int Index { get; private set; }

		public DeathOfBattler(int index)
		{
			this.Index = index;
		}
	}
}