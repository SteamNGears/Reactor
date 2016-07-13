using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Reactor
{
	[CustomEditor (typeof(CallFunctionNode))]
	public class CallFunctionEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			CallFunctionNode node = (CallFunctionNode)target;
			SerializedProperty prop = serializedObject.FindProperty("function"); 

			EditorGUIUtility.LookLikeControls();
			EditorGUILayout.PropertyField(prop);//<--- NullReferenceException: SerializedObject of SerializedProperty has been Disposed.

			if(GUI.changed)
			{
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}