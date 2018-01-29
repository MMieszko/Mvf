namespace FormsMvvm.Converters
{
    public abstract class MvfValueConverter
    {
        public abstract object Convert(object value);

        public virtual object ConvertBack(object value) => value;
    }
}
