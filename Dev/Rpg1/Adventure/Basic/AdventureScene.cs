using System;
using System.IO;
using ace;
using Rpg1.Model;
using Rpg1.Model.Adventure;
using Rpg1.Model.Adventure.Messages;
using Rpg1.Model.Battle;
using Rpg1.Model.Battle.Messages;
using Rpg1.Model.Messages;
using Rpg1.Model.Title;

namespace Rpg1.Ace.Adventure.Basic
{
	class AdventureScene : Scene, IChannelable
	{
		abstract class State : IDisposable
		{
			protected AdventureScene Owner { get; private set; }

			public State(AdventureScene owner)
			{
				Owner = owner;
			}

			public abstract void Initialize();

			public abstract void AddMessageHandlerTo(SteppingChannel<IMessage> channel);

			public abstract void Dispose();
		}

		class AdventureState : State
		{
			AdventureLayer layer { get; set; }

			public AdventureState(AdventureScene owner, AdventureContext context)
				: base(owner)
			{
				layer = new AdventureLayer(context);
			}

			public override void Initialize()
			{
				Owner.AddLayer(layer);
			}

			public override void AddMessageHandlerTo(SteppingChannel<IMessage> channel)
			{
				channel.AddMessageHandler<SetCharactor>(SetCharactor);
				channel.AddMessageHandler<HealMessage>(ShowHeal);
			}

			public override void Dispose()
			{
				Owner.RemoveLayer(layer);
			}


			private void ShowHeal(HealMessage msg, Action callback)
			{
				layer.ShowHealing(msg.Amount);
				Owner.noticeLayer.ShowMessage(msg.Battler.Ability.Name + "のHPが" + msg.Amount + "回復した！", callback);
			}

			private void SetCharactor(SetCharactor msg, Action callback)
			{
				Texture2D tex = null;
				if(msg.ImagePath != null)
				{
					var path = Path.Combine("Texture/Charactor/", msg.ImagePath);
					tex = Engine.Graphics.CreateTexture2D(path);
				}
				layer.SetCharactor(tex, callback);
			}
		}

		class BattleState : State
		{
			BattleLayer layer { get; set; }

			public BattleState(AdventureScene owner, BattleContext context)
				: base(owner)
			{
				layer = new BattleLayer(context);
			}

			public override void AddMessageHandlerTo(SteppingChannel<IMessage> channel)
			{
				channel.AddMessageHandler<ChooseSkill, BattleCommand>(StartChooseSkill);
				channel.AddMessageHandler<PlayEffect>(StartPlayingEffect);
				channel.AddMessageHandler<DeathOfBattler>(ShowDeath);
				channel.AddMessageHandler<Damage>(ShowDamage);
				channel.AddMessageHandler<HealMessage>(ShowHealing);
				channel.AddMessageHandler<LoseMessage>(OnLose);
			}

			public override void Dispose()
			{
				Owner.RemoveLayer(layer);
			}

			public override void Initialize()
			{
				Owner.AddLayer(layer);
			}

			#region Handlers

			private void StartChooseSkill(ChooseSkill msg, Action<BattleCommand> callback)
			{
				layer.StartChooseSkill(callback);
				Owner.noticeLayer.ShowMessage("どうする？");
			}

			private void StartPlayingEffect(PlayEffect msg, Action callback)
			{
				var effectPath = Path.Combine("Effect/", msg.EffectPath);
				var effect = Engine.Graphics.CreateEffect(effectPath);

				var soundPath = Path.Combine("Sound/", msg.SoundPath);
				var sound = Engine.Sound.CreateSoundSource(soundPath, true);

				layer.StartPlayingEffect(msg.BattlerIndex, effect, sound, msg.Wait, callback);
			}

			private void ShowDeath(DeathOfBattler msg, Action callback)
			{
				layer.ShowDeath(msg.Index);
				callback();
			}

			private void ShowDamage(Damage msg, Action callback)
			{
				layer.ShowDamage(msg.Battler.Index, msg.Amount);
				var m = string.Format("{0}に{1}のダメージ！", msg.Battler.Ability.Name, msg.Amount);
				Owner.noticeLayer.ShowMessage(m, callback);
			}

			private void ShowHealing(HealMessage msg, Action callback)
			{
				layer.ShowHealing(msg.Battler.Index, msg.Amount);
				var m = string.Format("{0}のHPが{1}回復した！", msg.Battler.Ability.Name, msg.Amount);
				Owner.noticeLayer.ShowMessage(m, callback);
			}

			private void OnLose(LoseMessage msg, Action callback)
			{
				var bgm = Engine.Sound.CreateSoundSource("Music/Summer_Sky.ogg", false);

				Engine.Sound.Stop(Owner.playingBgmHandler);
				Owner.playingBgmHandler = Engine.Sound.Play(bgm);
				Engine.Sound.SetVolume(Owner.playingBgmHandler, 0.5f);

				Owner.noticeLayer.ShowMessage("戦いに 負けた…", callback);
			}
			#endregion
		}

		BackGroundLayer bgLayer { get; set; }
		NoticeLayer noticeLayer { get; set; }
		State state { get; set; }
		int playingBgmHandler { get; set; }

		public AdventureScene(AdventureContext context)
		{
			bgLayer = new BackGroundLayer(context);
			AddLayer(bgLayer);

			noticeLayer = new NoticeLayer(context);
			AddLayer(noticeLayer);

			state = new AdventureState(this, context);
			state.Initialize();
		}

		public void AddMessageHandlerTo(SteppingChannel<IMessage> channel)
		{
			channel.AddMessageHandler<ChangeContextMessage<AdventureContext>>(SetAdventureState);
			channel.AddMessageHandler<ChangeContextMessage<BattleContext>>(SetBattleState);
			channel.AddMessageHandler<ChangeFlowMessage<object>>(GoToTitle);
			channel.AddMessageHandler<ChangeFlowMessage<ScoreContext>>(GoToScore);
			channel.AddMessageHandler<GetAchievementMessage>(ShowAchievement);
			channel.AddMessageHandler<StringMessage>(ShowMessage);
			channel.AddMessageHandler<ChangeBackgroundMessage>(ChangeBackground);
			channel.AddMessageHandler<PlayBgmMessage>(PlayBgm);

			state.AddMessageHandlerTo(channel);
		}

		private void GoToScore(ChangeFlowMessage<ScoreContext> msg, Action callback)
		{
			var title = new Title.TitleScene(msg.Context);
			Program.ChangeFlow(msg.Flow, title);
			Engine.ChangeSceneWithTransition(title, new TransitionFade(1, 1));
			Engine.Sound.FadeOut(playingBgmHandler, 1);
		}

		private void PlayBgm(PlayBgmMessage msg, Action callback)
		{
			if(playingBgmHandler != 0)
			{
				Engine.Sound.FadeOut(playingBgmHandler, 1);
			}

			var path = Path.Combine("Music/", msg.BgmPath);
			var bgm = Engine.Sound.CreateSoundSource(path, false);
			bgm.IsLoopingMode = true;
			playingBgmHandler = Engine.Sound.Play(bgm);

			Engine.Sound.SetVolume(playingBgmHandler, 0.8f);

			callback();
		}

		private void ShowMessage(StringMessage msg, Action callback)
		{
			noticeLayer.ShowMessage(msg.Message, callback);
		}

		private void ChangeBackground(ChangeBackgroundMessage msg, Action callback)
		{
			var path = Path.Combine("Texture/Adventure/", msg.ImagePath);
			bgLayer.ChangeBackGround(
				Engine.Graphics.CreateTexture2D(path),
				callback);
		}

		private void ShowAchievement(GetAchievementMessage msg, Action callback)
		{
			noticeLayer.ShowAchivement(msg.Achievement);
			Console.WriteLine("実績解除：{0}", msg.Achievement.Title);
			callback();
		}

		private void GoToTitle(ChangeFlowMessage<object> msg, Action callback)
		{
			var title = new Title.TitleScene();
			Program.ChangeFlow(msg.Flow, title);
			Engine.ChangeSceneWithTransition(title, new TransitionFade(1, 1));
			Engine.Sound.FadeOut(playingBgmHandler, 1);
		}

		private void SetAdventureState(ChangeContextMessage<AdventureContext> msg, Action callback)
		{
			state.Dispose();

			state = new AdventureState(this, msg.Context);
			state.Initialize();
			callback = Program.ChangeChannel(this);

			callback();
		}

		private void SetBattleState(ChangeContextMessage<BattleContext> msg, Action callback)
		{
			state.Dispose();

			state = new BattleState(this, msg.Context);
			state.Initialize();
			callback = Program.ChangeChannel(this);

			callback();
		}
	}
}
