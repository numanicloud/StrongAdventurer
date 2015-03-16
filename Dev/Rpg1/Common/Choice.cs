using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rpg1.Ace.Common
{
	public enum ChoiceControll
	{
		Next, Previous, Decide, Cancel
	}

	public class ChoiceHoldOption
	{
		public int HoldWaitTime { get; set; }
		public int HoldSpanTime { get; set; }

		public ChoiceHoldOption(int wait, int span)
		{
			HoldWaitTime = wait;
			HoldSpanTime = span;
		}
	}

	/// <summary>
	/// キー入力によって選択肢の処理をするクラス。
	/// </summary>
	/// <typeparam name="TKeyCode">キーの識別子となる型。</typeparam>
	public class Choice<TKeyCode>
	{
		#region フィールド
		int size_;
		bool enabled;

		public int Size
		{
			get { return size_; }
			set
			{
				size_ = value;
				if(SelectedIndex > size_)
					SelectedIndex = size_;
			}
		}
		public int SelectedIndex { get; set; }
		public bool Loop { get; set; }

		public ChoiceHoldOption Hold { get; set; }
		public event Action<int> OnMove;
		public event Action<int> OnDecide;
		public event Action<int> OnCancel;

		Dictionary<TKeyCode, ChoiceControll> controlls { get; set; }
		Dictionary<ChoiceControll, int> count { get; set; }
		Func<TKeyCode, bool> keyIsHold { get; set; }
		#endregion

		#region メソッド
		public Choice(int size, bool loop, Func<TKeyCode, bool> keyIsHold)
		{
			this.Size = size;
			this.Loop = loop;
			this.keyIsHold = keyIsHold;
			controlls = new Dictionary<TKeyCode, ChoiceControll>();
			count = new Dictionary<ChoiceControll, int>();
			count[ChoiceControll.Next] = 1;
			count[ChoiceControll.Previous] = 1;
			count[ChoiceControll.Decide] = 1;
			count[ChoiceControll.Cancel] = 1;
			enabled = false;
		}

		public Choice(int size, bool loop, Func<TKeyCode, bool> keyIsHold, int holdWaitTime, int holdSpanTime)
			: this(size, loop, keyIsHold)
		{
			Hold = new ChoiceHoldOption(holdWaitTime, holdSpanTime);
			enabled = false;
		}

		public void Set(TKeyCode keycode, ChoiceControll controll)
		{
			controlls[keycode] = controll;
		}

		public void Update()
		{
			foreach(TKeyCode item in controlls.Keys)
			{
				ChoiceControll controll = controlls[item];

				if(keyIsHold(item)) ++count[controll];
				else count[controll] = 0;

				if(enabled &&
					(count[controll] == 1 ||
					Hold != null && ((count[controll] - Hold.HoldWaitTime) % Hold.HoldSpanTime == 1)))
				{
					switch(controll)
					{
					case ChoiceControll.Next:
						MoveNext();
						break;
					case ChoiceControll.Previous:
						MovePrevious();
						break;
					case ChoiceControll.Decide:
						Decide();
						break;
					case ChoiceControll.Cancel:
						Cancel();
						break;
					}
				}
			}

			if(!enabled)
			{
				Enum.GetValues(typeof(ChoiceControll))
					.Cast<ChoiceControll>()
					.Where(x => !controlls.ContainsValue(x))
					.ForEach(x => count[x] = 0);

				if(count.All(x => x.Value == 0))
					enabled = true;
			}
		}

		private void MoveNext()
		{
			if(SelectedIndex < Size - 1) ++SelectedIndex;
			else if(Loop) SelectedIndex = 0;
			else return;
			if(OnMove != null)
				OnMove(SelectedIndex);
		}
		private void MovePrevious()
		{
			if(SelectedIndex > 0) --SelectedIndex;
			else if(Loop) SelectedIndex = Size - 1;
			else return;
			if(OnMove != null)
				OnMove(SelectedIndex);
		}
		private void Decide()
		{
			if(OnDecide != null)
				OnDecide(SelectedIndex);
		}
		private void Cancel()
		{
			if(OnCancel != null)
				OnCancel(SelectedIndex);
		}
		#endregion
	}
}
