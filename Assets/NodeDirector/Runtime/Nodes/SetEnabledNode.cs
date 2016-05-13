using UnityEngine;
using System.Collections;
using Reactor;

public class SetEnabledNode :  BaseNode
{
	public GameObject target;
	public bool value = false;


	public override void Start ()
	{
		if(this.target == null)
			this.target = this.m_Sequence.m_GameObject;
		this.m_Sequence.m_GameObject.SetActive(value);
		this.End();
	}
	  
	public override void Update ()
	{

	}

}
