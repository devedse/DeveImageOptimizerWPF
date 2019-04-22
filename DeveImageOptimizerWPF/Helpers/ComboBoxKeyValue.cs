namespace DeveImageOptimizerWPF.Helpers
{
    public class ComboBoxKeyValue<T>
    {
        public string Key { get; set; }
        public T Value { get; set; }

        public ComboBoxKeyValue(string key, T value)
        {
            Key = key;
            Value = value;
        }
    }
}
