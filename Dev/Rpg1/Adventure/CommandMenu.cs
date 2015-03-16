using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ace;
using Rpg1.Ace.Common;
using Rpg1.Model.Battle;
using Rpg1.Model.Entities;

namespace Rpg1.Ace.Adventure
{
	class CommandMenu : TextureObject2D
	{
		static readonly Vector2DF ContentsCenter = new Vector2DF(60, 0);
		static readonly Color ActiveColor = new Color(255, 255, 255, 255);
		static readonly Color InactiveColor = new Color(70, 70, 70, 200);
		static readonly float SlideInGoal = 32;
		static readonly string SlideInKey = "SlideIn";
		static readonly string SlideOutKey = "SlideOut";

		private Choice<Keys> choice;
		private TextureObject2D[] choices;
        private SoundSource selectSound, decideSound;
		private Action<BattleCommand> callback;
		private int previousSelectedIndex;
		private bool enabled;

		public CommandMenu(Action<BattleCommand> callback)
		{
			this.callback = callback;
			enabled = false;
		}

		protected override void OnStart()
		{
			Position = new Vector2DF(320, -45);

			choices = Enumerable.Range(0, 4)
				.Select(x => new TextureObject2D
			{
				Position = new Vector2DF((x-1)*145, -50),
				CenterPosition = ContentsCenter,
				Color = InactiveColor,
			}).ToArray();

			choices[0].Texture = Engine.Graphics.CreateTexture2D("Texture/Adventure/Attack.png");
			choices[1].Texture = Engine.Graphics.CreateTexture2D("Texture/Adventure/Counter.png");
			choices[2].Texture = Engine.Graphics.CreateTexture2D("Texture/Adventure/Absorb.png");
			choices[3].Texture = Engine.Graphics.CreateTexture2D("Texture/Adventure/Quit.png");

			choices.ForEach(o =>
			{
				AddChild(o, ChildMode.Position);
				Layer.AddObject(o);
				o.AddComponent(
					new EasingComponent(
						() => o.Position.Y,
						v => o.Position = new Vector2DF(o.Position.X, v),
						0,
						umw.EasingStart.StartRapidly3,
						umw.EasingEnd.EndSlowly2,
						20,
						() => enabled = true),
					"FirstSlideIn");
			});

			selectSound = Engine.Sound.CreateSoundSource("Sound/kachi38.wav", true);
			decideSound = Engine.Sound.CreateSoundSource("Sound/metal11.wav", true);

			choice = new Choice<Keys>(4, true, k => Engine.Keyboard.GetKeyState(k) == KeyState.Hold);
			choice.Set(Keys.Right, ChoiceControll.Next);
			choice.Set(Keys.Left, ChoiceControll.Previous);
			choice.Set(Keys.Z, ChoiceControll.Decide);
			choice.Hold = new ChoiceHoldOption(30, 10);
			choice.OnMove += Choice_OnMove;
			choice.OnDecide += Choice_OnDecide;

			choices[0].RemoveComponent("FirstSlideIn");
			MakeActive(choices[0]);
			previousSelectedIndex = 0;
		}

		private void Choice_OnDecide(int index)
		{
			Engine.Sound.Play(decideSound);

			callback((BattleCommand)index);

			Vanish();
			choices.ForEach(x => x.Vanish());
		}

		private void Choice_OnMove(int index)
		{
			Engine.Sound.Play(selectSound);
			var prev = choices[previousSelectedIndex];

			prev.RemoveComponent(SlideInKey);

			prev.Color = InactiveColor;

			prev.AddComponent(
				new EasingComponent(
					() => prev.Position.Y,
					v => prev.Position = new Vector2DF(prev.Position.X, v),
					0,
					umw.EasingStart.StartRapidly3,
					umw.EasingEnd.EndSlowly1,
					10),
				SlideOutKey);


			MakeActive(choices[index]);

			previousSelectedIndex = index;
		}

		private static void MakeActive(TextureObject2D obj)
		{
			obj.RemoveComponent(SlideOutKey);

			obj.Color = ActiveColor;

			obj.AddComponent(
				new EasingComponent(
					() => obj.Position.Y,
					v => obj.Position = new Vector2DF(obj.Position.X, v),
					SlideInGoal,
					umw.EasingStart.StartRapidly3,
					umw.EasingEnd.EndSlowly1,
					10),
				SlideInKey);
		}

		protected override void OnUpdate()
		{
			if(enabled)
			{
				choice.Update();
			}
		}
	}
}
