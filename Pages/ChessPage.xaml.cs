using ChineseChess2.Class;
using ChineseChess2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ChineseChess2.Pages {
	public delegate void OnTurnChangeEventHandler(bool isRedTurn);
	public sealed partial class ChessPage: Page {
		public const int WIDTH = 9;
		public const int HEIGHT = 10;
		public const string DEFAULTOPENING = "jmxsksxmj/9/1p5p1/b1b1b1b1b/9/9/B1B1B1B1B/1P5P1/9/JMXSKSXMJ/";

		public static List<ChessNode> chessNodes;
		public static Dictionary<Vector2, Node> nodes;

		public Selector selector;
		public List<Selector> legalMoveSelectors;
		public Selector[] lastMoveSelectors;

		public static event OnTurnChangeEventHandler OnTurnChanged;

		private Vector2 startPos;
		private Vector2 DefaultPos => new Vector2(-1, -1);
		private bool Selected => startPos != DefaultPos;

		public static List<Move> history;
		/// <summary>true means player is black, false means player is red</summary>
		public static bool isUpRed;

		private static bool isRedTurn;
		public static bool IsRedTurn {
			get => isRedTurn;
			set {
				isRedTurn = value;
				OnTurnChanged?.Invoke(IsRedTurn);
			}
		}
		public static Side CurrentSide => IsRedTurn ? Side.Red : Side.Black;
		public static Side OppositeSide => IsRedTurn ? Side.Black : Side.Red;

		public ChessPage() {
			this.InitializeComponent();
			startPos = DefaultPos;
			history = new List<Move>();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e) {
			base.OnNavigatedTo(e);

			chessNodes = new List<ChessNode>();
			nodes = new Dictionary<Vector2, Node>();

			legalMoveSelectors = new List<Selector>();
			lastMoveSelectors = new Selector[2];

			selector = new Selector(Vector2.Zero, Colors.Blue) { Visible = false };
			MyMainGrid.Children.Insert(0, selector);

			IsRedTurn = false;
			for(int i = 0; i < WIDTH; i++) {
				for(int j = 0; j < HEIGHT; j++) {
					nodes = Translator.LoadNodes(
						"jmxsksxmj/9/4p2p1/b1b1P1b1b/9/9/B1B1B1B1B/7P1/9/JMXSKSXMJ/"
					);
					ChessNode cn = new ChessNode(GetNode(i, j));
					cn.onClick += (n, p) => {
						List<Move> legalMoves = new MoveGenerator(nodes).GenerateLegalMovs();
						if(legalMoves.Count == 0) {
							throw new Exception("WOW");
						}
						void Select(Vector2 target) {
							startPos = target;
							SetSelector(target);
							ShowLegalMoves(legalMoves.Where((m) => m.from == cn.pos).ToList());
						}
						void Deselect() {
							startPos = DefaultPos;
							SetSelector(DefaultPos);
							ClearLegalMoveSelectos();
						}
						if(Selected) {
							if(p == startPos) {
								Deselect();
								return;
							}
							if(GetNode(p).side == GetNode(startPos).side) {
								Select(p);
								return;
							}
							if(!legalMoves.Contains(new Move(startPos, p))) {
								return;
							}
							MakeMove(new Move(startPos, p));
							Deselect();
						} else {
							if(GetNode(p).side == CurrentSide) {
								Select(p);
							}
						}
					};
					MyMainGrid.Children.Insert(0, cn);
					Grid.SetColumn(cn, i);
					Grid.SetRow(cn, j);
					chessNodes.Add(cn);
				}
			}

			TextBlock tb = new TextBlock() {
				Text = "楚河                        汉界",
				FontSize = 48,
				Foreground = new SolidColorBrush(Colors.Black),
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center
			};
			Grid.SetRow(tb, 4);
			Grid.SetRowSpan(tb, 2);
			Grid.SetColumnSpan(tb, 9);

			MyMainGrid.Children.Insert(0, tb);
		}

		public static void MakeMove(Move m) {
			Node a = GetNode(m.from);
			Node b = GetNode(m.to);
			if(b.side != Side.Empty) {
				m.SetOriginal(b.side, b.type.Value);
				CaptureNodes(a, b);
			} else {
				SwapNodes(b, a);
			}
			IsRedTurn = !IsRedTurn;
			history.Add(m);
			UpdateDisplay();
			//ChessPage.ShowLastMove(a, b);

			//if(!ChessPage.GameOver) {
			//	//MainPage.Log(ChessPage.redTurn ? "红方->黑方" : "黑方->红方");
			//	ChessPage.redTurn = !ChessPage.redTurn;
			//}
		}
		public static void UndoMode() {
			if(history.Count > 0) {
				UnmakeMove(history[history.Count - 1]);
			}
			//GetNode(0, 0).type = PieceType.BING;
			//UpdateDisplay();
		}
		public static void UnmakeMove(Move m) {
			GetNode(m.from).type = GetNode(m.to).type.Value;
			GetNode(m.from).side = GetNode(m.to).side;
			if(m.originalSide == Side.Empty) {
				GetNode(m.to).type = null;
				GetNode(m.to).side = Side.Empty;
			} else {
				GetNode(m.to).type = m.originalType;
				GetNode(m.to).side = m.originalSide;
			}

			IsRedTurn = !IsRedTurn;
			history.RemoveAt(history.Count - 1);
			UpdateDisplay();
		}
		public static void CaptureNodes(Node a, Node b) {
			b.side = a.side;
			b.type = a.type;
			a.side = Side.Empty;
			a.type = null;
		}
		public static void SwapNodes(Node a, Node b) {
			(PieceType?, Side) tmp = (a.type, a.side);
			a.type = b.type;
			a.side = b.side;
			b.type = tmp.Item1;
			b.side = tmp.Item2;
		}

		public static void UpdateDisplay() {
			foreach(ChessNode item in chessNodes) {
				item.node = nodes[item.pos];
				item.UpdateChessDisplay();
			}
		}
		public void ShowLegalMoves(List<Move> ms) {
			ClearLegalMoveSelectos();
			foreach(Move m in ms) {
				Selector s = new Selector(m.to, Colors.Green);
				MyMainGrid.Children.Add(s);
				legalMoveSelectors.Add(s);
			}
		}
		public void ClearLegalMoveSelectos() {
			foreach(Selector s in legalMoveSelectors) {
				MyMainGrid.Children.Remove(s);
			}
			legalMoveSelectors.Clear();
		}
		public void SetSelector(Vector2 v) {
			if(v == DefaultPos) {
				selector.Visible = false;
				return;
			}
			selector.Visible = true;
			selector.Position = v;
		}
		public static Node GetKingNode(Side s) {
			Node king = null;
			foreach(Node n in nodes.Values) {
				if(!n.castle) {
					continue;
				}
				if(n.type == PieceType.SHUAI && n.side == s) {
					king = n;
					break;
				}
			}
			return king;
		}

		public static Node GetNode(int x, int y) => nodes[new Vector2(x, y)];
		public static Node GetNode(Vector2 v) => GetNode(v.x, v.y);

		public static string ConvertChinese(PieceType pt, Side side) {
			if(side == Side.Empty) {
				return "空";
			}
			switch(pt) {
				case PieceType.BING:
					return side == Side.Red ? "兵" : "卒";
				case PieceType.PAO:
					return side == Side.Red ? "炮" : "砲";
				case PieceType.JU:
					return "車";
				case PieceType.MA:
					return "马";
				case PieceType.XIANG:
					return side == Side.Red ? "相" : "象";
				case PieceType.SHI:
					return side == Side.Red ? "仕" : "士";
				case PieceType.SHUAI:
					return side == Side.Red ? "帅" : "将";
				default:
					throw new Exception();
			}
		}
	}
}
