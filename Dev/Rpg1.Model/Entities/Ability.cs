using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NacHelpers;

namespace Rpg1.Model.Entities
{
	public class Ability
	{
		private readonly int MaxMaxHp = 999;

		private int maxHp;

		public string Name { get; set; }
		public int MaxHp
		{
			get { return maxHp; }
			set { maxHp = Helper.Clamp(value, 0, MaxMaxHp); }
		}
	}
}
