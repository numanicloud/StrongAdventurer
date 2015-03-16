using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rpg1.Model.Entities
{
	[DataContract]
	public class Score
	{
		[DataMember]
		public int[] Scores { get; set; }

		[DataMember]
		public List<int> Achievements { get; set; }

		[DataMember]
		public int GameOverCount { get; set; }

		[DataMember]
		public int FairyCount { get; set; }

		[DataMember]
		public int Round20Count { get; set; }

		[DataMember]
		public int NightKingCount { get; set; }

		[DataMember]
		public HashSet<int> OvercomedEnemies { get; set; }

		public static Score CreateInitial()
		{
			return new Score
			{
				Scores = new int[] { 20, 16, 12, 8, 4 },
				GameOverCount = 0,
				Achievements = new List<int>(),
				OvercomedEnemies = new HashSet<int>(),
			};
		}
	}
}
