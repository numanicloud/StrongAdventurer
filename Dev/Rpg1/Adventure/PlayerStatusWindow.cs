using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ace;
using Rpg1.Model.Battle;

namespace Rpg1.Ace.Adventure
{
	class PlayerStatusWindow : BattlerAppearance
	{
		private static readonly Vector2DF HpBarPosition = new Vector2DF(30, 36);
		private static readonly Vector2DF NamePosition = new Vector2DF(8, 0);

		Battler battler;
		TextObject2D name, hpIndicator;
		HpBar hpBar;

		public override Vector2DF EffectPosition
		{
			get { return GetGlobalPosition() + new Vector2DF(Texture.Size.X, Texture.Size.Y) / 2; }
		}

		public PlayerStatusWindow(Battler battler)
		{
			this.battler = battler;
		}

		protected override void OnStart()
		{
			base.OnStart();

			Texture = Engine.Graphics.CreateTexture2D("Texture/Adventure/StatusWindow.png");

			hpBar = new HpBar(battler)
			{
				Position = HpBarPosition,
			};
			AddChild(hpBar, ChildMode.Position);
			Layer.AddObject(hpBar);

			var font = Engine.Graphics.CreateFont(Def.BasicFont);

			name = new TextObject2D()
			{
				Position = NamePosition,
				Font = font,
				Text = battler.Ability.Name,
			};
			AddChild(name, ChildMode.Position);
			Layer.AddObject(name);

			hpIndicator = new TextObject2D()
			{
				Position = new Vector2DF(170, 12),
				Font = font,
				Text = battler.Hp.ToString(),
				Scale = new Vector2DF(0.75f, 0.75f),
			};
			AddChild(hpIndicator, ChildMode.Position);
			Layer.AddObject(hpIndicator);

			battler.PropertyChanged += Battler_PropertyChanged;
			UpDateHp();
		}

		private void Battler_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if(e.PropertyName == "Hp")
			{
				UpDateHp();
			}
		}

		private void UpDateHp()
		{
			hpIndicator.Text = battler.Hp.ToString();
			var size = hpIndicator.Font.CalcTextureSize(hpIndicator.Text, WritingDirection.Horizontal);
			hpIndicator.CenterPosition = new Vector2DF(size.X, 0);
		}

		public override void ShowDeath()
		{
			Texture = Engine.Graphics.CreateTexture2D("Texture/Adventure/StatusWindow2.png");
		}
	}
}
