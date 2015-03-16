using System.Runtime.Serialization;
using Rpg1.Model.Entities;

namespace Rpg1.Model.Battle.AI
{
	[DataContract]
	internal class RepeatAIPauseData : IAIPauseData
	{
		[DataMember]
		public SkillKind? PreviousSkill { get; set; }
	}
}