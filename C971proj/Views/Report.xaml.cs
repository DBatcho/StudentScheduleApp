using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971proj.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Report : ContentPage
    {
        public Report()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Sends user to ReportTermClass Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void TermsClasses_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReportTermClass());
        }
    }
}