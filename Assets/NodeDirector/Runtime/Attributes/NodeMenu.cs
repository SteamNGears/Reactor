using UnityEngine;
using System.Collections;
using System;

[AttributeUsage(AttributeTargets.Class)]
public class NodeMenu : Attribute
{
	public string path;
	public NodeMenu(string path)
	{
		this.path = path;
	}
}
