using System;
using UnityEngine;

namespace Reactor
{
	public class StartNode: BaseNode
	{
		public StartNode ()
		{
			this.name = "START";
		}

        public override void Update()
        {
            this.End();
        }
    }
}