using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg1.Model.Battle.Messages
{
	public class HealMessage : IMessage
	{
		public int Amount { get; private set; }
		public Battler Battler { get; private set; }

		public HealMessage(Battler battler, int amount)
		{
			Battler = battler;
			Amount = amount;
		}
	}
}
