using ChineseChess2.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
	}
}