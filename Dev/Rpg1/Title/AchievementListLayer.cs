using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ace;
using Rpg1.Ace.Common;
using Rpg1.Model.Entities;
using Rpg1.Model.Title;

namespace Rpg1.Ace.Title
{
	class AchievementListLayer : Layer2D, IDisposable
	{
		public static readonly int LineNum = 6;
		public static readonly Vector2DF ListPosition = new Vector2DF(100, 140);
		public static readonly int LineHeight = 42;

		TextureObject2D selector;
		CameraObject2D camera;
		Choice<Keys> choice;

		public int Offset { get; private set; }
		public Action<int> OnMove { get; set; }

		public AchievementListLayer(AchievementContext context)
		{
			var trophyTexture = new Dictionary<StatsColor, Texture2D>()
			{
				{ StatsColor.Silver, Engine.Graphics.CreateTexture2D("Texture/SilverTrophy.png") },
				{ StatsColor.Gold, Engine.Graphics.CreateTexture2D("Texture/GoldTrophy.png") },
			};
			var font = Engine.Graphics.CreateFont(Def.BasicFont);

			selector = new TextureObject2D()
			{
				Texture = Engine.Graphics.CreateTexture2D("Texture/Title/AchievementSelector.png"),
				Position = new Vector2DF(-16, 0),
				CenterPosition = new Vector2DF(0, 16),
			};
			AddObject(selector);
			selector.AddComponent(
				new TimeAnimationComponent(t => selector.Src = new RectF(0, (t * 25) % 32, 280, 32)),
				"Animation");

			var status = context.Status.ToArray();
			for(int i = 0; i < status.Length; i++)
			{
				var y = (i - Offset) * LineHeight;
				var color = status[i].IsGotten ? new Color(255, 255, 255, 255) : new Color(128, 128, 128, 200);

				var trophy = new TextureObject2D()
				{
					Texture = trophyTexture[status[i].Achievement.Color],
					Position = new Vector2DF(0, y),
					Color = color,
				};
				trophy.SetCenterPosition(CenterPosition.CenterLeft);
				AddObject(trophy);

				var name = new TextObject2D()
				{
					Font = font,
					Text = status[i].IsGotten ? status[i].Achievement.Title : "？？？？？？？？",
					Position = new Vector2DF(42, y),
					Color = color,
				};
				name.SetCenterPosition(CenterPosition.CenterLeft);
				AddObject(name);
			}

			camera = new CameraObject2D()
			{
				Src = new RectI(-16, -LineHeight / 2, 400, LineHeight * LineNum),
				Dst = new RectI((int)ListPosition.X, (int)ListPosition.Y, 400, LineHeight * LineNum),
			};
			AddObject(camera);

			choice = new Choice<Keys>(status.Length, false, k => Engine.Keyboard.GetKeyState(k) == KeyState.Hold);
			choice.Set(Keys.Up, ChoiceControll.Previous);
			choice.Set(Keys.Down, ChoiceControll.Next);
			choice.Hold = new ChoiceHoldOption(25, 8);
			choice.OnMove += Choice_OnMove;
		}

		private void Choice_OnMove(int index)
		{
			selector.AddComponent(
				EasingComponent.CreateYEasing(selector, LineHeight * index, umw.EasingStart.StartRapidly3, umw.EasingEnd.EndSlowly3, 10),
				"Move");

			if(index >= Offset + LineNum)
			{
				Offset++;
				camera.AddComponent(
					new EasingComponent(
						() => camera.Src.Y,
						v => camera.Src = new RectI(camera.Src.X, (int)v, camera.Src.Width, camera.Src.Height),
						Offset * LineHeight - LineHeight / 2,
						umw.EasingStart.StartRapidly3,
						umw.EasingEnd.EndSlowly3,
						10),
					"Move");
			}
			if(index < Offset)
			{
				Offset--;
				camera.AddComponent(
					new EasingComponent(
						() => camera.Src.Y,
						v => camera.Src = new RectI(camera.Src.X, (int)v, camera.Src.Width, camera.Src.Height),
						Offset * LineHeight - LineHeight / 2,
						umw.EasingStart.StartRapidly3,
						umw.EasingEnd.EndSlowly3,
						10),
					"Move");
			}

			if(OnMove != null)
			{
				OnMove(index);
			}
		}

		protected override void OnUpdated()
		{
			choice.Update();
		}

		public void Dispose()
		{
			Vanish();
		}
	}
}
