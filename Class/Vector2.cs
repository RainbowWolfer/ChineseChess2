using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess2.Class {
	/// <summary> Only for Int </summary>
	public struct Vector2 {
		public int x;
		public int y;

		public static readonly Vector2 Zero = new Vector2(0, 0);

		public Vector2(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public override string ToString() {
			return "(" + x + " : " + y + ")";
		}
		public override bool Equals(object obj) {
			if(obj is Vector2 v) {
				return x == v.x && y == v.y;
			} else {
				return false;
			}
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

		public static bool operator ==(Vector2 left, Vector2 right) {
			return left.Equals(right);
		}

		public static bool operator !=(Vector2 left, Vector2 right) {
			return !(left == right);
		}
		public static Vector2 operator +(Vector2 a, Vector2 b) {
			return new Vector2(a.x + b.x, a.y + b.y);
		}
		public static Vector2 operator -(Vector2 a, Vector2 b) {
			return new Vector2(a.x - b.x, a.y - b.y);
		}
		public static Vector2 operator *(Vector2 a, int b) {
			return new Vector2(a.x * b, a.y * b);
		}
	}
}
