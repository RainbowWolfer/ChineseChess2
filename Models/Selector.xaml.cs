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

namespace ChineseChess2.Models {
	public sealed partial class Selector: UserControl {
		public Color color;
		public Vector2 pos;

		public SolidColorBrush ColorBrush => new SolidColorBrush(color);

		public bool Visible {
			get => MyGrid.Visibility == Visibility.Visible;
			set => MyGrid.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
		}

		public Selector() {
			this.InitializeComponent();
			color = Colors.Blue;
		}
	}
}
