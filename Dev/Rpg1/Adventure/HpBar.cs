using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ace;
using Rpg1.Ace.Common;
using Rpg1.Model.Battle;

namespace Rpg1.Ace.Adventure
{
	class HpBar : TextureObject2D
	{
		Battler battler;
		TextureObject2D hpBar, hpFrame;

		public HpBar(Battler battler)
		{
			this.battler = battler;
		}

		protected override void OnStart()
		{
			IsDrawn = false;

			hpBar = new TextureObject2D()
			{
				Position = new Vector2DF(3, 0),
				Texture = Engine.Graphics.CreateTexture2D("Texture/Adventure/HpBar.png"),
			};
			AddChild(hpBar, ChildMode.Position);
			Layer.AddObject(hpBar);

			hpFrame = new TextureObject2D()
			{
				Texture = Engine.Graphics.CreateTexture2D("Texture/Adventure/HpFrame.png"),
			};
			AddChild(hpFrame, ChildMode.Position);
			Layer.AddObject(hpFrame);

			battler.PropertyChanged += Battler_PropertyChanged;

			hpBar.Src = new RectF(0, 0, CalcBarLength(), hpBar.Texture.Size.Y);
		}

		private float CalcBarLength()
		{
			var ratio = (float)battler.Hp / battler.Ability.MaxHp;
			var barLength = hpBar.Texture.Size.X * ratio;
			return barLength;
		}

		private void Battler_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if(e.PropertyName == "Hp")
			{
				UpdateHpBar();
			}
		}

		private void UpdateHpBar()
		{
			float y = hpBar.Texture.Size.Y;

			AddComponent(new EasingComponent(
				() => hpBar.Src.Width,
				f => hpBar.Src = new RectF(0, 0, f, y),
				CalcBarLength(),
				umw.EasingStart.StartRapidly2,
				umw.EasingEnd.EndSlowly2,
				10), "Easing");
		}
	}
}
