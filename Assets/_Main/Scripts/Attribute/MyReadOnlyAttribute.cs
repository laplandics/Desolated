using UnityEngine;
using UnityEditor;

namespace MyAttributes
{
    public class MyReadOnlyAttribute : PropertyAttribute { }

#if UNITY_EDITOR
    
    [CustomPropertyDrawer(typeof(MyReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (IsArrayLike(property)) return GetArrayHeight(property);
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            if (IsArrayLike(property)) DrawArray(position, property, label);
            else DrawNormal(position, property, label);
            EditorGUI.EndProperty();
        }

        private static bool IsArrayLike(SerializedProperty property)
        { return property.isArray && property.propertyType != SerializedPropertyType.String; }

        private static void DrawNormal(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.DisabledScope(true))
            { EditorGUI.PropertyField(position, property, label, true); }
        }

        private static float GetArrayHeight(SerializedProperty property)
        {
            var height = EditorGUIUtility.singleLineHeight;
            if (!property.isExpanded) return height;
            height += EditorGUIUtility.standardVerticalSpacing;
            height += EditorGUIUtility.singleLineHeight;
            for (var i = 0; i < property.arraySize; i++)
            {
                var element = property.GetArrayElementAtIndex(i);
                height += EditorGUIUtility.standardVerticalSpacing;
                height += EditorGUI.GetPropertyHeight(element, true);
            }

            return height;
        }

        private static void DrawArray(Rect position, SerializedProperty property, GUIContent label)
        {
            var headerRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(headerRect, property.isExpanded, label, true);
            if (!property.isExpanded) return;

            EditorGUI.indentLevel++;
            var y = headerRect.yMax + EditorGUIUtility.standardVerticalSpacing;
            var sizeRect = new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(sizeRect, "Size", property.arraySize.ToString());
            y = sizeRect.yMax + EditorGUIUtility.standardVerticalSpacing;

            using (new EditorGUI.DisabledScope(true))
            {
                for (var i = 0; i < property.arraySize; i++)
                {
                    var element = property.GetArrayElementAtIndex(i);
                    var elementHeight = EditorGUI.GetPropertyHeight(element, true);

                    var elementRect = new Rect(position.x, y, position.width, elementHeight);
                    EditorGUI.PropertyField(elementRect, element, new GUIContent($"Element {i}"), true);

                    y += elementHeight + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            EditorGUI.indentLevel--;
        }
    }
    #endif
}
