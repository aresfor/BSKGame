#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NodeCanvas.Editor
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Class)]
    public class EnumLabelAttribute : PropertyAttribute
    {
        public string label;
        public EnumLabelAttribute(string label)
        {
            this.label = label;
        }
    }
    
    public class EnumExtension
    {
        public static object EX_EnumPopup(string title, Enum selected)
        {
            int index = 0;
            var array = Enum.GetValues(selected.GetType());
            int length = array.Length;
            int selectedIndex = 0;
            string[] enumString = new string[length];
            for (int i = 0; i < length; i++)
            {
                selectedIndex = (int)array.GetValue(i) == selected.GetHashCode() ? i : selectedIndex;
                FieldInfo[] fields = selected.GetType().GetFields();
                foreach (FieldInfo field in fields)
                {
                    if (field.Name.Equals(array.GetValue(i).ToString()))
                    {
                        object[] objs = field.GetCustomAttributes(typeof(EnumLabelAttribute), true);
                        if (objs != null && objs.Length > 0)
                        {
                            enumString[i] = ((EnumLabelAttribute)objs[0]).label;
                        }
                    }
                }
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(title);
            index = EditorGUILayout.Popup(selectedIndex, enumString);
            EditorGUILayout.EndHorizontal();
            return Enum.ToObject(selected.GetType(), array.GetValue(index));
        }


        public static object EX_EnumPopup(GUIContent title, Enum selected, params GUILayoutOption[] options)
        {
            int index = 0;
            var array = Enum.GetValues(selected.GetType());
            int length = array.Length;
            int selectedIndex = 0;
            string[] enumString = new string[length];
            for (int i = 0; i < length; i++)
            {
                selectedIndex = (int)array.GetValue(i) == selected.GetHashCode() ? i : selectedIndex;
                FieldInfo[] fields = selected.GetType().GetFields();
                foreach (FieldInfo field in fields)
                {
                    if (field.Name.Equals(array.GetValue(i).ToString()))
                    {
                        object[] objs = field.GetCustomAttributes(typeof(EnumLabelAttribute), true);
                        if (objs != null && objs.Length > 0)
                        {
                            enumString[i] = ((EnumLabelAttribute)objs[0]).label;
                            continue;
                        }
                        if (enumString[i] == null)
                        {
                            enumString[i] = field.Name;
                        }
                    }
                }
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(title);
            index = EditorGUILayout.Popup(selectedIndex, enumString, options);
            EditorGUILayout.EndHorizontal();
            return Enum.ToObject(selected.GetType(), array.GetValue(index));
        }
    }
}
#endif