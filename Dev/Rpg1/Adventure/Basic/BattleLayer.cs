using System;
using System.Collections.Generic;
using ace;
using Rpg1.Ace.Common;
using Rpg1.Model.Battle;
using umw;

namespace Rpg1.Ace.Adventure.Basic
{
	class BattleLayer : Layer2D
	{
		public static readonly Vector2DF EnemyPosition = new Vector2DF(320, 270);
		private static readonly Vector2DF MessageWindowPosition = new Vector2DF(0, 300);
		private static readonly Vector2DF MessagePosition = new Vector2DF(10, 15);

		BattleContext context { get; set; }
		IDictionary<int, BattlerAppearance> battlers { get; set; }

		public BattleLayer(BattleContext context)
		{
			this.context = context;

			var playerStatus = new PlayerStatusWindow(context.Player)
			{
				Position = AdventureLayer.PlayerStatusWindowPosition,
			};
			AddObject(playerStatus);

			var enemyObject = new EnemyObject(context.Enemy)
			{
				Position = EnemyPosition,
			};
			AddObject(enemyObject);

			var turnCounter = new Counter<BattleContext>(
				context,
				x => x.Turn,
				Engine.Graphics.CreateTexture2D("Texture/Adventure/TurnCounter.png"));
			turnCounter.Position = new Vector2DF(-96 - 100, 50);
			AddObject(turnCounter);

			turnCounter.AddComponent(
				EasingComponent.CreateXEasing(turnCounter, 0, EasingStart.StartRapidly2, EasingEnd.EndSlowly2, 30),
				"SlideIn");

			battlers = new Dictionary<int, BattlerAppearance>();
			battlers[context.Player.Index] = playerStatus;
			battlers[context.Enemy.Index] = enemyObject;
		}

		public void ShowHealing(int battlerIndex, int amount)
		{
			battlers[battlerIndex].ShowHealing(amount);
		}

		public void ShowDamage(int battlerIndex, int amount)
		{
			battlers[battlerIndex].ShowDamage(amount);
		}

		public void ShowDeath(int battlerIndex)
		{
			battlers[battlerIndex].ShowDeath();
		}

		public void StartPlayingEffect(int battlerIndex, Effect effect, SoundSource sound, float wait, Action callback)
		{
			battlers[battlerIndex].PlayEffect(effect, sound, wait, callback);
		}

		public void StartChooseSkill(Action<BattleCommand> callback)
		{
			AddObject(new CommandMenu(callback));
		}
	}
}
