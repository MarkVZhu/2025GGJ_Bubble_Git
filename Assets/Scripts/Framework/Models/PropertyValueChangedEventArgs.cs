public class PropertyValueChangedEventArgs
{
    public string PropertyName;
    public object Value;

    public PropertyValueChangedEventArgs(string propertyName, object value)
    {
        PropertyName = propertyName;
        Value = value;
    }
}