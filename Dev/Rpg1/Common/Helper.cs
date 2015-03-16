using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ace;

namespace Rpg1.Ace.Common
{
	static class Helper
	{
		public static Vector2DF ToFloat(this Vector2DI v)
		{
			return new Vector2DF(v.X, v.Y);
		}

		public static void SetCenterPosition(this TextureObject2D obj, CenterPosition centerPosition)
		{
			obj.CenterPosition = GetPosition(obj.Texture.Size, centerPosition);
		}

		public static void SetCenterPosition(this TextObject2D obj, CenterPosition centerPosition)
		{
			var size = obj.Font.CalcTextureSize(obj.Text, obj.WritingDirection);
			obj.CenterPosition = GetPosition(size, centerPosition);
		}

		private static Vector2DF GetPosition(Vector2DI size, CenterPosition centerPosition)
		{
			switch(centerPosition)
			{
			case CenterPosition.TopLeft: return new Vector2DF(0, 0);
			case CenterPosition.TopCenter: return new Vector2DF(size.X / 2, 0);
			case CenterPosition.TopRight: return new Vector2DF(size.X, 0);
			case CenterPosition.CenterLeft: return new Vector2DF(0, size.Y / 2);
			case CenterPosition.CenterCenter: return new Vector2DF(size.X / 2, size.Y / 2);
			case CenterPosition.CenterRight: return new Vector2DF(size.X, size.Y / 2);
			case CenterPosition.BottomLeft: return new Vector2DF(0, size.Y);
			case CenterPosition.BottomCenter: return new Vector2DF(size.X / 2, size.Y);
			case CenterPosition.BottomRight: return new Vector2DF(size.X, size.Y);
			default: throw new Exception();
			}
		}
	}
}
