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

		public Move bestMove;
		public int bestEval;

		public void StartSearch(int depth) {
			
			Search(depth, 0, NegativeInfinity, PositiveInfinity);

		}
		public int Search(int depth, int plyFromRoot, int alpha, int beta) {
			if(depth == 0) {
				int eval = PieceValue.Evaluate(ChessPage.CurrentSide);
				return eval;
			}

			List<Move> moves = new MoveGenerator(ChessPage.nodes).GenerateLegalMovs();
			OrderMoves(moves);

			if(moves.Count == 0) {
				return NegativeInfinity;
			}

			foreach(Move m in moves) {
				ChessPage.MakeMove(m, updateDisplay: false);
				int evaluation = -Search(depth - 1, plyFromRoot + 1, -beta, -alpha);
				ChessPage.UnmakeMove(m, updateDisplay: false);
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
		public int Search() {
			return 0;
		}
		public void OrderMoves(List<Move> moves) {
			var mg = new MoveGenerator(ChessPage.nodes);

			foreach(Move m in moves) {
				int moveScoreGuess = 0;

				Node from = ChessPage.GetNode(m.from);
				Node to = ChessPage.GetNode(m.to);

				if(to.type == null) {

				}

				//foreach(Move m in mg.GenerateLegalMovs) {

				//}
				if(false) {

				}

			}
		}
	}
}
