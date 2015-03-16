using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ace;

namespace Rpg1.Ace.Common
{
	class MessageWindow : TextureObject2D
	{
		abstract class Status
		{
			protected MessageWindow Owner { get; private set; }
			public Status(MessageWindow owner)
			{
				Owner = owner;
			}
			public abstract void OnUpdate();
		}

		class NeutralStatus : Status
		{
			public NeutralStatus(MessageWindow owner)
				: base(owner)
			{

			}
			public override void OnUpdate()
			{
			}
		}

		class WritingStatus : Status
		{
			string message { get; set; }
			Action callback { get; set; }
			int charCount { get; set; }
			int count { get; set; }

			public WritingStatus(MessageWindow owner, string message, Action callback)
				: base(owner)
			{
				this.message = message;
				this.callback = callback;
				charCount = 0;
				count = 0;
			}

			public override void OnUpdate()
			{
				count++;
				if(count == Owner.WaitOnChar)
				{
					charCount++;
					count = 0;
				}

				Owner.messageObject.Text = string.Concat(message.Take(charCount));

				if(charCount == message.Length)
				{
					Owner.status = new WaitingStatus(Owner, callback);
				}
			}
		}

		class WaitingStatus : Status
		{
			Action callback { get;set; }

			public WaitingStatus(MessageWindow owner, Action callback)
				: base(owner)
			{
				this.callback = callback;
			}

			public override void OnUpdate()
			{
				if(Engine.Keyboard.GetKeyState(Program.DecideKey) == KeyState.Push)
				{
					var s = Engine.Sound.Play(Owner.readSound);
					Engine.Sound.SetVolume(s, 0.5f);

					Owner.messageObject.Text = "";
					callback();
					callback = null;
					Owner.status = new NeutralStatus(Owner);
				}
			}
		}


		TextObject2D messageObject { get; set; }
		SoundSource readSound { get;set; }
		Status status { get;set; }

		public int WaitOnChar { get; set; }

		public Vector2DF MessagePosition
		{
			get { return messageObject.Position; }
			set { messageObject.Position = value; }
		}

		public Font MessageFont
		{
			get { return messageObject.Font; }
			set { messageObject.Font = value; }
		}


		public MessageWindow()
		{
			messageObject = new TextObject2D()
			{
				Text = "",
				DrawingPriority = 2,
			};
			status = new NeutralStatus(this);
			WaitOnChar = 1;
		}

		protected override void OnStart()
		{
			AddChild(messageObject, ChildMode.Position);
			Layer.AddObject(messageObject);

			readSound = Engine.Sound.CreateSoundSource("Sound/on02.wav", true);
		}

		protected override void OnUpdate()
		{
			status.OnUpdate();
		}

		public void ShowMessage(string message, Action callback)
		{
			status = new WritingStatus(this, message, callback);
		}

		public void ShowMessage(string message)
		{
			messageObject.Text = message;
		}
	}
}
