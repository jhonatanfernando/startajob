using System;
using Acr.UserDialogs;
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
					ToastConfig config = new ToastConfig((result as ResultError).ResultMessage);
					config.SetBackgroundColor(System.Drawing.Color.Red);
					Dialogs.Toast(config);
				}
				else
				{
					ToastConfig config = new ToastConfig("User " + (result as Entity).FirstName + " was found!");
					config.SetBackgroundColor(System.Drawing.Color.Green);
					Dialogs.Toast(config);
				}
			}
			catch (Exception e)
			{
				await Dialogs.AlertAsync(e.Message, "Error", "Ok", null);
			}



		}
	}
}
