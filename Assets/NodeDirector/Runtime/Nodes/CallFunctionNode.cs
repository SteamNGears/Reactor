using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using Reactor;

public class CallFunctionNode : BaseNode
{
	public GameObject target;
	public UnityEvent funciton;

	// Use this for initialization
	override public void Start () 
	{
		this.funciton.Invoke();
		this.End();
	}
	
	// Update is called once per frame
	override public void Update () 
	{
	
	}
}
