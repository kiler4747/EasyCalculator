using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Calc;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EasyCalculator
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			this.InitializeComponent();
		}

		Calculator calc = new Calculator();

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.  The Parameter
		/// property is typically used to configure the page.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			tbResult.Text = calc.Calculate(((TextBox)sender).Text).ToString("G");
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			int cursorPosition = tbExpression.SelectionStart;			
			tbExpression.Text = tbExpression.Text.Insert(cursorPosition,((Button) sender).Content.ToString());
			tbExpression.SelectionStart = cursorPosition + ((Button)sender).Content.ToString().Length;
			tbExpression.Focus(FocusState.Keyboard);
		}

		private void btClear_Click(object sender, RoutedEventArgs e)
		{
			tbExpression.Text = string.Empty;
			tbExpression.Focus(FocusState.Keyboard);
		}

		private void btCos_Click(object sender, RoutedEventArgs e)
		{
			int cursorPosition = tbExpression.SelectionStart;
			tbExpression.Text = tbExpression.Text.Insert(cursorPosition,((Button)sender).Content.ToString() + "()");
			tbExpression.SelectionStart = cursorPosition + (((Button) sender).Content.ToString() + "()").Length - 1;
			tbExpression.Focus(FocusState.Keyboard);
			//tbExpression.SelectionStart;
		}

	}
}
