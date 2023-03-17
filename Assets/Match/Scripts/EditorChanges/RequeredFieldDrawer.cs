#if (UNITY_EDITOR) 
using UnityEditor;
using UnityEngine;

namespace Assets.Match.Scripts.EditorChanges
{

    [CustomPropertyDrawer(typeof(RequiredField))]

    public class RequeredFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            RequiredField field = attribute as RequiredField;

            if (property.objectReferenceValue == null)
            {
                GUI.color = field.color;
                EditorGUI.PropertyField(position, property, label);
                GUI.color = Color.white;
            }
            else
                EditorGUI.PropertyField(position, property, label);
        }
    }
}
#endif
