using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using Reactor;

public class CallFunctionNode : BaseNode
{
	public GameObject target;
	public UnityEvent funciton;

	// Use this for initialization
	void Start () 
	{
		if(this.funciton != null)
			this.funciton.Invoke();
		this.End();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
