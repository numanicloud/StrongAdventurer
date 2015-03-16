using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Battle.AI;

namespace Rpg1.Model.Entities
{
	[DataContract]
 	class PauseData
	{
		[DataMember]
		public int RoundCount { get; set; }
		[DataMember]
		public int Turn { get; set; }
		[DataMember]
		public int PlayerHp { get; set; }
		[DataMember]
		public int EnemyId { get; set; }
		[DataMember]
		public int EnemyHp { get; set; }
		[DataMember]
		public IAIPauseData EnemyAi { get; set; }
		[DataMember]
		public bool HaveNotLose { get; set; }
	}
}
