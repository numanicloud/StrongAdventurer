using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ace;
using Rpg1.Ace.Adventure;
using Rpg1.Ace.Common;
using Rpg1.Model.Battle;

namespace Rpg1.Ace.Adventure
{
	class EnemyObject : BattlerAppearance
	{
		EnemyBattler battler;
		HpBar hpBar;

		public EnemyObject(EnemyBattler battler)
		{
			this.battler = battler;
		}

		public override Vector2DF EffectPosition
		{
			get { return GetGlobalPosition() - new Vector2DF(0, Texture.Size.Y / 2); }
		}

		public override void ShowDeath()
		{
			AddComponent(new EasingComponent(
				() => Color.A,
				v =>
				{
					Color = new Color(255, 255, 255, (byte)v);
					hpBar.Color = new Color(255, 255, 255, (byte)v);
				},
				0,
				umw.EasingStart.StartSlowly2,
				umw.EasingEnd.EndRapidly2,
				30), "Fadeout");
		}

		protected override void OnStart()
		{
			base.OnStart();

			var enemyImagePath = Path.Combine("Texture/Charactor/", battler.EnemiesAbility.ImagePath);
			Texture = Engine.Graphics.CreateTexture2D(enemyImagePath);
			this.SetCenterPosition(Common.CenterPosition.BottomCenter);

			hpBar = new HpBar(battler)
			{
				Position = new Vector2DF(-70, 5),
			};
			AddChild(hpBar, ChildMode.Position);
			Layer.AddObject(hpBar);
		}
	}
}
