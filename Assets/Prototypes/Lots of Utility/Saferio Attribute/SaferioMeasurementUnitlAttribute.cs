using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SaferioMeasurementUnitAttribute : PropertyAttribute
{
    public string unit;

    public SaferioMeasurementUnitAttribute(string unit)
    {
        this.unit = unit;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SaferioMeasurementUnitAttribute))]
public class UnitLabelPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SaferioMeasurementUnitAttribute unitLabelAttribute = (SaferioMeasurementUnitAttribute)attribute;

        EditorGUI.LabelField(position, label);

        position.xMin += EditorGUIUtility.labelWidth;

        if (property.propertyType == SerializedPropertyType.Float || property.propertyType == SerializedPropertyType.Integer)
        {
            float newValue = property.propertyType == SerializedPropertyType.Float
                ? property.floatValue
                : property.intValue;

            newValue = EditorGUI.FloatField(position, newValue);

            property.floatValue = newValue;
            EditorGUI.LabelField(new Rect(position.xMax - 100, position.y, 100, position.height), unitLabelAttribute.unit);
        }
        else
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}
#endif
