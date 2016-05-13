using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Reactor
{
    [CreateAssetMenu]
	public class NodeSequence : ScriptableObject
	{
		[HideInInspector]
		public GameObject m_GameObject;

		[Serializable]
		public class SerializableDictionary : Dictionary<int, BaseNode>, ISerializationCallbackReceiver
		{
			[SerializeField]
			private List<int> keys = new List<int>();

			[SerializeField]
			private List<BaseNode> values = new List<BaseNode>();


			// save the dictionary to lists
			public void OnBeforeSerialize()
			{
				Debug.Log("Serializing");

				keys.Clear();
				values.Clear();
				foreach(KeyValuePair<int, BaseNode> pair in this)
				{
					keys.Add(pair.Key);
					values.Add(pair.Value);
				}

				Debug.Log("Serialization Complete");
			}

			// load dictionary from lists
			public void OnAfterDeserialize()
			{
				Debug.Log("Deserializing");
				this.Clear();

				if(keys.Count != values.Count)
					throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

				for(int i = 0; i < keys.Count; i++)
					this.Add(keys[i], values[i]);
				Debug.Log("Deserialization Complete");
			}
		}

		[SerializeField]
		public SerializableDictionary nodes;


		public BaseNode startNode
		{
			get{return this.nodes.ContainsKey(0) ? this.nodes[0]:null;}
			private set{this.nodes[0] = value;}
		}

		public LinkedList<BaseNode> curNodes;
		private int id = 0;

		/// <summary>
		/// Start the sequence.
		/// </summary>
		public void Init ()
		{
			if(this.nodes == null)
			{
				this.nodes = new SerializableDictionary();
			}
			if(this.startNode == null)
			{
                startNode = new StartNode();//ScriptableObject.CreateInstance<StartNode>();
				startNode.position = new Rect(10,10,200,100);
				this.AddNode(startNode);
			}
			curNodes = new LinkedList<BaseNode> ();
			curNodes.AddLast (startNode);

			startNode.Init();
		}

		/// <summary>
		/// Update the sequence
		/// </summary>
		public void Update ()
		{
//            Debug.Log("Scene updating");
			LinkedList<BaseNode> addList = new LinkedList<BaseNode>();
			LinkedList<BaseNode> removeList = new LinkedList<BaseNode>();
			foreach (BaseNode b in curNodes) {
				if (b.Active)
					b.Update ();
				else {
					b.End ();
//                    Debug.Log("node:" + b.name + "has ended");
					foreach (BaseNode n in b.next) {
						addList.AddLast(n);
					}
					removeList.AddLast(b);
				}
			}

			foreach(BaseNode rem in removeList)
				curNodes.Remove(rem);

			foreach(BaseNode add in addList)
			{
				curNodes.AddLast(add);
				add.Init();
			}
			
		}

		/// <summary>
		/// Adds a node to the sequence, but does not require it to be connected to another node
		/// </summary>
		/// <param name="b">The blue component.</param>
		public void AddNode(BaseNode b)
		{
			b.m_Sequence = this;
			this.nodes[id++] = b;
		}

		/// <summary>
		/// Resets this instance.
		/// </summary>
		public void Reset()
		{
			foreach(int key in this.nodes.Keys)
			{
				this.nodes[key].Active = false;
			}
			this.startNode.Active = true;

		}
	}
}