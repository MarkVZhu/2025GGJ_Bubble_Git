using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
public class ConditionalHideDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute conditionalHide = (ConditionalHideAttribute)attribute;

        // 获取条件字段的值
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(conditionalHide.conditionField);

        if (conditionProperty != null && conditionProperty.propertyType == SerializedPropertyType.Boolean)
        {
            bool shouldHide = conditionalHide.hideOnFalse ? !conditionProperty.boolValue : conditionProperty.boolValue;

            // 如果条件满足，不绘制字段
            if (!shouldHide)
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
        else
        {
            Debug.LogError($"ConditionalHide: Cannot find bool field {conditionalHide.conditionField}");
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute conditionalHide = (ConditionalHideAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(conditionalHide.conditionField);

        if (conditionProperty != null && conditionProperty.propertyType == SerializedPropertyType.Boolean)
        {
            bool shouldHide = conditionalHide.hideOnFalse ? !conditionProperty.boolValue : conditionProperty.boolValue;

            // 如果需要隐藏，返回高度为0
            return shouldHide ? 0f : EditorGUI.GetPropertyHeight(property, label);
        }

        return EditorGUI.GetPropertyHeight(property, label);
    }
}
