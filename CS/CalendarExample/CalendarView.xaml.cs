using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XamarinForms.Editors;
using Xamarin.Forms;

namespace CalendarExample
{
    public partial class CalendarView : ContentPage
    {
        public static readonly BindablePropertyKey OrientationPropertyKey = BindableProperty.CreateReadOnly("Orientation", typeof(StackOrientation), typeof(CalendarView), StackOrientation.Vertical);
        public static readonly BindableProperty OrientationProperty = OrientationPropertyKey.BindableProperty;
        public StackOrientation Orientation => (StackOrientation)GetValue(OrientationProperty);

        public CalendarView()
        {
            DevExpress.XamarinForms.Editors.Initializer.Init();
            InitializeComponent();
            ViewModel = new CalendarViewModel();
            BindingContext = ViewModel;
        }

        CalendarViewModel ViewModel { get; }

        void CustomDayCellStyle(object sender, CustomSelectableCellStyleEventArgs e)
        {
            if (e.Date == DateTime.Today)
                return;

            if (ViewModel.SelectedDate != null && e.Date == ViewModel.SelectedDate.Value)
                return;

            SpecialDate specialDate = ViewModel.TryFindSpecialDate(e.Date);
            if (specialDate == null)
                return;

            e.FontAttributes = FontAttributes.Bold;
            Color textColor;
            if (specialDate.IsHoliday)
            {
                textColor = Color.FromHex("F44848");
                e.EllipseBackgroundColor = Color.FromRgba(textColor.R, textColor.G, textColor.B, 0.25);
                e.TextColor = textColor;

                return;
            }
            textColor = Color.FromHex("55575C");
            e.EllipseBackgroundColor = Color.FromRgba(textColor.R, textColor.G, textColor.B, 0.15);
            e.TextColor = textColor;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            SetValue(OrientationPropertyKey, width > height ? StackOrientation.Horizontal : StackOrientation.Vertical);
        }
    }
}
