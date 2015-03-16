using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Entities;

namespace Rpg1.Model.Battle.AI
{
	[DataContract]
	class CheckingAIPauseData : IAIPauseData
	{
		[DataMember]
		public SkillKind? PreviousSkill { get; set; }
		[DataMember]
		public SkillKind? PreviousRivalsSkill { get; set; }
	}
}
