using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ace;
using Rpg1.Model;
using Rpg1.Ace.Common;
using Rpg1.Model.Title;
using Rpg1.Model.Messages;
using Rpg1.Model.Adventure;

namespace Rpg1.Ace.Title
{
	abstract class TitleSceneLayer : Layer2D, IChannelable, IDisposable
	{
		public abstract void AddMessageHandlerTo(SteppingChannel<IMessage> channel);

		public abstract void Dispose();
	}

	class TitleScene : Scene, IChannelable
	{
		Layer2D background;
		TitleSceneLayer layer;
		int bgmHandler;

		public TitleScene()
		{
			background = new TitleBackgroundLayer();
			AddLayer(background);

			layer = new TitleLayer();
			AddLayer(layer);
		}

		public TitleScene(ScoreContext context)
		{
			background = new TitleBackgroundLayer();
			background.AddPostEffect(new PostEffectGaussianBlur { Intensity = 2 });
			AddLayer(background);

			layer = new ScoreLayer(context);
			AddLayer(layer);
		}

		protected override void OnUpdateForTheFirstTime()
		{
			var bgm = Engine.Sound.CreateSoundSource("Music/Title.ogg", false);
			bgm.IsLoopingMode = true;
			bgmHandler = Engine.Sound.Play(bgm);
			Engine.Sound.SetVolume(bgmHandler, 0.4f);
		}

		public void AddMessageHandlerTo(SteppingChannel<IMessage> channel)
		{
			channel.AddMessageHandler<ChangeFlowMessage<Model.Adventure.AdventureContext>>(ChangeToAdventure);
			channel.AddMessageHandler<ChangeContextMessage<ScoreContext>>(ChangeToScore);
			channel.AddMessageHandler<ChangeContextMessage<AchievementContext>>(ChangeToAchievement);
			channel.AddMessageHandler<ChangeFlowMessage<object>>(ChangeToTitle);
			channel.AddMessageHandler<ChangeToLoadSceneMessage>(ChangeToLoad);

			if(layer != null)
			{
				layer.AddMessageHandlerTo(channel);
			}
		}

		private void ChangeToLoad(ChangeToLoadSceneMessage msg, Action callback)
		{
			var scene = new LoadScene();
			scene.Callback = Program.ChangeChannel(scene);
			Engine.ChangeSceneWithTransition(scene, new TransitionFade(1, 1));
			Engine.Sound.FadeOut(bgmHandler, 1);
		}

		private void ChangeToAchievement(ChangeContextMessage<AchievementContext> msg, Action callback)
		{
			layer.Dispose();
			layer = new AchievementLayer(msg.Context);
			AddLayer(layer);

			background.AddPostEffect(new PostEffectGaussianBlur { Intensity = 2 });

			callback = Program.ChangeChannel(this);
			callback();
		}

		private void ChangeToTitle(ChangeFlowMessage<object> msg, Action callback)
		{
			layer.Dispose();
			layer = new TitleLayer();
			AddLayer(layer);

			background.ClearPostEffects();

			Program.ChangeFlow(msg.Flow, this);
		}

		private void ChangeToScore(ChangeContextMessage<ScoreContext> msg, Action callback)
		{
			layer.Dispose();
			layer = new ScoreLayer(msg.Context);
			AddLayer(layer);

			background.AddPostEffect(new PostEffectGaussianBlur { Intensity = 2 });

			callback = Program.ChangeChannel(this);
			callback();
		}

		private void ChangeToAdventure(ChangeFlowMessage<AdventureContext> msg, Action callback)
		{
			var scene = new Adventure.Basic.AdventureScene(msg.Context);
			Program.ChangeFlow(msg.Flow, scene);
			Engine.ChangeSceneWithTransition(scene, new TransitionFade(1, 1));
			Engine.Sound.FadeOut(bgmHandler, 1);
			callback();
		}
	}
}
