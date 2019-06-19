using System.Windows.Input;
using FreshMvvm;
using Xamarin.Forms;

namespace PushTestApp.PageModels
{
	public class MainPageModel : FreshBasePageModel
	{
		private bool _isShowingPopup;
		private string _popupMessage;

		public MainPageModel()
		{
			DismissPopupCommand = new Command(() => DismissPopup());
		}

		public bool IsShowingPopup
		{
			get
			{
				return _isShowingPopup;
			}
			set
			{
				_isShowingPopup = value;
				RaisePropertyChanged();
			}
		}

		public string PopupMessage
		{
			get
			{
				return $"Our message type is: {_popupMessage }";
			}
			set
			{
				_popupMessage = value;
				RaisePropertyChanged();
			}
		}

		private void DismissPopup()
		{
			IsShowingPopup = false;
		}

		public ICommand DismissPopupCommand
		{
			get;
			private set;
		}
	}
}
