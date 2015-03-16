using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NacHelpers;
using Rpg1.Model.Adventure;
using Rpg1.Model.Entities;

namespace Rpg1.Model
{
	static class RpgHelper
	{

		public static bool IsLose(SkillKind me, SkillKind rival)
		{
			return (3 + (int)me - (int)rival) % 3 == 2;
		}

		public static SkillKind RotateEnum(this SkillKind kind, int offset)
		{
			return (SkillKind)(((int)kind + offset) % 3);
		}

		public static IEnumerable<SkillKind> GetSkillExcept(SkillKind except)
		{
			return TypeHelper.GetValues<SkillKind>().Except(new[] { except });
		}


	}
}
