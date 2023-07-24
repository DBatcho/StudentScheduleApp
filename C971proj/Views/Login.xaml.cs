using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C971proj.Models;
using C971proj.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971proj.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Checks the username/password combination against the database:: True: login, False: Error Message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void Submit_Clicked(object sender, EventArgs e)
        {
            bool loggedin = false;
            foreach (Student student in await DatabaseServices.GetStudent())
            {
                if ((student.UserName == Username.Text) && (student.Password == Password.Text))
                {
                    loggedin = true;
                    DatabaseServices.LoggedIn(student.StudentId, student.StudentName);
                    await Navigation.PushAsync(new HomePage());
                }
            }
            if (loggedin == false)
            {
                await DisplayAlert("Error: Username/Password", "Username or Password Incorrect", "OK");
                return;
            }
        }
    }
}