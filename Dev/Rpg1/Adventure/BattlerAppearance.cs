using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ace;
using Rpg1.Ace.Common;

namespace Rpg1.Ace.Adventure
{
	abstract class BattlerAppearance : TextureObject2D
	{
		private Font damageFont;
		private SoundSource damageSound;
		private SoundSource healSound;

		protected override void OnStart()
		{
			damageFont = Engine.Graphics.CreateFont("Font/ArialBold.aff");
			damageSound = Engine.Sound.CreateSoundSource("Sound/bosu33.wav", true);
			healSound = Engine.Sound.CreateSoundSource("Sound/power02.wav", true);
		}

		public abstract Vector2DF EffectPosition { get; }
		public abstract void ShowDeath();

		public void ShowDamage(int amount)
		{
			var obj = new DamageIndicator(damageFont, amount, EffectPosition);
			Layer.AddObject(obj);

			Engine.Sound.Play(damageSound);
		}

		public void PlayEffect(Effect effect, SoundSource sound, float wait, Action callback)
		{
			var e = new EffectObject2D()
			{
				Position = EffectPosition,
				Effect = effect,
				Scale = new Vector2DF(6, 6)
			};

			Layer.AddObject(e);
			e.Play();

			var t = DateTime.Now.TimeOfDay;
			AddComponent(new TimerComponent(wait, callback), "EffectTimer" + t.ToString());

			Engine.Sound.Play(sound);
		}

		public void ShowHealing(int amount)
		{
			var obj = new DamageIndicator(damageFont, amount, EffectPosition);
			obj.Color = new Color(80, 255, 50, 255);
			Layer.AddObject(obj);

			Engine.Sound.Play(healSound);
		}
	}
}
