using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C971proj.Services;
using C971proj.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971proj.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportTermClass : ContentPage
    {
        public ReportTermClass()
        {
            InitializeComponent();
                                    
        }
        protected override async void OnAppearing()
        {
            TermChoice.Items.Clear();
            foreach (var term in await DatabaseServices.GetTerm())
            {
                TermChoice.Items.Add(term.TermName);
            }
        }
        /// <summary>
        /// Upon Clicking, Error Checks if a terms was selected, if one was opens the ReportTerm page with the selected term
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void SubmitTerm_Clicked(object sender, EventArgs e)
        {
            if (TermChoice.SelectedIndex == -1)
            {
                await DisplayAlert("Error: Choose Term", "Please select a Term", "OK");
                return;
            }
            else
            {
                foreach (Term term in await DatabaseServices.GetTerm())
                {
                    if (term.TermName == TermChoice.SelectedItem.ToString())
                    {
                        await Navigation.PushAsync(new ReportTerm(term));
                    }
                }
            }
            
        }
    }
}