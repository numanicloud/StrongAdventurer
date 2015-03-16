using System.IO;
using ace;
using Rpg1.Model.Entities;
using Rpg1.Ace.Common;
using System;

namespace Rpg1.Ace.Adventure
{
	class AchivementNotice : TextureObject2D
	{
		TextureObject2D trophy;
		TextObject2D label, title;

		public AchivementNotice(Achievement achievement)
		{
			Texture = Engine.Graphics.CreateTexture2D("Texture/Adventure/AchievementBack.png");

			string trophyPath = null;
			switch(achievement.Color)
			{
			case StatsColor.Silver:
				trophyPath = "SilverTrophy.png";
				break;
			case StatsColor.Gold:
				trophyPath = "GoldTrophy.png";
				break;
			}

			trophy = new TextureObject2D()
			{
				Texture = Engine.Graphics.CreateTexture2D(Path.Combine("Texture/", trophyPath)),
				Position = new Vector2DF(24, 20),
			};
			trophy.SetCenterPosition(Common.CenterPosition.CenterCenter);

			var font = Engine.Graphics.CreateFont("Font/MPlusAchievement.aff");

			label = new TextObject2D()
			{
				Font = font,
				Text = "ŽÀÑ‰ðœI",
				Color = new Color(255, 255, 0, 255),
				Position = new Vector2DF(48, 0),
			};

			title = new TextObject2D()
			{
				Font = font,
				Text = achievement.Title,
				Position = new Vector2DF(48, 16),
			};
		}

		protected override void OnStart()
		{
			AddChild(trophy, ChildMode.Position);
			AddChild(label, ChildMode.Position);
			AddChild(title, ChildMode.Position);
			Layer.AddObject(trophy);
			Layer.AddObject(label);
			Layer.AddObject(title);

			Position = new Vector2DF(-180, Position.Y);

			Action slideOut = () => AddComponent(
				EasingComponent.CreateXEasing(this, -180, umw.EasingStart.StartRapidly2, umw.EasingEnd.EndSlowly2, 20, Vanish),
				"SlideOut");

			Action timer = () => AddComponent(new TimerComponent(5, slideOut), "Timer");

			AddComponent(
				EasingComponent.CreateXEasing(this, 0, umw.EasingStart.StartRapidly2, umw.EasingEnd.EndSlowly2, 20, timer),
				"SlideIn");
		}

		protected override void OnVanish()
		{
			trophy.Vanish();
			label.Vanish();
			title.Vanish();
		}
	}
}