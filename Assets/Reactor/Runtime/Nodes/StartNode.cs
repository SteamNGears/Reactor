using System;
using UnityEngine;

namespace Reactor
{
	public class StartNode: BaseNode
	{
		public StartNode ()
		{
			this.NodeName = "START";
		}

        void Update()
        {
            this.End();
        }
    }
}