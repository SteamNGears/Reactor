using UnityEngine;
using System.Collections;

namespace Reactor
{
	/// <summary>
	/// A node director component that manages a graph of nodes 
	/// The node director allows nodes to be connected and executed in sequence or paralell
	/// </summary>
	public class NodeScene : MonoBehaviour
	{

		public NodeSequence sequence;

		/// <summary>
		/// Inits all of the Nodes
		/// </summary>
		void Start ()
		{
			Debug.Log("Initializing sequence");
			if(this.sequence != null)
			{
				this.sequence.m_GameObject = this.gameObject;
				sequence.Init();

			}
			Debug.Log("Initialization complete");
		}

		
	
		// Update is called once per frame
		void Update ()
		{
			if(this.sequence != null)
				this.sequence.Update();
		}

		/// <summary>
		/// Raises the disable event.
		/// </summary>
		void OnDisable()
		{
			if(this.sequence != null)
				this.sequence.Reset();
		}

		public void DoSomething(){}
	}
}
