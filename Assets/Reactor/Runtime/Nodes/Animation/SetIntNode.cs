using UnityEngine;
using System.Collections;

namespace Reactor
{
	[NodeMenu("Animator/Set Integer")]
	public class SetIntNode : BaseNode 
	{

		/// <summary>
		/// The animator to target(attached component by default)
		/// </summary>
		public Animator m_Animator;
		public string m_Property = "";
		public int m_Value = 0;

		/// <summary>
		/// Initializes a new instance of the <see cref="Reactor.SetBoolNode"/> class.
		/// </summary>
		public SetIntNode()
		{
			this.NodeName = "Set Int Property";
		}

		/// <summary>
		/// Sets the correct animator
		/// </summary>
		void Awake()
		{
			this.m_Animator = this.m_Animator ?? GetComponent<Animator>();
		}

		/// <summary>
		/// Sets the property then ends
		/// </summary>
		void Start ()
		{

			if(this.m_Animator == null)
			{
				Debug.Log("Could not find animator");
				this.End();
			}
			this.m_Animator.SetInteger(m_Property, m_Value);

		}
	}
}