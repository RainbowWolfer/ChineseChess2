using ChineseChess2.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess2.Class {
	public class MoveOrdering {
		private readonly MoveGenerator moveGenerator;

		private readonly Dictionary<Vector2, bool> opponentAttackMap;
		private readonly List<int> moveScores;

		public MoveOrdering(MoveGenerator moveGenerator) {
			this.moveGenerator = moveGenerator;
			moveScores = new List<int>();
			opponentAttackMap = new Dictionary<Vector2, bool>();
			CalculateOpponentAttackMap(null);
		}
		public Dictionary<Vector2, bool> GetOpponentAttackMap() => opponentAttackMap;
		private void InitializeOpponentAttackMap() {
			opponentAttackMap.Clear();
			for(int i = 0; i < ChessPage.WIDTH; i++) {
				for(int j = 0; j < ChessPage.HEIGHT; j++) {
					opponentAttackMap.Add(new Vector2(i, j), false);
				}
			}
		}
		private void CalculateOpponentAttackMap(PieceType? pieceType) {
			InitializeOpponentAttackMap();
			foreach(Node n in moveGenerator.nodes.Values) {
				if(n.side == ChessPage.CurrentSide || n.side == Side.Empty) {
					continue;
				}
				bool Check(Move m) {
					if(ChessPage.GetNode(m.from).type == PieceType.PAO) {
						return ChessPage.GetNode(m.to).type != null;
					} else {
						return true;
					}
				}
				List<Move> ms = moveGenerator.GetNodeMoves(n, true);
				if(pieceType == null) {
					ms.ForEach((m) => opponentAttackMap[m.to] = Check(m));
				} else if(n.type == pieceType.Value) {
					ms.ForEach((m) => opponentAttackMap[m.to] = Check(m));
				}
			}
		}

		public void OrderMoves(List<Move> moves) {
			moveScores.Clear();
			foreach(Move m in moves) {
				int moveScoreGuess = 0;

				Node from = ChessPage.GetNode(m.from);
				Node to = ChessPage.GetNode(m.to);

				if(to.type != null) {
					moveScoreGuess = 10 * PieceValue.GetNodeValue(to) - PieceValue.GetNodeValue(from);
				}
				if(opponentAttackMap[m.to] == true) {
					switch(from.type.Value) {
						case PieceType.BING:
							moveScoreGuess = -100;
							break;
						case PieceType.PAO:
							moveScoreGuess = -800;
							break;
						case PieceType.JU:
							moveScoreGuess = -1000;
							break;
						case PieceType.MA:
							moveScoreGuess = -600;
							break;
						case PieceType.XIANG:
							moveScoreGuess = -400;
							break;
						case PieceType.SHI:
							moveScoreGuess = -500;
							break;
						case PieceType.SHUAI:
							moveScoreGuess = -10000;
							break;
						default:
							throw new Exception();
					}
				}
				moveScores.Add(moveScoreGuess);
			}
			Sort(moves);
		}
		public void Sort(List<Move> moves) {
			for(int a = 0; a < moves.Count - 1; a++) {
				for(int b = 0; b < moves.Count - 1 - a; b++) {
					if(moveScores[b] < moveScores[b + 1]) {
						Move tmp1 = moves[b + 1];
						moves[b + 1] = moves[b];
						moves[b] = tmp1;

						int tmp2 = moveScores[b + 1];
						moveScores[b + 1] = moveScores[b];
						moveScores[b] = tmp2;
					}
				}
			}
		}
		public void PrintToMainPage(List<Move> moves) {
			string result = "Move Ordering " + (ChessPage.IsRedTurn ? "红方着棋" : "黑方着棋") + '\n';

			for(int i = 0; i < moves.Count; i++) {
				Move m = moves[i];
				string from = m.from + ChessPage.ConvertChinese(m.from);
				string to = m.to + ChessPage.ConvertChinese(m.to);
				result += from + " TO " + to + " _ " + moveScores[i] + "\n";
			}

			MainPage.Log(result);
		}
	}
}
