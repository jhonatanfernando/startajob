using System;
using Xamarin.Forms;

namespace SmallTest
{
	public class LoginViewModel : BaseViewModel
	{
		private Login login;

		public LoginViewModel()
		{
			login = new Login();
		}

		public Login Login
		{
			get
			{
				return login;
			}
			set
			{
				login = value;

				if (value == null)
					return;


				OnPropertyChanged("Login");
			}
		}

		Command loginCommand;
		public Command LoginCommand
		{
			get
			{
				return loginCommand ??
					(loginCommand = new Command(ExecuteLoginCommand));
			}
		}

		async void ExecuteLoginCommand()
		{
			try
			{
				var result = new ServiceSoap().Execute(login.UserName, login.Password);

				if (result is ResultError)
				{
					await Dialogs.AlertAsync((result as ResultError).ResultMessage, "Information", "Ok", null);
				}
				else
				{
					await Dialogs.AlertAsync("User " + (result as Entity).FirstName + " was found!", "Information", "Ok", null);
				}
			}
			catch (Exception e)
			{
				await Dialogs.AlertAsync(e.Message, "Error", "Ok", null);
			}



		}
	}
}
