using ChineseChess2.Class;
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
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace ChineseChess2.Pages {
	public sealed partial class ChessNode: UserControl {
		public Node node;
		public Vector2 pos;

		public Action<Node, Vector2> onClick;

		public int x => pos.x;
		public int y => pos.y;

		public ChessNode(Node node) {
			this.node = node;
			this.pos = node.pos;
			this.InitializeComponent();

			UpdateChessDisplay();
		}
		public void UpdateChessDisplay() {
			if(y == 0) {
				BorderU.Visibility = Visibility.Visible;
				VU.Visibility = Visibility.Collapsed;
			} else if(y == ChessPage.HEIGHT - 1) {
				BorderD.Visibility = Visibility.Visible;
				VD.Visibility = Visibility.Collapsed;
			}
			if(x == 0) {
				BorderL.Visibility = Visibility.Visible;
				HL.Visibility = Visibility.Collapsed;
			} else if(x == ChessPage.WIDTH - 1) {
				BorderR.Visibility = Visibility.Visible;
				HR.Visibility = Visibility.Collapsed;
			}

			if(y == 4 && x != 0 && x != ChessPage.WIDTH - 1) {
				VD.Visibility = Visibility.Collapsed;
			} else if(y == 5 && x != 0 && x != ChessPage.WIDTH - 1) {
				VU.Visibility = Visibility.Collapsed;
			}

			if(x == 3 && y == 0) {
				LDR.Visibility = Visibility.Visible;
			} else if(x == 5 && y == 0) {
				LDL.Visibility = Visibility.Visible;
			} else if(x == 3 && y == 2) {
				LUR.Visibility = Visibility.Visible;
			} else if(x == 5 && y == 2) {
				LUL.Visibility = Visibility.Visible;
			} else if(x == 4 && y == 1) {
				LDR.Visibility = Visibility.Visible;
				LDL.Visibility = Visibility.Visible;
				LUR.Visibility = Visibility.Visible;
				LUL.Visibility = Visibility.Visible;
			}
			if(x == 3 && y == ChessPage.HEIGHT - 3) {
				LDR.Visibility = Visibility.Visible;
			} else if(x == 5 && y == ChessPage.HEIGHT - 3) {
				LDL.Visibility = Visibility.Visible;
			} else if(x == 3 && y == ChessPage.HEIGHT - 1) {
				LUR.Visibility = Visibility.Visible;
			} else if(x == 5 && y == ChessPage.HEIGHT - 1) {
				LUL.Visibility = Visibility.Visible;
			} else if(x == 4 && y == ChessPage.HEIGHT - 2) {
				LDR.Visibility = Visibility.Visible;
				LDL.Visibility = Visibility.Visible;
				LUR.Visibility = Visibility.Visible;
				LUL.Visibility = Visibility.Visible;
			}

			if(node.side == Side.Empty) {
				MyElipse.Visibility = Visibility.Collapsed;
				MyNameText.Visibility = Visibility.Collapsed;
			} else {
				MyElipse.Visibility = Visibility.Visible;
				MyNameText.Visibility = Visibility.Visible;
				Color color;
				if(node.side == Side.Black) {
					color = Colors.Black;
				} else if(node.side == Side.Red) {
					color = Colors.Red;
				}
				MyElipse.Stroke = new SolidColorBrush(color);
				MyNameText.Text = ChessPage.ConvertChinese(node.type.Value, node.side);
			}
			//MyTestText.Text = node.pos.ToString();
			MyTestText.Text = "";
		}

		private void Grid_Tapped(object sender, TappedRoutedEventArgs e) {
			onClick?.Invoke(this.node, this.pos);
		}

		public override string ToString() {
			return pos + "_" + node.side + "_" + node.type;
		}
	}
}
