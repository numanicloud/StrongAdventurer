using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Entities;

namespace Rpg1.Model.Battle
{
	public class ActionContext
	{
		public BattleContext BattleContext { get; set; }
		public Battler Doer { get; set; }
		public Battler Target { get; set; }
	}
}
