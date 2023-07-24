using System;
using C971proj.Views;
using C971proj.Services;
using C971proj.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971proj
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage (new Login());
            if (Settings.FirstRun)
            {
              DatabaseServices.LoadSampleData();
              Settings.FirstRun = false;       
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
