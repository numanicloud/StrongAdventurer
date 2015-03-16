using System;
using Rpg1.Model.Entities;

namespace Rpg1.Model.Battle.Messages
{
	public class ChooseSkill : IResponse<BattleCommand>
	{
		public BattleCommand Response { get; set; }
	}
}