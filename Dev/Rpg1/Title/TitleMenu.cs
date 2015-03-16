using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ace;
using Rpg1.Ace.Common;
using Rpg1.Model.Title;

namespace Rpg1.Ace.Title
{
	class TitleMenu : TextureObject2D
	{
		private static readonly Vector2DF MenuPosition = new Vector2DF(Program.WindowSize.X / 2, 210);

		private TitleMenuChoice[] choices;
		List<TextureObject2D> choiceObjects;
		TextureObject2D selector;
		Choice<Keys> choice;
		Action<TitleMenuChoice> callback;
		SoundSource moveSound, decideSound;


		public TitleMenu(TitleMenuChoice[] choices, Action<TitleMenuChoice> callback)
		{
			this.choices = choices;
			this.callback = callback;
			choiceObjects = new List<TextureObject2D>();
		}

		protected override void OnStart()
		{
			Position = MenuPosition;
			IsDrawn = false;

			moveSound = Engine.Sound.CreateSoundSource(Def.MoveSound, true);
			decideSound = Engine.Sound.CreateSoundSource(Def.DecideSound, true);

			choices.ForEach((c, i) =>
			{
				var obj = new TextureObject2D()
				{
					Position = new Vector2DF(0, i * 55),
					DrawingPriority = 2,
					Texture = Engine.Graphics.CreateTexture2D(Path.Combine("Texture/Title/", c.ImagePath)),
					Color = c.IsEnabled ? new Color(255, 255, 255, 0) : new Color(100, 100, 100, 0),
				};
				obj.CenterPosition = obj.Texture.Size.ToFloat() / 2;
				AddChild(obj, ChildMode.Position);
				Layer.AddObject(obj);
				choiceObjects.Add(obj);

				obj.AddComponent(
					EasingComponent.CreateAlphaEasing(
						obj,
						c.IsEnabled ? (byte)255 : (byte)200,
						umw.EasingStart.StartRapidly3,
						umw.EasingEnd.EndSlowly3,
						30), "FadeIn");
			});

			selector = new TextureObject2D()
			{
				Position = choiceObjects[0].Position,
				Texture = Engine.Graphics.CreateTexture2D("Texture/Title/Selector.png"),
				DrawingPriority = 1,
				AlphaBlend = AlphaBlendMode.Mul,
			};
			selector.CenterPosition = new Vector2DF(160, 25);
			AddChild(selector, ChildMode.Position);
			Layer.AddObject(selector);

			selector.AddComponent(
				EasingComponent.CreateAlphaEasing(
					selector,
					255,
					umw.EasingStart.StartRapidly3,
					umw.EasingEnd.EndSlowly3,
					30), "FadeIn");

			selector.AddComponent(
				new TimeAnimationComponent(
					t => selector.Src = new RectF(0, (t * 25) % 50, 320, 50)),
				"SelectorAnimation");

			choice = new Choice<Keys>(choices.Length, true, x => Engine.Keyboard.GetKeyState(x) == KeyState.Hold);
			choice.Set(Keys.Up, ChoiceControll.Previous);
			choice.Set(Keys.Down, ChoiceControll.Next);
			choice.Set(Keys.Z, ChoiceControll.Decide);
			choice.Hold = new ChoiceHoldOption(40, 10);
			choice.OnMove += Choice_OnMove;
			choice.OnDecide += Choice_OnDecide;
		}

		private Color ChangeAlpha(Color source, byte a)
		{
			return new Color(source.R, source.G, source.B, a);
		}

		private void Choice_OnDecide(int index)
		{
			if(!choices[index].IsEnabled)
			{
				return;
			}

			Engine.Sound.Play(decideSound);
			callback(choices[index]);
			IsUpdated = false;

			Vanish();
			choiceObjects.ForEach(x => x.Vanish());
			selector.Vanish();
		}

		private void Choice_OnMove(int index)
		{
			Engine.Sound.Play(moveSound);

			selector.RemoveComponent("OnMove");

			selector.AddComponent(
				new EasingComponent(
					() => selector.Position.Y,
					v => selector.Position = new Vector2DF(selector.Position.X, v),
					choiceObjects[index].Position.Y,
					umw.EasingStart.StartRapidly3,
					umw.EasingEnd.EndSlowly2,
					10),
				"OnMove");
		}

		protected override void OnUpdate()
		{
			choice.Update();
		}
	}
}
