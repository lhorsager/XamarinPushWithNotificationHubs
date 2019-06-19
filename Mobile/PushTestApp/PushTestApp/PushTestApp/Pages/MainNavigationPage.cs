using System;
using System.Threading.Tasks;
using FreshMvvm;
using Xamarin.Forms;

namespace PushTestApp.Pages
{
	public class MainNavigationPage : NavigationPage, IFreshNavigationService 
	{
		public string NavigationServiceName { get; private set; }

		public MainNavigationPage(Page page) : this(page, "MainPage")
		{
		}

		public MainNavigationPage(Page page, string navigationPageName) : base(page)
		{
			var pageModel = page.GetModel();
			pageModel.CurrentNavigationServiceName = navigationPageName;
			NavigationServiceName = navigationPageName;
			RegisterNavigation();
		}

		protected void RegisterNavigation()
		{
			FreshIOC.Container.Register<IFreshNavigationService>(this, NavigationServiceName);
		}

		public void NotifyChildrenPageWasPopped()
		{
			this.NotifyAllChildrenPopped();
		}

		public virtual Task PopPage(bool modal = false, bool animate = true)
		{
			if (modal)
				return Navigation.PopModalAsync(animate);
			return Navigation.PopAsync(animate);
		}


		public Task PopToRoot(bool animate = true)
		{
			return Navigation.PopToRootAsync(animate);
		}

		internal Page CreateContainerPageSafe(Page page)
		{
			if (page is NavigationPage || page is MasterDetailPage || page is Xamarin.Forms.TabbedPage)
			{
				return page;
			}

			return CreateContainerPage(page);
		}

		protected virtual Page CreateContainerPage(Page page)
		{
			return new NavigationPage(page);
		}

		public virtual Task PushPage(Xamarin.Forms.Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
		{
			if (modal)
				return Navigation.PushModalAsync(CreateContainerPageSafe(page), animate);

			model.CurrentNavigationServiceName = NavigationServiceName;
			return Navigation.PushAsync(page, animate);
		}

		public Task<FreshBasePageModel> SwitchSelectedRootPageModel<T>() where T : FreshBasePageModel
		{
			throw new Exception("This navigation container has no selected roots, just a single root");
		}
	}
}
