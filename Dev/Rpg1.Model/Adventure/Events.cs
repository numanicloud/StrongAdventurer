using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Adventure.Messages;
using Rpg1.Model.Entities;
using Rpg1.Model.Repository;

namespace Rpg1.Model.Adventure
{
	static class Events
	{
		public static IEnumerable<IMessage> TryOpenAchievement(DataBase db, Def.AchievementId id)
		{
			return TryOpenAchievement(db, (int)id);
		}

		public static IEnumerable<IMessage> TryOpenAchievement(DataBase db, int id)
		{
			if(!db.SaveData.Achievements.Contains(id))
			{
				db.SaveData.Achievements.Add(id);
				ScoreRepository.Save(db.SaveData, Def.DoEncrypt);
				yield return new GetAchievementMessage(db.Achievements.First(x => x.Id == id));
			}
		}
	}
}
