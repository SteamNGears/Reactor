using UnityEngine;
using System.Collections;

namespace Reactor
{
	[NodeMenu("Debug/Log Warning")]
	public class DebugLogWarningNode : BaseNode
	{

		public string message = "Enter message";

		public DebugLogWarningNode ()
		{
			this.NodeName = "DEBUG LOG";
		}

		void Start ()
		{
			Debug.LogWarning (this.message);
			this.End ();
		}

	}
}