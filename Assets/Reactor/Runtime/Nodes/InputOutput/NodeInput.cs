using UnityEngine;
using System.Collections;

namespace Reactor
{

	public enum InputType{SINGLE, MULTI};

	public class NodeInput
	{
		private BaseNode owner;
		public NodeInput (BaseNode _owner)
		{
			this.owner = _owner;
		}

		public void Begin()
		{
			if(this.owner.enabled == false)
				this.owner.enabled = true;
		}

		public void Connect(BaseNode n)
		{

		}
	}
}