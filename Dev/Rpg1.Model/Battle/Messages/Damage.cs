using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg1.Model.Battle.Messages
{
	public class Damage : IMessage
	{
		public Battler Battler { get; private set; }
		public int Amount { get; set; }

		public Damage(Battler battler, int amount)
		{
			Battler = battler;
			Amount = amount;
		}
	}
}
