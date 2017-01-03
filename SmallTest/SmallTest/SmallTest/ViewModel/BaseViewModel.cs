using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Forms;
using System.Linq;

namespace SmallTest
{
	public class BaseViewModel : INotifyPropertyChanged
	{

		public BaseViewModel()
		{
			Dialogs = UserDialogs.Instance;
		}

		public IUserDialogs Dialogs { get; private set; }

		public bool IsInitialized { get; set; }

		public async Task PushModalAsync(Page page)
		{
			if (App.Navigation != null)
				await App.Navigation.PushModalAsync(page);
		}

		public async Task PopModalAsync()
		{
			if (App.Navigation != null)
				await App.Navigation.PopModalAsync();
		}

		public async Task PushAsync(Page page)
		{
			if (App.Navigation != null)
				await App.Navigation.PushAsync(page);
		}

		public async Task PopAsync()
		{
			if (App.Navigation != null)
				await App.Navigation.PopAsync();
		}

		public void RemoveStackPages(Page exceptedPage = null)
		{
			var existingPages = App.Navigation.NavigationStack.ToList();
			foreach (var page in existingPages)
			{
				if (page != null && page == exceptedPage)
					continue;

				App.Navigation.RemovePage(page);
			}
		}

		#region INotifyPropertyChanging implementation

		public event PropertyChangingEventHandler PropertyChanging;

		#endregion

		public void OnPropertyChanging(string propertyName)
		{
			if (PropertyChanging == null)
				return;

			PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
		}

		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		public void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged == null)
				return;

			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

