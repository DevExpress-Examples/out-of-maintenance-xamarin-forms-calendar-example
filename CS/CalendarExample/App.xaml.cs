using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CalendarExample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new CalendarView();
        }
    }
}
