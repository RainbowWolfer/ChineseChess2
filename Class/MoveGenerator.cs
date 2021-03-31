using ChineseChess2.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess2.Class {
	public class MoveGenerator {
		public static int HEIGHT => ChessPage.HEIGHT;
		public static int WIDTH => ChessPage.WIDTH;

		public Dictionary<Vector2, Node> nodes;

		private bool IsUpRed => ChessPage.isUpRed;
		private bool IsRedTurn => ChessPage.IsRedTurn;
		private Side CurrentSide => ChessPage.CurrentSide;
		private Side OppositeSide => ChessPage.OppositeSide;

		public MoveGenerator(Dictionary<Vector2, Node> nodes) {
			this.nodes = nodes;
		}

		public List<Move> GenerateLegalMovs() {
			List<Move> pseudoMoves = GenerateMoves();
			List<Move> legalMoves = new List<Move>();
			foreach(Move moveToVerify in pseudoMoves) {
				ChessPage.MakeMove(moveToVerify);
				List<Move> opponentResponses = GenerateMoves();
				if(opponentResponses.Any(res =>
					ChessPage.GetKingNode(OppositeSide).pos == res.to
				)) {
					//still in check if do this move;
				} else {
					legalMoves.Add(moveToVerify);
				}

				ChessPage.UnmakeMove(moveToVerify);
			}

			return legalMoves;
		}

		public List<Move> GenerateMoves() {
			List<Move> result = new List<Move>();
			foreach(Node node in nodes.Values) {
				if(node.side != CurrentSide) {
					continue;
				}
				foreach(Move m in GetNodeMoves(node)) {
					result.Add(m);
				}
			}
			return result;
		}
		public List<Move> GetNodeMoves(Node startNode) {
			if(startNode.type == null) {
				throw new Exception("node type cannot be null");
			}
			List<Node> endNodes = new List<Node>();
			int _x = startNode.pos.x;
			int _y = startNode.pos.y;
			switch(startNode.type.Value) {
				case PieceType.BING: {
						bool oversea = IsOverSea(startNode);
						int y = _y + (IsSame(startNode.side) ? +1 : -1);
						if(IsInBorder(_x, y) && startNode.side != GetNode(_x, y).side) {
							endNodes.Add(GetNode(_x, y));
						}
						if(oversea) {
							if(IsInBorder(_x - 1, _y) && startNode.side != GetNode(_x - 1, _y).side) {
								endNodes.Add(GetNode(_x - 1, _y));
							}
							if(IsInBorder(_x + 1, _y) && startNode.side != GetNode(_x + 1, _y).side) {
								endNodes.Add(GetNode(_x + 1, _y));
							}
						}
						break;
					}
				case PieceType.PAO: {
						void Add(int initial, int max, bool add, bool hor) {
							bool encouter = false;
							for(int i = initial; add ? i < max : i >= 0; i += add ? 1 : -1) {
								int x = hor ? i : _x;
								int y = hor ? _y : i;
								if(IsInBorder(x, y)) {
									if(GetNode(x, y).side != Side.Empty && !encouter) {
										encouter = true;
										continue;
									}
									if(!encouter && startNode.side != GetNode(x, y).side) {
										endNodes.Add(GetNode(x, y));
									}
									if(GetNode(x, y).side != Side.Empty && encouter && startNode.side != GetNode(x, y).side) {
										endNodes.Add(GetNode(x, y));
										break;
									}
								}
							}
						}
						Add(_x + 1, WIDTH, true, true);
						Add(_x - 1, WIDTH, false, true);
						Add(_y + 1, HEIGHT, true, false);
						Add(_y - 1, HEIGHT, false, false);
						break;
					}
				case PieceType.JU: {
						void Add(int initial, int max, bool add, bool hor) {
							for(int i = initial; add ? i < max : i >= 0; i += add ? 1 : -1) {
								int x = hor ? i : _x;
								int y = hor ? _y : i;
								if(IsInBorder(x, y)) {
									if(startNode.side != GetNode(x, y).side) {
										endNodes.Add(GetNode(x, y));
									}
									if(GetNode(x, y).side != Side.Empty) {
										break;
									}
								}
							}
						}
						Add(_x + 1, WIDTH, true, true);
						Add(_x - 1, WIDTH, false, true);
						Add(_y + 1, HEIGHT, true, false);
						Add(_y - 1, HEIGHT, false, false);
						break;
					}
				case PieceType.MA: {
						void Add(int dx, int dy) {
							bool b = Math.Abs(dx) > Math.Abs(dy);
							if(IsInBorder(_x + dx, _y + dy)) {
								if(GetNode(_x + (b ? (dx > 0 ? 1 : -1) : 0), _y + (b ? 0 : (dy > 0 ? 1 : -1))).side == Side.Empty && startNode.side != GetNode(_x + dx, _y + dy).side) {
									endNodes.Add(GetNode(_x + dx, _y + dy));
								}
							}
						}
						Add(1, 2);
						Add(2, 1);
						Add(-1, 2);
						Add(2, -1);
						Add(-1, -2);
						Add(-2, -1);
						Add(1, -2);
						Add(-2, 1);
						break;
					}
				case PieceType.XIANG: {
						int m = 2;
						void Add(bool hor, bool ver) {//positive => +1
							int dx = hor ? m : -m;
							int dy = ver ? m : -m;
							if(!IsInBorder(_x + dx, _y + dy) || IsOverSea(GetNode(_x + dx, _y + dy), startNode.side) || startNode.side == GetNode(_x + dx, _y + dy).side) {
								return;
							}
							for(int i = 1; i < m; i++) {
								if(GetNode(_x + i * (hor ? 1 : -1), _y + i * (ver ? 1 : -1)).side != Side.Empty) {
									return;
								}
							}
							endNodes.Add(GetNode(_x + dx, _y + dy));
						}
						Add(true, true);
						Add(true, false);
						Add(false, true);
						Add(false, false);
						break;
					}
				case PieceType.SHI: {
						void Add(bool hor, bool ver) {//positive => +1
							int dx = hor ? 1 : -1;
							int dy = ver ? 1 : -1;
							if(!IsInBorder(_x + dx, _y + dy) || IsOverSea(GetNode(_x + dx, _y + dy), startNode.side) || !GetNode(_x + dx, _y + dy).castle || startNode.side == GetNode(_x + dx, _y + dy).side) {
								return;
							}
							endNodes.Add(GetNode(_x + dx, _y + dy));
						}
						Add(true, true);
						Add(true, false);
						Add(false, true);
						Add(false, false);
						break;
					}
				case PieceType.SHUAI: {
						void Add(int dx, int dy) {
							if(IsInBorder(_x + dx, _y + dy) && GetNode(_x + dx, _y + dy).castle && startNode.side != GetNode(_x + dx, _y + dy).side) {
								endNodes.Add(GetNode(_x + dx, _y + dy));
							}
						}
						Add(0, 1);
						Add(0, -1);
						Add(1, 0);
						Add(-1, 0);

						bool b = IsSame(startNode.side);
						for(int i = _y; b ? i < HEIGHT : i >= 0; i += b ? 1 : -1) {
							if(IsInBorder(_x, i)) {
								if(GetNode(_x, i) == startNode) {
									continue;
								}
								if(GetNode(_x, i).type == PieceType.SHUAI) {
									//MainPage.Log(node + " | " + ns[_x, i));
									endNodes.Add(GetNode(_x, i));
									break;
								}
								if(GetNode(_x, i).side != Side.Empty && GetNode(_x, i).type != PieceType.SHUAI) {
									break;
								}
							}
						}

						break;
					}
				default:
					throw new Exception();
			}
			var result = new List<Move>();
			foreach(var item in endNodes) {
				result.Add(new Move(startNode.pos, item.pos));
			}
			return result;
		}
		private bool IsSame(Side s) => !(IsUpRed ^ s == Side.Red);
		private bool IsOverSea(Node node) {
			return IsOverSea(node, node.side);
		}
		private bool IsOverSea(Node node, Side s) {
			if(IsSame(s)) {
				return node.pos.y >= 5;
			} else {
				return node.pos.y <= 4;
			}
		}
		private bool IsInBorder(int x, int y) {
			return x >= 0 && x < WIDTH && y >= 0 && y < HEIGHT;
		}

		private Node GetNode(int x, int y) {
			return nodes[new Vector2(x, y)];
		}

	}

	public class Move {
		public Vector2 from;
		public Vector2 to;

		public Side originalSide;
		public PieceType originalType;

		public void SetOriginal(Side side, PieceType pieceType) {
			originalSide = side;
			originalType = pieceType;
		}

		public Move(Vector2 from, Vector2 to) {
			this.from = from;
			this.to = to;
			originalSide = Side.Empty;//default
		}

		public override bool Equals(object obj) {
			return obj is Move move &&
				EqualityComparer<Vector2>.Default.Equals(from, move.from) &&
				EqualityComparer<Vector2>.Default.Equals(to, move.to);
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}
		public override string ToString() {
			return from + " TO " + to;
		}
	}
}
