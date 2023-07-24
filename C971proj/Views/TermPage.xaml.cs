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
    public partial class TermPage : ContentPage
    {
        public TermPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            TermCollectionView.ItemsSource = await DatabaseServices.GetTerm();
        }
        /// <summary>
        /// Takes user to the Add Term Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void AddTerm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TermAdd());
        }
        /// <summary>
        /// Informs user how to edit a class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditTerm_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Term Edit", "Select a Term to Edit", "OK");
        }
        /// <summary>
        /// Takes user to Term's Course page upon Selecting a Term
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void TermCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                Term term = (Term)e.CurrentSelection.FirstOrDefault();
                await Navigation.PushAsync(new TermEdit(term));
            }
        }

    }
}