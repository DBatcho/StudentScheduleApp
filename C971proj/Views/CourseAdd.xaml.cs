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
    public partial class CourseAdd : ContentPage
    {
        private readonly int _selectedTermId;
        public CourseAdd(int termid, DateTime termstart, DateTime termend)
        {
            InitializeComponent();
            _selectedTermId = termid;

            StartDate.Date = termstart.Date;
            EndDate.Date = termend.Date;

            Obj1_DueDate.Date = termstart.Date;
            Obj2_DueDate.Date = termstart.Date;
        }
        /// <summary>
        /// Error Checks All Course Fields and Saves if Correct
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void SaveCourse_Clicked(object sender, EventArgs e)
        {
            #region Course Input Error Checking
            int count = 0;
            bool stringcorrect = true;

            if (string.IsNullOrWhiteSpace(Title.Text))
            {
                await DisplayAlert("Error: Course Title", "Please Enter a Title", "OK");
                return;
            }
            if (StartDate.Date > EndDate.Date)
            {
                await DisplayAlert("Error: Course Start End/Date", "Course Start Date must be before Course End Date", "OK");
                return;
            }
            foreach (var term in await DatabaseServices.GetTerm())
            {
                if (term.TermId == _selectedTermId)
                {
                    if ((StartDate.Date > term.TermEnd.Date) || (term.TermStart.Date > StartDate.Date))
                    {
                        await DisplayAlert("Error: Start Date", "Course Start Date must fall in Term Start/End Date", "OK");
                        return;
                    }
                    if ((EndDate.Date > term.TermEnd.Date) || (term.TermStart.Date > EndDate.Date))
                    {
                        await DisplayAlert("Error: End Date", "Course End Date must fall in Term Start/End Date", "OK");
                        return;
                    }
                }
            }
            if (string.IsNullOrWhiteSpace(Obj1.Text))
            {
                await DisplayAlert("Error: Objective 1", "Please Enter an Objective for Objective 1", "OK");
                return;
            }
            if (DateTime.Compare(Obj1_DueDate.Date, StartDate.Date) < 0)
            {
                await DisplayAlert("Error: Course Objective Assessment Start Date", "Please Enter a Course Objective Assessment Start Date Later then Course Start Date", "OK");
                return;
            }
            if (DateTime.Compare(Obj1_DueDate.Date, EndDate.Date) > 0)
            {
                await DisplayAlert("Error: Course Objective Assessment End Date", "Please Enter a Course Objective Assessment Date Earlier then Course End Date", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(Obj2.Text))
            {
                await DisplayAlert("Error: Objective 2", "Please Enter an Objective for Objective 2", "OK");
                return;
            }
            if (DateTime.Compare(Obj2_DueDate.Date, StartDate.Date) < 0)
            {
                await DisplayAlert("Error: Course Performance Assessment Start Date", "Please Enter a Perfromance Assessment Start Date Later then Course Start Date", "OK");
                return;
            }
            if (DateTime.Compare(Obj2_DueDate.Date, EndDate.Date) > 0)
            {
                await DisplayAlert("Error: Course Performance Assessment End Date", "Please Enter a Performance Assessment End Date Earlier then Course End Date", "OK");
                return;
            }
            if (Status.SelectedIndex == -1)
            {
                await DisplayAlert("Error: Course Status", "Please select a Course Status", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(CI_Name.Text))
            {
                await DisplayAlert("Error: Course Instructor Name", "Please Enter a Course Instructor Name", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(CI_Phone.Text))
            {
                await DisplayAlert("Error: Course Instructor Phone Number", "Please Enter a Course Instructor Phone Number", "OK");
                return;
            }
            foreach (var check in CI_Phone.Text)
            {
                count++;
                if (!char.IsDigit(check))
                {
                    stringcorrect = false;
                    break;
                }
            }
            if ((count > 10) || (count < 10))
            {
                await DisplayAlert("Error: Course Instructor Phone Number", "Please Enter a Valid Course Instructor Phone Number. Must be 10 numbers only. No symbols, letters, or spaces.", "OK");
                return;
            }
            if (stringcorrect == false)
            {
                await DisplayAlert("Error: Course Instructor Phone Number", "Please Enter a Valid Course Instructor Phone Number. Must be 10 numbers only. No symbols, letters, or spaces.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(CI_Email.Text))
            {
                await DisplayAlert("Error: Course Instructor Email", "Please Enter Course Instructor Email", "OK");
                return;
            }
            stringcorrect = true;
            if (!CI_Email.Text.Contains("@"))
            {
                stringcorrect = false;
            }
            if ((!CI_Email.Text.Contains(".com")) &&
                (!CI_Email.Text.Contains(".gov")) &&
                (!CI_Email.Text.Contains(".edu")) &&
                (!CI_Email.Text.Contains(".org")))
            {
                stringcorrect = false;
            }
            if (stringcorrect == false)
            {
                await DisplayAlert("Error: Course Instructor Email", "A Course Instructor Email must contain an @ and a Domain. Please Enter a Valid Course Instructor Email", "OK");
                return;
            }
            #endregion

            await DatabaseServices.AddCourse(_selectedTermId, Title.Text, StartDate.Date, EndDate.Date,
                Obj1.Text, Obj1_DueDate.Date,
                Obj2.Text, Obj2_DueDate.Date,
                Status.SelectedItem.ToString(), AlertSE.IsChecked, AlertObj.IsChecked, OptionalNotes.Text,
                CI_Name.Text, CI_Phone.Text, CI_Email.Text);
            await Navigation.PopAsync();
        }
    }
}