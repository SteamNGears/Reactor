using System;
using UnityEngine;

namespace Reactor
{
	public class StartNode: BaseNode
	{
		public StartNode ()
		{
		
		}

        public override void Update()
        {
            this.End();
        }
    }
}