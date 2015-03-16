using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg1.Model.Entities
{
	public class EnemiesAbility : Ability
	{
		public int Id { get; set; }
		public string ImagePath { get; set; }
		public string AiId { get; set; }
		public int Start { get; set; }
		public int End { get; set; }
	}
}
