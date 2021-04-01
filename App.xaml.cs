using ChineseChess2.Class;
using ChineseChess2.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ChineseChess2 {
	sealed partial class App: Application {
		public App() {
			this.InitializeComponent();
			this.Suspending += OnSuspending;
			//Task.Run(async () => {
			//	await Task.Delay(1);
			//	Debug.WriteLine("This is a thread");
			//});
		}

		//public 

		protected override void OnLaunched(LaunchActivatedEventArgs e) {
			if(!(Window.Current.Content is Frame rootFrame)) {
				rootFrame = new Frame();

				rootFrame.NavigationFailed += OnNavigationFailed;

				if(e.PreviousExecutionState == ApplicationExecutionState.Terminated) {
					//TODO: Load state from previously suspended application
				}

				Window.Current.Content = rootFrame;
			}

			if(e.PrelaunchActivated == false) {
				if(rootFrame.Content == null) {
					rootFrame.Navigate(typeof(MainPage), e.Arguments);
				}
				Window.Current.Activate();
			}
			Window.Current.CoreWindow.KeyDown += async (s, c) => {
				switch(c.VirtualKey) {
					case VirtualKey.C:
						string str = Translator.Translate(ChessPage.nodes);
						DataPackage dp = new DataPackage() {
							RequestedOperation = DataPackageOperation.Copy
						};
						dp.SetText(str);
						Clipboard.SetContent(dp);
						break;
					case VirtualKey.Q:
						ChessPage.DrawTrueFalse(new MoveOrdering(new MoveGenerator(ChessPage.nodes)).GetOpponentAttackMap());
						break;
					case VirtualKey.F:
						//Debug.WriteLine(PieceValue.Evaluate(Side.Red));
						//ChessPage.IsInCheck(out Side side);
						//Debug.WriteLine(side);
						MainPage.ShowSearchingText(true);

						List<Move> moves = new MoveGenerator(ChessPage.nodes).GenerateLegalMovs();
						var o = new MoveOrdering(new MoveGenerator(ChessPage.nodes));
						o.OrderMoves(moves);
						o.PrintToMainPage(moves);

						await Task.Delay(1);
						AI ai = new AI();

						long start = DateTime.Now.Ticks;
						int sSec = DateTime.Now.Second;
						ai.StartSearch(MainPage.searchDepth);
						long end = DateTime.Now.Ticks;
						int eSec = DateTime.Now.Second;
						MainPage.Log(((end - start) / 10000).ToString() + "ms\n" + (eSec - sSec).ToString() + "  Count : " + ai.searchCount);

						Debug.WriteLine(ai.bestMove + "_" + ai.bestEval);
						ChessPage.MakeMove(ai.bestMove);


						MainPage.ShowSearchingText(false);
						break;
				}
			};
		}
		public static string GetCurrentTime() {
			return " (" + DateTime.Now.Hour + " : " + DateTime.Now.Minute + " : " + DateTime.Now.Second + ") ";
		}

		void OnNavigationFailed(object sender, NavigationFailedEventArgs e) {
			throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
		}

		private void OnSuspending(object sender, SuspendingEventArgs e) {
			var deferral = e.SuspendingOperation.GetDeferral();
			//TODO: Save application state and stop any background activity
			deferral.Complete();
		}
	}
}
