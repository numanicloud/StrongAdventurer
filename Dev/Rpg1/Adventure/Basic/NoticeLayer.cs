using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ace;
using Rpg1.Ace.Common;
using Rpg1.Model.Adventure;
using Rpg1.Model.Entities;

namespace Rpg1.Ace.Adventure.Basic
{
	class NoticeLayer : Layer2D
	{
		private static readonly Vector2DF MessageWindowPosition = new Vector2DF(0, 300);
		private static readonly Vector2DF MessagePosition = new Vector2DF(10, 10);

		private MessageWindow messageWindow { get; set; }
		private SoundSource getAchievementSound;

		public NoticeLayer(AdventureContext context)
		{
			messageWindow = new MessageWindow()
			{
				Position = MessageWindowPosition,
				MessageFont = Engine.Graphics.CreateFont(Def.BasicFont),
				MessagePosition = MessagePosition,
				Texture = Engine.Graphics.CreateTexture2D("Texture/Adventure/MessageWindow.png"),
				WaitOnChar = 1,
				DrawingPriority = 1,
			};
			AddObject(messageWindow);

			var roundCounter = new Counter<AdventureContext>(
				context,
				x => x.RoundCount,
				Engine.Graphics.CreateTexture2D("Texture/Adventure/RoundCounter.png"));
			roundCounter.Position = new Vector2DF(-96, 5);
			roundCounter.DrawingPriority = 1;
			AddObject(roundCounter);

			roundCounter.AddComponent(
				new EasingComponent(
					() => roundCounter.Position.X,
					v => roundCounter.Position = new Vector2DF(v, roundCounter.Position.Y),
					0,
					umw.EasingStart.StartRapidly2,
					umw.EasingEnd.EndSlowly2,
					30),
				"SlideIn");

			getAchievementSound = Engine.Sound.CreateSoundSource("Sound/ata_a52.wav", true);
		}

		public void ShowMessage(string message)
		{
			messageWindow.ShowMessage(message);
		}

		public void ShowMessage(string message, Action callback)
		{
			messageWindow.ShowMessage(message, callback);
		}

		public void ShowAchivement(Achievement achievement)
		{
			Engine.Sound.Play(getAchievementSound);
			AddObject(new AchivementNotice(achievement) { Position = new Vector2DF(0, 250), });
		}
	}
}
