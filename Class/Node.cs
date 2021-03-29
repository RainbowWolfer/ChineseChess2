using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess2.Class {
	public class Node {
		public Side side;
		public PieceType? type;
		public Vector2 pos;

		public bool castle;

		public Node(PieceType type, Side side, Vector2 pos) {
			this.pos = pos;
			this.type = type;
			this.side = side;
		}
		public Node(Vector2 pos) {
			this.pos = pos;
			side = Side.Empty;
			type = null;
		}

		public override string ToString() {
			return side.ToString() + " | " + (type == null ? "NULL" : type.ToString()) + " | " + pos;
		}
	}
	public enum Side {
		Empty = 0, Red = 1, Black = 2
	}
	public enum PieceType {
		BING = 0, PAO = 1, JU = 2, MA = 3, XIANG = 4, SHI = 5, SHUAI = 6
	}
}
