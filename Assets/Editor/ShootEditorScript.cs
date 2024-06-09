using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime;

[CustomEditor(typeof(Shoot))]
public class ShootEditorScript : Editor
{
    private SerializedObject serializedGunSettings;
    private SerializedProperty gunType;

    private void OnEnable()
    {
        serializedGunSettings = new SerializedObject(target);
        gunType = serializedGunSettings.FindProperty("gunType");
    }

    public override void OnInspectorGUI()
    {
        serializedGunSettings.Update();

        Shoot gunSettings = (Shoot)target;

        SerializedProperty property = serializedGunSettings.GetIterator();
        property.NextVisible(true);

        while (property.NextVisible(false))
        {
            if (property.name == "pelletsPerShot" || property.name == "spreadAngle" || property.name == "range")
            {
                if (gunType.enumValueIndex == (int)Shoot.GunType.SHOTGUN)
                {
                    EditorGUILayout.PropertyField(property, true);
                }
            }
            else
            {
                EditorGUILayout.PropertyField(property, true);
            }
        }

        serializedGunSettings.ApplyModifiedProperties();
    }
}
