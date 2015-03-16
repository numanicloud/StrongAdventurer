using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ace;
using Rpg1.Ace.Common;
using Rpg1.Model;
using Rpg1.Model.Entities;
using Rpg1.Model.Title;

namespace Rpg1.Ace.Title
{
	class AchievementLayer : TitleSceneLayer
	{
		private static readonly Vector2DF ConditionTextPosition = new Vector2DF(30, 400);
		private static readonly Vector2DF PraiseTextPosition = new Vector2DF(30, 430);

		AchievementContext context;
		AchievementStatus[] status;
		Action callback;
		bool isFirstUpdate;

		AchievementListLayer listLayer;
		TextureObject2D title;
		TextObject2D conditionText;
		TextObject2D praiseText;
		TextureObject2D arrowUp;
		TextureObject2D arrowDown;
		SoundSource moveSound;
		SoundSource decideSound;

		public AchievementLayer(AchievementContext context)
		{
			this.context = context;
			isFirstUpdate = true;
			status = context.Status.ToArray();

			title = new TextureObject2D()
			{
				Texture = Engine.Graphics.CreateTexture2D("Texture/Title/AchievementTitle.png"),
				Position = new Vector2DF(-320, 0),
			};
			AddObject(title);
			title.AddComponent(
				EasingComponent.CreateXEasing(title, 0, umw.EasingStart.StartRapidly2, umw.EasingEnd.EndSlowly2, 30),
				"SlideIn");
			title.AddComponent(
				new TimeAnimationComponent(
					t => title.Position = new Vector2DF(title.Position.X, (float)Math.Sin(t) * 5)),
				"LogoWave");

			var font = Engine.Graphics.CreateFont(Def.BasicFont);

			listLayer = new AchievementListLayer(context);
			listLayer.OnMove = i =>
				{
					Engine.Sound.Play(moveSound);
					OnMove(i);
				};

			conditionText = new TextObject2D()
			{
				Font = font,
				Position = ConditionTextPosition,
			};
			AddObject(conditionText);

			praiseText = new TextObject2D()
			{
				Font = font,
				Position = PraiseTextPosition,
			};
			AddObject(praiseText);

			arrowUp = new TextureObject2D()
			{
				Texture = Engine.Graphics.CreateTexture2D("Texture/Title/ArrowUp.png"),
				Position = AchievementListLayer.ListPosition + new Vector2DF(-32, 9),
			};
			AddObject(arrowUp);

			arrowDown = new TextureObject2D()
			{
				Texture = Engine.Graphics.CreateTexture2D("Texture/Title/ArrowDown.png"),
				Position = AchievementListLayer.ListPosition + new Vector2DF(-32, 9 + (AchievementListLayer.LineNum - 1) * AchievementListLayer.LineHeight),
			};
			AddObject(arrowDown);

			OnMove(0);

			moveSound = Engine.Sound.CreateSoundSource(Def.MoveSound, true);
			decideSound = Engine.Sound.CreateSoundSource(Def.DecideSound, true);
		}

		public override void AddMessageHandlerTo(SteppingChannel<IMessage> channel)
		{
			channel.AddMessageHandler<Model.Title.WaitAchievementSceneFinished>(StartControll);
		}

		private void StartControll(WaitAchievementSceneFinished msg, Action callback)
		{
			this.callback = callback;
		}

		public override void Dispose()
		{
			Vanish();
			listLayer.Dispose();
		}

		protected override void OnUpdated()
		{
			if(isFirstUpdate)
			{
				Scene.AddLayer(listLayer);
				isFirstUpdate = false;
			}
			if(callback != null && Engine.Keyboard.GetKeyState(Program.DecideKey) == KeyState.Push)
			{
				Engine.Sound.Play(decideSound);
				callback();
				callback = null;
			}
		}

		private void OnMove(int index)
		{
			var a = status[index];
			conditionText.Text = a.Achievement.Description;
			praiseText.Text = a.IsGotten ? a.Achievement.PraiseText : "";

			arrowUp.IsDrawn = listLayer.Offset > 0;
			arrowDown.IsDrawn = listLayer.Offset < status.Length - AchievementListLayer.LineNum;
		}
	}
}
