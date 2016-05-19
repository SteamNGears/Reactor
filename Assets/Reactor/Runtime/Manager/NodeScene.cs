using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Reactor
{
	/// <summary>
	/// A node director component that manages a graph of nodes 
	/// The node director allows nodes to be connected and executed in sequence or paralell
	/// </summary>
	[RequireComponent(typeof(StartNode))]
	public class NodeScene : MonoBehaviour
	{

		private int curID = 1;
		public SerializableDictionary nodes = new SerializableDictionary();



		/// <summary>
		/// Inits all of the Nodes
		/// </summary>
		void Start ()
		{
			foreach(int key in nodes.Keys)
			{
				nodes[key].enabled = false;
			}
			//set up start node
			nodes[0].enabled = true;
		}

		// Update is called once per frame
		void Update ()
		{

		}


		public void AddNode(System.Type nodeType, Vector2 position)
		{
			BaseNode node = (BaseNode)this.gameObject.AddComponent(nodeType);
			node.position = new Rect(position.x, position.y, 200,200);
			//node.hideFlags = HideFlags.HideInInspector;
			node.enabled = false;
			this.nodes[curID++] = node;
		}


		void Reset()
		{
			var start = this.gameObject.GetComponent<StartNode>();
			if(start == null)
				start = this.gameObject.AddComponent<StartNode>();
			nodes[0] = start;
		}





		[System.Serializable]
		public class SerializableDictionary : Dictionary<int, BaseNode>, ISerializationCallbackReceiver
		{
			[SerializeField]
			private List<int> keys = new List<int>();
			
			[SerializeField]
			private List<BaseNode> values = new List<BaseNode>();
			
			
			// save the dictionary to lists
			public void OnBeforeSerialize()
			{
//				Debug.Log("Serializing");
				
				keys.Clear();
				values.Clear();
				foreach(KeyValuePair<int, BaseNode> pair in this)
				{
					keys.Add(pair.Key);
					values.Add(pair.Value);
				}
				
//				Debug.Log("Serialization Complete");
			}
			
			// load dictionary from lists
			public void OnAfterDeserialize()
			{
//				Debug.Log("Deserializing");
				this.Clear();
				
				if(keys.Count != values.Count)
					throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));
				
				for(int i = 0; i < keys.Count; i++)
					this.Add(keys[i], values[i]);
//				Debug.Log("Deserialization Complete");
			}
		}

	
	}
}
