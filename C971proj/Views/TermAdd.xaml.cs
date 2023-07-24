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
    public partial class TermAdd : ContentPage
    {
        public TermAdd()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Saves Term
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void SaveTerm_Clicked(object sender, EventArgs e)
        {
            #region Term Input Error Checking
            if (string.IsNullOrWhiteSpace(TermName.Text))
            {
                await DisplayAlert("Invalid: Term Name", "Please Enter Term Name", "OK");
                return;
            }
            if (TermStartDate.Date < DateTime.Today)
            {
                await DisplayAlert("Invalid: Term Start Date", "Term Start Date MUST be after Current Date", "OK");
                return;
            }
            if (TermStartDate.Date > TermEndDate.Date)
            {
                await DisplayAlert("Error: Term Dates", "Term End Date may not be before Term Start Date", "OK");
                return;
            }
            foreach (var term in await DatabaseServices.GetTerm())
            {
                if ((TermStartDate.Date <= term.TermEnd) && (term.TermStart <= TermEndDate.Date))
                {
                    await DisplayAlert("Invalid: Term Date(s)", "Terms may not overlap with existing terms", "OK");
                    return;
                }
            }
            #endregion

            await DatabaseServices.AddTerm(TermName.Text, DatabaseServices.GetLogInId(), TermStartDate.Date, TermEndDate.Date);
            await Navigation.PopAsync();
        }
    }
}