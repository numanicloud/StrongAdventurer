using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Battle;
using Rpg1.Model.Entities;
using Rpg1.Model.Messages;

namespace Rpg1.Model.BattleDebug
{
	class BattleDebugFlow
	{
		DataBase game { get;set; }

		public BattleDebugFlow(DataBase game)
		{
			this.game = game;
		}

		public IEnumerable<IMessage> GetFlow()
		{
			while(true)
			{
				var input = new ChoiceMessage<EnemiesAbility>(game.Enemies);

				var aContext = new Adventure.AdventureFlow(game).Context;
				var battle = new BattleFlow(aContext, input.Response);
				foreach(var item in battle.GetFlow(x => { }))
				{
					yield return item;
				}
			}
		}
	}
}
