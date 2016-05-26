using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Reactor
{
	/// <summary>
	/// The editor display for the Node Director
	/// </summary>
	[CustomEditor (typeof(ReactorSequence))]
	public class EditorNodeDirector : Editor
	{ 
        
		/// <summary>
		/// Raises the inspector GUI event.
		/// </summary>
		public override void OnInspectorGUI ()
		{
			if (GUILayout.Button ("Open Sequence")) {
				Debug.Log ("Opening Sequence");
				NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
				editor.Init(Selection.activeGameObject);
			}
            
		}

	}

}