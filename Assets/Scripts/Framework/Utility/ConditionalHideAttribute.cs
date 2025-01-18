using UnityEngine;

public class ConditionalHideAttribute : PropertyAttribute
{
    public string conditionField; // 条件字段名称
    public bool hideOnFalse;      // 条件字段为 false 时是否隐藏

    public ConditionalHideAttribute(string conditionField, bool hideOnFalse = true)
    {
        this.conditionField = conditionField;
        this.hideOnFalse = hideOnFalse;
    }
}
