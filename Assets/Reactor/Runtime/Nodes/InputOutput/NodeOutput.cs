using UnityEngine;
using System.Collections;

namespace Reactor
{
	public class NodeOutput
	{
		protected BaseNode[] target;

		public void Connect(BaseNode n)
		{

			if(this.target == null)
			{
				this.target = new BaseNode[]{n};
			}
			BaseNode[] newList = new BaseNode[this.target.Length + 1];
			int i = 0;
			for(i = 0; i < this.target.Length; i++)
				newList[i] = this.target[i];
			newList[i] = n;
			this.target = newList;
		}

		public void Begin()
		{
			foreach(BaseNode n in this.target)
			{
				n.enabled = true;
			}
		}

	}
}