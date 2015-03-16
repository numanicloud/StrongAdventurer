using System;
using ace;
using Rpg1.Ace.Common;
using Rpg1.Model.Adventure;

namespace Rpg1.Ace.Adventure.Basic
{
	class AdventureLayer : Layer2D
	{
		public static readonly Vector2DF PlayerStatusWindowPosition = new Vector2DF(Program.WindowSize.X / 2 - 90, Program.WindowSize.Y - 10 - 60);

		PlayerStatusWindow playerStatus;
		TextureObject2D charactor;

		public AdventureLayer(AdventureContext context)
		{
			playerStatus = new PlayerStatusWindow(context.Player)
			{
				Position = PlayerStatusWindowPosition
			};
			AddObject(playerStatus);
		}

		public void SetCharactor(Texture2D texture, Action callback)
		{
			const string FadeInName = "FadeIn";

			Action<Texture2D, Action> show = (tex, c) =>
			{
				if(tex == null)
				{
					c();
					return;
				}

				charactor = new TextureObject2D()
				{
					Texture = tex,
					Position = BattleLayer.EnemyPosition,
					Color = new Color(255, 255, 255, 0),
				};
				charactor.SetCenterPosition(CenterPosition.BottomCenter);
				AddObject(charactor);
				charactor.AddComponent(
					EasingComponent.CreateAlphaEasing(
						charactor,
						255,
						umw.EasingStart.StartRapidly2,
						umw.EasingEnd.EndSlowly2,
						30,
						c), FadeInName);
			};

			if(charactor != null)
			{
				charactor.RemoveComponent(FadeInName);
				charactor.AddComponent(
					EasingComponent.CreateAlphaEasing(
						charactor,
						0,
						umw.EasingStart.StartRapidly2,
						umw.EasingEnd.EndSlowly2,
						30,
						() =>
				{
					charactor.Vanish();
					charactor = null;
					show(texture, callback);
				}), "FadeOut");
			}
			else
			{
				show(texture, callback);
			}
		}

		public void ShowHealing(int amount)
		{
			playerStatus.ShowHealing(amount);
		}
	}
}
