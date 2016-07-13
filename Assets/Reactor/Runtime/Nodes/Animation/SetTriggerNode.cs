using UnityEngine;
using System.Collections;


namespace Reactor
{
	[NodeMenu("Animator/Set Boolean")]
	public class SetTriggerNode : BaseNode
	{

		/// <summary>
		/// The animator to target(attached component by default)
		/// </summary>
		public Animator m_Animator;
		public string m_TriggerName = "";

		/// <summary>
		/// Initializes a new instance of the <see cref="Reactor.SetBoolNode"/> class.
		/// </summary>
		public SetTriggerNode ()
		{
			this.NodeName = "Set Trigger Property";
		}

		/// <summary>
		/// Sets the correct animator
		/// </summary>
		void Awake ()
		{
			this.m_Animator = this.m_Animator ?? GetComponent<Animator> ();
		}

		/// <summary>
		/// Sets the property then ends
		/// </summary>
		void Start ()
		{

			if (this.m_Animator == null) {
				Debug.Log ("Could not find animator");
				this.End ();
			}
			this.m_Animator.SetTrigger (m_TriggerName);

		}
	}
}
