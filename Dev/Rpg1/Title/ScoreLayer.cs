using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ace;
using Rpg1.Ace.Common;
using Rpg1.Model;
using Rpg1.Model.Title;

namespace Rpg1.Ace.Title
{
	class ScoreLayer : TitleSceneLayer
	{
		TextureObject2D title { get; set; }
		List<TextureObject2D> scores { get; set; }
		Action callback { get; set; }
		SoundSource finishSound { get; set; }

		public ScoreLayer(ScoreContext context)
		{
			scores = new List<TextureObject2D>();
			DrawingPriority = 1;

			var scoreBackground = Engine.Graphics.CreateTexture2D("Texture/Title/ScoreBack.png");
			var labelFont = Engine.Graphics.CreateFont(Def.BasicFont);
			var numberFont = Engine.Graphics.CreateFont("Font/ArialBold.aff");

			context.Scores.ForEach((x, i) =>
			{
				var color = context.CurrentRank - 1 == i ? new Color(255, 255, 0, 255) : new Color(255, 255, 255, 255);

				var obj = new TextureObject2D()
				{
					Texture = scoreBackground,
					Position = new Vector2DF(640, 120 + 55 * i),
				};
				scores.Add(obj);
				AddObject(obj);
				obj.AddComponent(
					new EasingComponent(
						() => obj.Position.X,
						v => obj.Position = new Vector2DF(v, obj.Position.Y),
						640 - 300,
						umw.EasingStart.StartRapidly2,
						umw.EasingEnd.EndSlowly2,
						30), "SlideIn");

				var text = new TextObject2D()
				{
					Font = labelFont,
					Text = string.Format("{0}位", i + 1),
					Position = new Vector2DF(14, 30),
					Color = color,
				};
				text.SetCenterPosition(CenterPosition.CenterLeft);
				obj.AddChild(text, ChildMode.Position);
				AddObject(text);

				var scoreText = new TextObject2D()
				{
					Font = numberFont,
					Text = x.ToString(),
					Position = new Vector2DF(150, 30),
					Color = color,
				};
				scoreText.SetCenterPosition(CenterPosition.CenterRight);
				obj.AddChild(scoreText, ChildMode.Position);
				AddObject(scoreText);
			});

			title = new TextureObject2D()
			{
				Texture = Engine.Graphics.CreateTexture2D("Texture/Title/ScoreSceneTitle.png"),
				Position = new Vector2DF(-320, 20),
				DrawingPriority = 1,
			};
			AddObject(title);
			title.AddComponent(
				EasingComponent.CreateXEasing(title, 0, umw.EasingStart.StartRapidly2, umw.EasingEnd.EndSlowly2, 30),
				"SlideIn");
			title.AddComponent(
				new TimeAnimationComponent(
					t => title.Position = new Vector2DF(title.Position.X, 20 + (float)Math.Sin(t) * 5)),
				"LogoWave");

			finishSound = Engine.Sound.CreateSoundSource(Def.DecideSound, true);
		}

		protected override void OnUpdated()
		{
			if(callback != null && Engine.Keyboard.GetKeyState(Keys.Z) == KeyState.Push)
			{
				Engine.Sound.Play(finishSound);
				callback();
				callback = null;
			}
		}

		public override void AddMessageHandlerTo(SteppingChannel<IMessage> channel)
		{
			channel.AddMessageHandler<WaitScoreSceneFinishedMessage>(StartWaitKey);
		}

		public override void Dispose()
		{
			foreach(var item in scores)
			{
				item.RemoveComponent("SlideIn");
				item.AddComponent(
					EasingComponent.CreateXEasing(
						item,
						Program.WindowSize.X,
						umw.EasingStart.StartRapidly1,
						umw.EasingEnd.EndSlowly1,
						30),
					"SlideOut");
			}
			title.RemoveComponent("SlideIn");
			title.RemoveComponent("LogoWave");
			title.AddComponent(
				EasingComponent.CreateYEasing(
					title,
					-title.Texture.Size.Y,
					umw.EasingStart.StartRapidly1,
					umw.EasingEnd.EndSlowly1,
					32,
					Vanish),
				"SlideOut");
		}

		private void StartWaitKey(WaitScoreSceneFinishedMessage msg, Action callback)
		{
			this.callback = callback;
		}
	}
}
