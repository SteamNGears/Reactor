using UnityEngine;
using System.Collections;
using Reactor;

public class SetEnabledNode :  BaseNode
{
	public GameObject target;
	public bool value = false;


	void Start ()
	{
		if(this.target == null)
			this.target = this.gameObject;
		this.gameObject.SetActive(value);
		this.End();
	}
	  
	void Update ()
	{

	}

}
