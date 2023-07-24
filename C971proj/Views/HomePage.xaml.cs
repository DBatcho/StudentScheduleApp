using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C971proj.Models;
using C971proj.Services;
using Plugin.LocalNotifications;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971proj.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            
            StudentName.Text = "Welcome " + DatabaseServices.GetStudentName() +"!";
        }

        /// <summary>
        /// Sends Notifications for Course Start and Objectives 1&2 if boxes are checked and todays date
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            foreach (Course courseRecord in await DatabaseServices.GetCourse())
            {
                var notifyRandom = new Random();
                var notifyId = notifyRandom.Next(1000);
                if (courseRecord.AlertSE == true)
                {
                    if (courseRecord.StartDate.Date == DateTime.Today.Date)
                    {
                        CrossLocalNotifications.Current.Show("Notice", $"{courseRecord.Title} begins today!", notifyId);
                        notifyId = notifyRandom.Next(1000);
                    }
                    if (courseRecord.EndDate.Date == DateTime.Today.Date)
                    {
                        CrossLocalNotifications.Current.Show("Notice", $"{courseRecord.Title} ends today!", notifyId);
                        notifyId = notifyRandom.Next(1000);
                    }
                }
                if (courseRecord.AlertObj == true)
                {
                    if (courseRecord.Obj1_DueDate.Date == DateTime.Today.Date)
                    {
                        CrossLocalNotifications.Current.Show("Notice", $"{courseRecord.Title}'s Objective: {courseRecord.Obj1} begins today!", notifyId);
                        notifyId = notifyRandom.Next(1000);
                    }
                    if (courseRecord.Obj2_DueDate.Date == DateTime.Today.Date)
                    {
                        CrossLocalNotifications.Current.Show("Notice", $"{courseRecord.Title}'s Objective: {courseRecord.Obj2} begins today!", notifyId);
                    }
                }
            }

        }
        /// <summary>
        /// Sends user to TermPage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void ViewTerm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TermPage());
        }
        /// <summary>
        /// Sends user to Reports Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void Reports_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Report());
        }
        /// <summary>
        /// Sends user to AppSettings Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void Settings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AppSettings());
        }
        /// <summary>
        /// Logs user out and returns to login screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void Logout_Clicked(object sender, EventArgs e)
        {
            DatabaseServices.LoggedOut();
            await Navigation.PushAsync(new Login());
        }
    }
}