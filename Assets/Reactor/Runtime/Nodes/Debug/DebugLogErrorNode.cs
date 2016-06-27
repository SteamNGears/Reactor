using UnityEngine;
using System.Collections;

namespace Reactor
{
	[NodeMenu("Debug/Log Error")]
	public class DebugLogErrorNode : BaseNode
	{
		public string message = "Enter message";

		public DebugLogErrorNode ()
		{
			this.NodeName = "DEBUG LOG";
		}

		void Start ()
		{
			Debug.LogError (this.message);
			this.End ();
		}
	}
}