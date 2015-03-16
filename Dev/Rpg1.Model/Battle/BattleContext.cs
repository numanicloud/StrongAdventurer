using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Entities;

namespace Rpg1.Model.Battle
{
	public class BattleContext : NotificationObject
	{
		private int turn_;

		public DataBase Game { get; set; }
		public PlayerBattler Player { get; set; }
		public EnemyBattler Enemy { get; set; }
		public int RoundCount { get; set; }
		public int Turn
		{
			get { return turn_; }
			set
			{
				if(turn_ != value)
				{
					turn_ = value;
					Raise();
				}
			}
		}
		public bool HaveNotLose { get; set; }
	}
}
