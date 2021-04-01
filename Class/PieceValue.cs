using ChineseChess2.Pages;
using System;

namespace ChineseChess2.Class {
	public static class PieceValue {
		private static int WIDTH => ChessPage.WIDTH;
		private static int HEIGHT => ChessPage.HEIGHT;
		private static bool IsSame(Side s) => !(ChessPage.isUpRed ^ s == Side.Red);
		//public static Side CurrentSide => ChessPage.CurrentSide;

		/// <summary> Red -> 1, Black -> -1 </summary>
		public static int Evaluate(Side s) {
			int redEval = CountMaterial(Side.Red);
			int blackEval = CountMaterial(Side.Black);
			int evaluation = redEval - blackEval;

			return evaluation * (s == Side.Red ? 1 : -1);
		}
		private static int CountMaterial(Side s) {
			int material = 0;
			foreach(Node item in ChessPage.nodes.Values) {
				if(item.side == s) {
					material += GetNodeValue(item);
				}
			}
			return material;
		}
		public static int GetNodeValue(Node node) {
			if(node.type == null) {
				throw new Exception();
			}
			bool same = IsSame(node.side);

			int _x = same ? node.pos.x : WIDTH - node.pos.x - 1;
			int _y = same ? node.pos.y : HEIGHT - node.pos.y - 1;//make sure keep _y 0top -> 9bot

			switch(node.type.Value) {
				case PieceType.BING:
					return Value_BING[_y, _x];
				case PieceType.PAO:
					return Value_PAO[_y, _x];
				case PieceType.JU:
					return Value_JU[_y, _x];
				case PieceType.MA:
					return Value_MA[_y, _x];
				case PieceType.XIANG:
					return Value_XIANG[_y, _x];
				case PieceType.SHI:
					return Value_SHI[_y, _x];
				case PieceType.SHUAI:
					return Value_SHUAI[_y, _x];
				default:
					throw new Exception();
			}
		}
		public static int CalculateAverage(int[,] source) {
			
			return 0;
		}
		private static readonly int[,] Value_BING = {
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0,},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0,},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0,},
			{ 7, 0, 7, 0, 15, 0, 7, 0, 7,},
			{ 7, 0, 13, 0, 16, 0, 13, 0, 7,},
			{ 14, 18, 20, 27, 29, 27, 20, 18, 14,},
			{ 19, 23, 27, 29, 30, 29, 27, 23, 19,},
			{ 19, 24, 32, 37, 37, 37, 32, 24, 19,},
			{ 19, 24, 34, 42, 44, 42, 34, 24, 19, },
			{ 9, 9, 9, 11, 13, 11, 9, 9, 9, },
		};
		private static readonly int[,] Value_PAO = {
			{ 96, 96, 97, 99, 99, 99, 97, 96, 96,       },
			{96, 97, 98, 98, 98, 98, 98, 97, 96,        },
			{97, 96, 100, 99, 101, 99, 100, 96, 97,  },
			{96, 96, 96, 96, 96, 96, 96, 96, 96,        },
			{95, 96, 99, 96, 100, 96, 99, 96, 95,      },
			{96, 96, 96, 96, 100, 96, 96, 96, 96,      },
			{96, 99, 99, 98, 100, 98, 99, 99, 96,      },
			{97, 97, 96, 91, 92, 91, 96, 97, 97,        },
			{98, 98, 96, 92, 89, 92, 96, 98, 98,        },
			{100, 100, 96, 91, 90, 91, 96, 100, 100,},
		};
		private static readonly int[,] Value_JU = {
			 {194, 206, 204, 212, 200, 212, 204, 206, 194},
			 {200, 208, 206, 212, 200, 212, 206, 208, 200,},
			 {198, 208, 204, 212, 212, 212, 204, 208, 198,},
			 {204, 209, 204, 212, 214, 212, 204, 209, 204,},
			 {208, 212, 212, 214, 215, 214, 212, 212, 208,},
			 {208, 211, 211, 214, 215, 214, 211, 211, 208,},
			 {206, 213, 213, 216, 216, 216, 213, 213, 206,},
			 {206, 208, 207, 214, 216, 214, 207, 208, 206,},
			 {206, 212, 209, 216, 233, 216, 209, 212, 206,},
			 {206, 208, 207, 213, 214, 213, 207, 208, 206,},
		};
		private static readonly int[,] Value_MA = {
			{88, 85, 90, 88, 90, 88, 90, 85, 88,                   },
			{85, 90, 92, 93, 78, 93, 92, 90, 85,                   },
			{93, 92, 94, 95, 92, 95, 94, 92, 93,                   },
			{92, 94, 98, 95, 98, 95, 98, 94, 92,                   },
			{90, 98, 101, 102, 103, 102, 101, 98, 90,         },
			{90, 100, 99, 103, 104, 103, 99, 100, 90,         },
			{93, 108, 100, 107, 100, 107, 100, 108, 93,     },
			{92, 98, 99, 103, 99, 103, 99, 98, 92,               },
			{90, 96, 103, 97, 94, 97, 103, 96, 90,               },
			{90, 90, 90, 96, 90, 96, 90, 90, 90,                   },
		};
		private static readonly int[,] Value_XIANG = {
			{0, 0, 20, 0, 0, 0, 20, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0},
			{18, 0, 0, 0, 23, 0, 0, 0, 18},
			{0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 20, 0, 0, 0, 20, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0},
		};
		private static readonly int[,] Value_SHI = {
			{0, 0, 0, 20, 0, 20, 0, 0, 0},
			{0, 0, 0, 0, 23, 0, 0, 0, 0},
			{0, 0, 0, 20, 0, 20, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0},
		};
		private static readonly int[,] Value_SHUAI = {
			{0, 0, 0, 11, 15, 11, 0, 0, 0},
			{0, 0, 0, 2, 2, 2, 0, 0, 0},
			{0, 0, 0, 1, 1, 1, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0},
		};
	}
}
