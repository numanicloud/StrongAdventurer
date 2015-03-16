using Rpg1.Model.Entities;

namespace Rpg1.Model.Adventure
{
	public enum DayOrNight
	{
		Morning, Noon, Evening, Night
	}

	public class AdventureContext : NotificationObject
	{
		private int roundCount_;

		public DataBase DataBase { get; set; }
		public Battle.PlayerBattler Player { get; set; }
		public int RoundCount
		{
			get { return roundCount_; }
			set
			{
				if(roundCount_ != value)
				{
					roundCount_ = value;
					Raise();
				}
			}
		}
		public DayOrNight DayOrNight { get; set; }
	}
}