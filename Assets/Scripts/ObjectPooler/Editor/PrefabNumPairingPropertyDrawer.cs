using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(PrefabNumPairing))]
public class PrefabNumPairingPropertyDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
        label = EditorGUI.BeginProperty(position, label, property);
        Rect content = position;
        content.width *= .5f;

        EditorGUIUtility.labelWidth = 60;
        EditorGUI.PropertyField(content, property.FindPropertyRelative("Prefab"));

        content.x += content.width;
        EditorGUIUtility.labelWidth = 100;
        EditorGUI.PropertyField(content, property.FindPropertyRelative("InitPoolSize"));

        EditorGUI.EndProperty();
    }

}
