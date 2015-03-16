using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Entities;
using Rpg1.Model.Messages;
using Rpg1.Model.Repository;

namespace Rpg1.Model.Title
{
	public class TitleFlow
	{
		public IEnumerable<IMessage> GetFlow()
		{
			var choices = new List<TitleMenuChoice>()
			{
				new TitleMenuChoice { ImagePath = "Start.png", GetFlow = GetStartFlow },
				new TitleMenuChoice { ImagePath = "Continue.png", GetFlow = GetContinueFlow },
				new TitleMenuChoice { ImagePath = "Score.png", GetFlow = GetScoreFlow },
				new TitleMenuChoice { ImagePath = "Achievement.png", GetFlow = GetAchievementFlow },
				new TitleMenuChoice { ImagePath = "Quit.png", GetFlow = GetQuitFlow },
			};

			if(!File.Exists(Def.PauseDataPath))
			{
				choices[1].IsEnabled = false;
			}

			var input = new TitleMenuMessage(choices.ToArray());
			yield return input;

			foreach(var item in input.Response.GetFlow())
			{
				yield return item;
			}
		}

		public IEnumerable<IMessage> GetScoreSceneFlow()
		{
			yield return new WaitScoreSceneFinishedMessage();

			var title = new TitleFlow();
			yield return new ChangeFlowMessage<object>(title.GetFlow(), null);
		}

		private IEnumerable<IMessage> GetAchievementFlow()
		{
			var achievements = AchievementRepository.Load();
			var gotten = ScoreRepository.Load(Def.DoEncrypt).Achievements;
			var context = new AchievementContext(achievements, gotten);
			yield return new ChangeContextMessage<AchievementContext>(context);

			yield return new WaitAchievementSceneFinished();

			var title = new TitleFlow();
			yield return new ChangeFlowMessage<object>(title.GetFlow(), null);
		}

		private IEnumerable<IMessage> GetQuitFlow()
		{
			yield return new TerminateMessage();
		}

		private IEnumerable<IMessage> GetScoreFlow()
		{
			var score = ScoreRepository.Load(Def.DoEncrypt);
			var context = new ScoreContext { Scores = score.Scores };
			yield return new ChangeContextMessage<ScoreContext>(context);

			yield return new WaitScoreSceneFinishedMessage();

			var title = new TitleFlow();
			yield return new ChangeFlowMessage<object>(title.GetFlow(), null);
		}

		private IEnumerable<IMessage> GetContinueFlow()
		{
			yield return new ChangeToLoadSceneMessage();

			var db = DataBase.CreateDB();
			var repo = new Repository.PauseRepository();
			repo.DoEncrypt = Def.DoEncrypt;
			var pause = repo.Load();
			var flow = new Adventure.AdventureFlow(db, pause);

			File.Delete(Def.PauseDataPath);
			yield return new ChangeFlowMessage<Adventure.AdventureContext>(flow.GetFlow(pause), flow.Context);
		}

		private IEnumerable<IMessage> GetStartFlow()
		{
			yield return new ChangeToLoadSceneMessage();
			var db = DataBase.CreateDB();
			var flow = new Adventure.AdventureFlow(db);
			yield return new ChangeFlowMessage<Adventure.AdventureContext>(flow.GetFlow(), flow.Context);
		}
	}
}
