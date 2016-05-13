using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using Reactor;
public class EventTest : MonoBehaviour 
{

	public UnityEvent e;


	public BaseNode node;
	// Use this for initialization
	void Start () {
		this.node = this.gameObject.AddComponent<CallFunctionNode>();
		this.node.hideFlags = HideFlags.HideInInspector;
		this.hideFlags = HideFlags.HideInInspector;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
