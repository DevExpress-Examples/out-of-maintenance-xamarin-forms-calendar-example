using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DevExpress.XamarinForms.Editors;

namespace CalendarExample
{
    public class NotificationObject : INotifyPropertyChanged
    {
        protected bool SetProperty<T>(ref T backingStore, T value, Action onChanged = null, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        protected bool SetProperty<T>(ref T backingStore, T value, Action<T, T> onChanged, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;
            T oldValue = backingStore;
            backingStore = value;
            onChanged?.Invoke(oldValue, value);
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class CalendarViewModel : NotificationObject
    {
        DateTime displayDate;
        DateTime? selectedDate;
        DXCalendarViewType activeViewType;
        bool isHolidaysAndObservancesListVisible;
        IEnumerable<SpecialDate> specialDates;

        public CalendarViewModel()
        {
            DisplayDate = DateTime.Today;
            UpdateHolidaysAndObservancesListVisible();
        }

        public IEnumerable<SpecialDate> SpecialDates
        {
            get => this.specialDates;
            set => SetProperty(ref this.specialDates, value);
        }

        public DateTime DisplayDate
        {
            get => this.displayDate;
            set => SetProperty(ref this.displayDate, value, () => {
                UpdateCurrentCalendarIfNeeded();
                SpecialDates = USCalendar.GetSpecialDatesForMonth(DisplayDate.Month);
            });
        }

        public DateTime? SelectedDate
        {
            get => this.selectedDate;
            set => SetProperty(ref this.selectedDate, value);
        }

        public DXCalendarViewType ActiveViewType
        {
            get => this.activeViewType;
            set => SetProperty(ref this.activeViewType, value, UpdateHolidaysAndObservancesListVisible);
        }

        public bool IsHolidaysAndObservancesListVisible
        {
            get => this.isHolidaysAndObservancesListVisible;
            set => SetProperty(ref this.isHolidaysAndObservancesListVisible, value);
        }

        USCalendar USCalendar { get; set; }

        public SpecialDate TryFindSpecialDate(DateTime date)
        {
            return SpecialDates.FirstOrDefault(x => x.Date == date);
        }

        void UpdateHolidaysAndObservancesListVisible()
        {
            IsHolidaysAndObservancesListVisible = ActiveViewType == DXCalendarViewType.Day;
        }

        void UpdateCurrentCalendarIfNeeded()
        {
            if (USCalendar == null || USCalendar.Year != DisplayDate.Year)
                USCalendar = new USCalendar(DisplayDate.Year);
        }
    }
}
