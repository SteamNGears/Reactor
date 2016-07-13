using UnityEngine;
using System.Collections;

namespace Reactor
{

	public class WaitForSwipe : BaseNode
	{
		public Collider[] m_TargetPoints;
		// Use this for initialization
		void Start ()
		{
			if(this.m_TargetPoints == null || this.m_TargetPoints.Length < 1)
			{
				Debug.Log("Not target points defined");
				this.End();
			}
		}
	
		// Update is called once per frame
		void Update ()
		{
			
		}
	}

}
