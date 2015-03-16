using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ace;

namespace umw
{
	public enum EasingStart : int
	{
		StartSlowly4 = -40,
		StartSlowly3 = -30,
		StartSlowly2 = -20,
		StartSlowly1 = -10,
		Start = 0,
		StartRapidly1 = 10,
		StartRapidly2 = 20,
		StartRapidly3 = 30,
		StartRapidly4 = 40,
	}

	public enum EasingEnd : int
	{
		EndSlowly4 = -40,
		EndSlowly3 = -30,
		EndSlowly2 = -20,
		EndSlowly1 = -10,
		End = 0,
		EndRapidly1 = 10,
		EndRapidly2 = 20,
		EndRapidly3 = 30,
		EndRapidly4 = 40,
	}

	public class Easing
	{
		/// <summary>
		/// Easingの三次方程式の項3つを求める。
		/// </summary>
		/// <param name="a1">始点の角度(度)</param>
		/// <param name="a2">終点の角度(度)</param>
		/// <returns>項</returns>
		public static float[] GetEasingFunction(float a1, float a2)
		{
			float g1 = (float)Math.Tan(((float)a1 + 45.0) / 180.0 * Math.PI);
			float g2 = (float)Math.Tan(((float)a2 + 45.0) / 180.0 * Math.PI);

			float c = g1;
			float a = g2 - g1 - (1.0f - c) * 2.0f;
			float b = (g2 - g1 - (a * 3.0f)) / 2.0f;

			return new float[3] { a, b, c };
		}

		/// <summary>
		/// Easingの三次方程式の項3つを求める。
		/// </summary>
		/// <param name="a1">始点の角度</param>
		/// <param name="a2">終点の角度</param>
		/// <returns>項</returns>
		public static float[] GetEasingFunction(EasingStart s, EasingEnd e)
		{
			return GetEasingFunction((float)s, (float)e);
		}

		/// <summary>
		/// 前フレームの値と現在のフレーム、最終フレームから現在のフレームの値を取得する。
		/// </summary>
		/// <param name="value">前フレームの値</param>
		/// <param name="lastValue">最終フレームの値</param>
		/// <param name="count">現在のフレーム</param>
		/// <param name="max">最終フレーム</param>
		/// <param name="easing">Easingの項</param>
		/// <returns></returns>
		public static float GetNextValue(float value, float lastValue, int count, int max, float[] easing)
		{
			if(count == max ) return lastValue;

			var t = (float)count / (float)max;
			var bt = (float)(count - 1) / (float)max;
			var t2 = t * t;
			var bt2 = bt * bt;
			var t3 = t2 * t;
			var bt3 = bt2 * bt;

			var v = easing[0] * t3 + easing[1] * t2 + easing[2] * t;
			var bv = easing[0] * bt3 + easing[1] * bt2 + easing[2] * bt;

			return (lastValue - value) / (1.0f - bv) * (v - bv) + value; 
		}

		/// <summary>
		/// 前フレームの値と現在のフレーム、最終フレームから現在のフレームの値を取得する。
		/// </summary>
		/// <param name="value">前フレームの値</param>
		/// <param name="lastValue">最終フレームの値</param>
		/// <param name="count">現在のフレーム</param>
		/// <param name="max">最終フレーム</param>
		/// <param name="easing">Easingの項</param>
		/// <returns></returns>
		public static Vector2DF GetNextValue(Vector2DF value, Vector2DF lastValue, int count, int max, float[] easing)
		{
			if (count == max) return lastValue;

			var t = (float)count / (float)max;
			var bt = (float)(count - 1) / (float)max;
			var t2 = t * t;
			var bt2 = bt * bt;
			var t3 = t2 * t;
			var bt3 = bt2 * bt;

			var v = easing[0] * t3 + easing[1] * t2 + easing[2] * t;
			var bv = easing[0] * bt3 + easing[1] * bt2 + easing[2] * bt;

			Vector2DF r;
			r.X = (lastValue.X - value.X) / (1.0f - bv) * (v - bv) + value.X;
			r.Y = (lastValue.Y - value.Y) / (1.0f - bv) * (v - bv) + value.Y;
			return r;
		}
	}
}