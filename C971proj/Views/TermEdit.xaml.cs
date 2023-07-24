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
    public partial class TermEdit : ContentPage
    {
        private readonly int _selectedTermId;
        private DateTime _selectedTermStart;
        private DateTime _selectedTermEnd;
        public TermEdit (Term term)
        {
            InitializeComponent();

            _selectedTermId = int.Parse(term.TermId.ToString());
            _selectedTermStart = term.TermStart.Date;
            _selectedTermEnd = term.TermEnd.Date;

            TermName.Text = term.TermName.ToString();
            TermStartDate.Date = term.TermStart.Date;
            TermEndDate.Date = term.TermEnd.Date;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            CourseCollectionView.ItemsSource = await DatabaseServices.GetCourse(_selectedTermId);
        }
        /// <summary>
        /// Saves new term data if all data is inputed correctly
        /// Error Checking: 
        ///   Term Name Field: Not Null
        ///   Term Date Field: Not Null, Not Before Current Date, Does not Conflict with Existing Term Dates
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
                if (term.TermId != _selectedTermId)
                {
                    if ((TermStartDate.Date <= term.TermEnd) && (term.TermStart <= TermEndDate.Date))
                    {
                        await DisplayAlert("Invalid: Term Date(s)", "Terms may not overlap with existing terms", "OK");
                        return;
                    }
                }
                
            }
            #endregion
            #region Error Checks No Conflicting Course Dates (start, end, obj1_date, obj2_date) with New Term Dates
            //todo: fix date changes
            if ((TermStartDate.Date != _selectedTermStart) || (TermEndDate.Date != _selectedTermEnd))
            {
                foreach (var course in await DatabaseServices.GetCourse(_selectedTermId))
                {
                    if ((course.StartDate.Date > TermEndDate.Date) || (TermStartDate.Date > course.StartDate.Date))
                    {
                        await DisplayAlert("Error: Course Dates", "Cannot Change Term Dates whose courses have dates outside of new Term Dates. Either delete courses or change dates", "OK");
                        return;
                    }
                    if ((course.EndDate.Date > TermEndDate.Date) || (TermStartDate.Date > course.EndDate.Date))
                    {
                        await DisplayAlert("Error: Course Dates", "Cannot Change Term Dates whose courses have dates outside of new Term Dates. Either delete courses or change dates", "OK");
                        return;
                    }
                    if ((course.Obj1_DueDate.Date > TermEndDate.Date) || (TermStartDate.Date > course.Obj1_DueDate.Date))
                    {
                        await DisplayAlert("Error: Objective Assessment Dates", "Cannot Change Term Dates whose courses have dates outside of new Term Dates. Either delete courses or change dates", "OK");
                        return;
                    }
                    if ((course.Obj2_DueDate.Date > TermEndDate.Date) || (TermStartDate.Date > course.Obj2_DueDate.Date))
                    {
                        await DisplayAlert("Error: Performance Assessment Dates", "Cannot Change Term Dates whose courses have dates outside of new Term Dates. Either delete courses or change dates", "OK");
                        return;
                    }
                }
            }
            #endregion

            await DatabaseServices.UpdateTerm(_selectedTermId, TermName.Text, TermStartDate.Date, TermEndDate.Date);
            await Navigation.PopAsync();
        }
        /// <summary>
        /// Confirms with user they want to Delete Term and All Associated Courses with that term, than deletes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void DeleteTerm_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Delete Term!", "Delete This Term and All Associated Courses?", "No", "Yes");

            if (answer == false)
            {
                await DatabaseServices.RemoveTerm(_selectedTermId);
                await DatabaseServices.RemoveCourseUsingTermId(_selectedTermId);

                await DisplayAlert("Deleted", "Term and All Associated Courses Deleted", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Canceled", "No Data Deleted", "OK");
            }
            
        }
        /// <summary>
        /// Checks (less than) 6 courses, than takes user to AddCourse Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void AddCourse_Clicked(object sender, EventArgs e)
        {
            int coursecount = 0;
            foreach (var course in await DatabaseServices.GetCourse(_selectedTermId))
            {
                coursecount++;
            }
            if (coursecount >= 6)
            {
                await DisplayAlert("Cannot Add Course", "Terms have a 6 course maximum. If you would like to add another course, then remove one from this term.", "OK");
            }
            else
            {
                await Navigation.PushAsync(new CourseAdd(_selectedTermId, TermStartDate.Date, TermEndDate.Date));
            }         
        }
        /// <summary>
        /// On Course Click, Sends User to CourseEdit Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void CourseCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var course = (Course)e.CurrentSelection.FirstOrDefault();
            if (e.CurrentSelection != null)
            {
                await Navigation.PushAsync(new CourseEdit(course));
            }
        }
        /// <summary>
        /// Searchs the terms courses by title
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void CourseSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var courses = await DatabaseServices.GetCourse(_selectedTermId);
            CourseCollectionView.ItemsSource = courses.Where(a => a.Title.StartsWith(e.NewTextValue));
        }
    }
}