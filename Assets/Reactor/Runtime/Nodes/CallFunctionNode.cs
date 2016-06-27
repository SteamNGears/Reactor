using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using Reactor;

public class CallFunctionNode : BaseNode
{
	public UnityEvent function = new UnityEvent();

	// Use this for initialization
	void Start () 
	{
		if(this.function != null)
			this.function.Invoke();
		this.End();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
