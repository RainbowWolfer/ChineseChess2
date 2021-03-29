using ChineseChess2.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess2.Class {
	public static class Translator {
		public const char bing = 'b';
		public const char ju = 'j';
		public const char pao = 'p';
		public const char ma = 'm';
		public const char xiang = 'x';
		public const char shi = 's';
		public const char jiang = 'k';

		public static int HEIGHT => ChessPage.HEIGHT;
		public static int WIDTH => ChessPage.WIDTH;

		/// <summary>
		/// upper is red, the other is black
		/// </summary>
		public static string Translate(Node[,] ns) {
			string result = "";
			for(int i = 0; i < ns.GetLength(1); i++) {
				for(int j = 0; j < ns.GetLength(0); j++) {
					Node n = ns[j, i];
					if(n.type == null) {
						ChangeNumber(ref result);
					} else {
						result += GetChar(n.type.Value, n.side);
					}

				}
				result += '/';
			}
			return result;
		}
		public static Dictionary<Vector2, Node> LoadNodes(string str) {
			str = str.Replace(" ", "");
			var ns = new Dictionary<Vector2, Node>();

			int x = 0;
			int y = 0;

			foreach(char c in str) {
				if(char.IsNumber(c)) {
					x += int.Parse(c.ToString());
					continue;
				} else if(c == '/') {
					x = 0;
					y++;
				} else {
					Vector2 v = new Vector2(x, y);
					ns.Add(v, NewNode(c, v));
					x++;
				}
			}
			for(int i = 0; i < WIDTH; i++) {
				for(int j = 0; j < HEIGHT; j++) {
					Node n;
					if(ns.ContainsKey(new Vector2(i, j))) {
						n = ns[new Vector2(i, j)];
					} else {
						n = new Node(new Vector2(i, j));
						ns.Add(new Vector2(i, j), n);
					}
					if(n == null) {
						n = new Node(new Vector2(i, j));
					}
					//n.x = i;
					//n.y = j;

					if((i == (WIDTH - 1) / 2 - 1 || i == (WIDTH - 1) / 2 || i == (WIDTH - 1) / 2 + 1) && (j == 0 || j == 1 || j == 2 || j == HEIGHT - 3 || j == HEIGHT - 2 || j == HEIGHT - 1)) {
						n.castle = true;
					}
				}
			}

			return ns;
		}
		private static Node NewNode(char c, Vector2 v) {
			switch(char.ToLower(c)) {
				case bing:
					return new Node(PieceType.BING, char.IsUpper(c) ? Side.Red : Side.Black, v);
				case ma:
					return new Node(PieceType.MA, char.IsUpper(c) ? Side.Red : Side.Black, v);
				case pao:
					return new Node(PieceType.PAO, char.IsUpper(c) ? Side.Red : Side.Black, v);
				case xiang:
					return new Node(PieceType.XIANG, char.IsUpper(c) ? Side.Red : Side.Black, v);
				case shi:
					return new Node(PieceType.SHI, char.IsUpper(c) ? Side.Red : Side.Black, v);
				case jiang:
					return new Node(PieceType.SHUAI, char.IsUpper(c) ? Side.Red : Side.Black, v);
				case ju:
					return new Node(PieceType.JU, char.IsUpper(c) ? Side.Red : Side.Black, v);
				default:
					throw new Exception();
			}
		}
		private static void ChangeNumber(ref string str, int add = 1) {
			char last = str[str.Length - 1];
			if(char.IsNumber(last)) {
				str = str.Remove(str.Length - 1, 1);
				str += (int.Parse(last.ToString()) + add).ToString();
			} else {
				str += add.ToString();
			}
		}
		private static char GetChar(PieceType p, Side s) {
			if(s == Side.Empty) {
				throw new Exception();
			}
			char c;
			switch(p) {
				case PieceType.BING:
					c = bing;
					break;
				case PieceType.PAO:
					c = pao;
					break;
				case PieceType.JU:
					c = ju;
					break;
				case PieceType.MA:
					c = ma;
					break;
				case PieceType.XIANG:
					c = xiang;
					break;
				case PieceType.SHI:
					c = shi;
					break;
				case PieceType.SHUAI:
					c = jiang;
					break;
				default:
					throw new Exception();
			}
			return s == Side.Red ? char.ToUpper(c) : c;
		}
	}
}
