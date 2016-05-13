using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Reactor
{
	/// <summary>
	/// The editor display for the Node Director
	/// </summary>
	[CustomEditor (typeof(NodeScene))]
	public class EditorNodeDirector : Editor
    { 
        
		/// <summary>
		/// Raises the inspector GUI event.
		/// </summary>
		public override void OnInspectorGUI ()
		{
            var seq = ((NodeScene)this.target).sequence;
            ((NodeScene)this.target).sequence = (NodeSequence)EditorGUILayout.ObjectField (seq, typeof(NodeSequence), false, null);
            if (seq != null)
            {
                if (GUILayout.Button("Open Sequence"))
                {
                    //Display the sequence editor
                    NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
                    editor.sequence = ((NodeScene)this.target).sequence;
                    editor.Init(AssetDatabase.GetAssetPath(seq));
                }
            }
            else
            {
                if (GUILayout.Button("Create New Sequence"))
                {
                    //Display the sequence editor
                    NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
                    //editor.sequence = this.seq;
                    editor.Init("new_sequence");
                }

            }
            
		}

	}

}