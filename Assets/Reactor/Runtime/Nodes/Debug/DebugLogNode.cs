using UnityEngine;
using System.Collections;

namespace Reactor
{
	[NodeMenu("Debug/Log Message")]
	public class DebugLogNode : BaseNode
	{
		public string message = "Enter message";

		public DebugLogNode ()
		{
			this.NodeName = "DEBUG LOG";
		}

		void Start ()
		{
			Debug.Log (this.message);
			this.End ();
		}
	
	}
}
