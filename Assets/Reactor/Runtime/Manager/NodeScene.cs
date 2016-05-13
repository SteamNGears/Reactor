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
		public Dictionary<int, BaseNode> nodes = new Dictionary<int, BaseNode>();

		public void Awake()
		{
			var start = this.gameObject.GetComponent<StartNode>();
			if(start == null)
				start = this.gameObject.AddComponent<StartNode>();
			nodes[0] = start;
		}


		/// <summary>
		/// Inits all of the Nodes
		/// </summary>
		void Start ()
		{
			//get all attached nodes
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
			this.nodes[curID++] = node;
		}
	
	}
}
