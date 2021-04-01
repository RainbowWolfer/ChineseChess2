using ChineseChess2.Class;
using ChineseChess2.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace ChineseChess2 {
	public sealed partial class MainPage: Page {
		public static MainPage Instance;
		public static StackPanel _MyInfoPanel => Instance.MyInfoPanel;
		public static int searchDepth = 2;
		public MainPage() {
			Instance = this;
			this.InitializeComponent();
			MyFrame.Navigate(typeof(ChessPage));
		}
		public static void Log(string content) {
			_MyInfoPanel.Children.Insert(0, new TextBlock() {
				Text = App.GetCurrentTime() + content,
				FontSize = 20,
				TextWrapping = TextWrapping.WrapWholeWords,
				Margin = new Thickness(0, 10, 0, 10)
			});
		}
		public static void ShowSearchingText(bool enable) {
			Instance.MySearchingText.Visibility = enable ? Visibility.Visible : Visibility.Collapsed;
		}

		private void Click1(object sender, RoutedEventArgs e) {
			MyDropDownButton.Content = "1";
			searchDepth = 1;
		}
		private void Click2(object sender, RoutedEventArgs e) {
			MyDropDownButton.Content = "2";
			searchDepth = 2;
		}
		private void Click3(object sender, RoutedEventArgs e) {
			MyDropDownButton.Content = "3";
			searchDepth = 3;
		}
		private void Click4(object sender, RoutedEventArgs e) {
			MyDropDownButton.Content = "4";
			searchDepth = 4;
		}
		private void Click5(object sender, RoutedEventArgs e) {
			MyDropDownButton.Content = "5";
			searchDepth = 5;
		}

		private void UndoButton_Click(object sender, RoutedEventArgs e) {
			ChessPage.UndoMode();
		}

		private async void MySearchButton_Click(object sender, RoutedEventArgs e) {
			ShowSearchingText(true);

			List<Move> moves = new MoveGenerator(ChessPage.nodes).GenerateLegalMovs();
			var o = new MoveOrdering(new MoveGenerator(ChessPage.nodes));
			o.OrderMoves(moves);
			o.PrintToMainPage(moves);

			await Task.Delay(1);
			AI ai = new AI();

			long start = DateTime.Now.Ticks;
			int sSec = DateTime.Now.Second;
			ai.StartSearch(searchDepth);
			long end = DateTime.Now.Ticks;
			int eSec = DateTime.Now.Second;
			Log(((end - start) / 10000).ToString() + "ms\n" + (eSec - sSec).ToString() + "  Count : " + ai.searchCount);

			Debug.WriteLine(ai.bestMove + "_" + ai.bestEval);
			ChessPage.MakeMove(ai.bestMove);


			ShowSearchingText(false);
		}
	}
}