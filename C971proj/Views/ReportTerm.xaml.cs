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
    public partial class ReportTerm : ContentPage
    {
        private readonly int _selectedTermId;
        public ReportTerm(Term term)
        {
            InitializeComponent();

            _selectedTermId = term.TermId;

            Title.Text = DatabaseServices.GetStudentName() + "'s Courses for " + term.TermName.ToString();
            TodayDate.Text = "Report Run on "+ DateTime.Now.ToString("MM-dd-yyyy hh:mm tt");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            CourseCollectionView.ItemsSource = await DatabaseServices.GetCourse(_selectedTermId);
        }
    }
}