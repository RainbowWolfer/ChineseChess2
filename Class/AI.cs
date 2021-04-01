using ChineseChess2.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess2.Class {
	public class AI {
		public const int PositiveInfinity = 99999;
		public const int NegativeInfinity = -PositiveInfinity;

		private MoveOrdering moveOrdering;

		public Move bestMove;
		public int bestEval;
		public int searchCount;

		public AI() {
			moveOrdering = new MoveOrdering(new MoveGenerator(ChessPage.nodes));
		}

		public void StartSearch(int depth) {
			searchCount = 0;
			Search(depth, 0, NegativeInfinity, PositiveInfinity);
		}
		private int Search(int depth, int plyFromRoot, int alpha, int beta) {
			if(depth == 0) {
				int eval = PieceValue.Evaluate(ChessPage.CurrentSide);
				return eval;
			}

			List<Move> moves = new MoveGenerator(ChessPage.nodes).GenerateLegalMovs();
			moveOrdering.OrderMoves(moves);
			if(moves.Count > 0 && plyFromRoot == 0) {
				bestMove = moves[0];
			}

			if(moves.Count == 0) {
				return NegativeInfinity;
			}

			foreach(Move m in moves) {
				ChessPage.MakeMove(m, updateDisplay: false);
				int evaluation = -Search(depth - 1, plyFromRoot + 1, -beta, -alpha);
				ChessPage.UnmakeMove(m, updateDisplay: false);
				searchCount++;
				if(evaluation >= beta) {
					return beta;
				}
				if(evaluation > alpha) {
					alpha = evaluation;
					if(plyFromRoot == 0) {
						bestMove = m;
						bestEval = evaluation;
					}
				}
			}

			return alpha;
		}
		private int Search() {
			return 0;
		}

	}
}
