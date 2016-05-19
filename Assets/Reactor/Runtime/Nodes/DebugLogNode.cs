using UnityEngine;
using System.Collections;
using Reactor;

public class DebugLogNode : BaseNode
{
    public string message = "Enter message";

	public DebugLogNode()
	{
		this.NodeName = "DEBUG LOG";
	}

	void Start ()
	{
        Debug.Log(this.message);
		this.End();
	}
	
}
