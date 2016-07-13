using UnityEngine;
using System.Collections;


namespace Reactor
{
	/// <summary>
	/// Waits for a key to be pressed
	/// </summary>
	[NodeMenu("Input/Wait For Key")]
	public class WaitForKeyNode : BaseNode
	{
		public WaitForKeyNode()
		{
			this.NodeName = "Wait For Key";
		}

		/// <summary>
		/// The key to wait for
		/// </summary>
		public KeyCode m_Key;
	
		void Update ()
		{
			if(Input.GetKeyDown(m_Key))
				this.End();
		}
	}
}
