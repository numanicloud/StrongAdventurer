using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ace;

namespace Rpg1.Ace.Adventure
{
	class Counter<T> : TextureObject2D where T : INotifyPropertyChanged
	{
		private static readonly Vector2DF IndicatorPosition = new Vector2DF(88, 20);

		private T obj;
		private Func<T, int> getter;
		private string propertyName;
		private TextObject2D indicator;

		public Counter(T obj, Expression<Func<T, int>> getter, Texture2D texture)
		{
			this.obj = obj;
			this.getter = getter.Compile();
			propertyName = (getter.Body as MemberExpression).Member.Name;
			Texture = texture;
		}

		protected override void OnStart()
		{
			indicator = new TextObject2D()
			{
				Position = IndicatorPosition,
				Font = Engine.Graphics.CreateFont("Font/ArialBold.aff"),
				DrawingPriority = DrawingPriority + 1,
			};
			UpdateIndicator();
			AddChild(indicator, ChildMode.Position);
			Layer.AddObject(indicator);

			obj.PropertyChanged += Obj_PropertyChanged;
		}

		private void Obj_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if(e.PropertyName == propertyName)
			{
				UpdateIndicator();
			}
		}

		private void UpdateIndicator()
		{
			indicator.Text = getter(obj).ToString();
			var size = indicator.Font.CalcTextureSize(indicator.Text, indicator.WritingDirection);
			indicator.CenterPosition = new Vector2DF(size.X, size.Y / 2);
		}
	}
}
