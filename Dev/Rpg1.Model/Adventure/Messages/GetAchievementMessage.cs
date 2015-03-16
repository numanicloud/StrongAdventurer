using Rpg1.Model.Entities;

namespace Rpg1.Model.Adventure.Messages
{
	public class GetAchievementMessage : IMessage
	{
		public Achievement Achievement { get; private set; }

		public GetAchievementMessage(Achievement stats)
		{
			Achievement = stats;
		}
	}
}