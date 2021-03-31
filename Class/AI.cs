using ChineseChess2.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess2.Class {
	public class AI {
		public int Search(int depth, int alpha, int beta) {
			if(depth == 0) {
				int eval = PieceValue.Evaluate(Side.Black);
				return eval;
			}

			List<Move> moves = new MoveGenerator(ChessPage.nodes).GenerateLegalMovs();

			if(moves.Count == 0) {
				return int.MinValue;
			}

			foreach(Move m in moves) {
				ChessPage.MakeMove(m);
				int evaluation = -Search(depth - 1, -beta, -alpha);
				ChessPage.UnmakeMove(m);
				if(evaluation >= beta) {
					return beta;
				}
				alpha = Math.Max(alpha, evaluation);
			}

			return alpha;
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
