using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectHolder))]
public class ObjectHolderEditor : Editor
{

    public override void OnInspectorGUI()
    {
        
        DrawDefaultInspector();

        ObjectHolder objectHolder = target as ObjectHolder;

        if (GUILayout.Button("Arrange List"))
        {
            objectHolder.ArrangeList();
        }

        serializedObject.ApplyModifiedProperties();
    }


}
