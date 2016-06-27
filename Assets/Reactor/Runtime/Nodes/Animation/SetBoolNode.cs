﻿using UnityEngine;
using System.Collections;

namespace Reactor
{
	/// <summary>
	/// Handles setting a boolean animator property
	/// </summary>
	[NodeMenu("Animator/Set Boolean")]
	public class SetBoolNode : BaseNode
	{
		/// <summary>
		/// The animator to target(attached component by default)
		/// </summary>
		public Animator m_Animator;
		public string m_Property = "";
		public bool m_Value = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="Reactor.SetBoolNode"/> class.
		/// </summary>
		public SetBoolNode()
		{
			this.NodeName = "Set Boolean Property";
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
			this.m_Animator.SetBool(m_Property, m_Value);
				
		}
	}
}
