using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C971proj.Models;
using C971proj.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971proj.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppSettings : ContentPage
    {
        public AppSettings()
        {
            InitializeComponent();
        }
        /*
        /// <summary>
        /// Clears Preferences
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearPreferences_Clicked(object sender, EventArgs e)
        {
            Preferences.Clear();
        }
        */
        /// <summary>
        /// Clears All Data from database and Loads Sample Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void LoadSampleData_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Clear Data, Load Sample Data, Logout", "Clear All Current Data in the Database, Load Sample Data, Logout?", "No", "Yes");

            if (answer == false)
            {
                await DatabaseServices.ClearAllData();
                await DisplayAlert("Data Cleared", "All Data in the database deleted and Sample Data Loaded", "OK");
                            
                DatabaseServices.LoadSampleData();
                Settings.FirstRun = false;

                await Navigation.PushAsync(new Login());
            }
            else
            {
                await DisplayAlert("Canceled", "No Data Deleted", "OK");
            }
        }

        /*
        /// <summary>
        /// Clears all data from the database, user must confirm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void ClearAllData_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Clear Data!", "Clear All Current Data in the Database?", "No", "Yes");

            if (answer == false)
            {
                await DatabaseServices.ClearAllData();
                await DisplayAlert("Data Cleared", "All Data in the database deleted", "OK");
            }
            else
            {
                await DisplayAlert("Canceled", "No Data Deleted", "OK");
            }
        }
       */
    }
}