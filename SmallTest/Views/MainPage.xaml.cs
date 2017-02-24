using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SmallTest
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
			this.BindingContext = new LoginViewModel();
		}
	}
}
