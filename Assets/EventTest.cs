using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using Reactor;

public class EventTest : MonoBehaviour 
{
	public UnityEvent e;
	public int test;
	public AnimationCurve curve;

	public BaseNode node;
	// Use this for initialization
	void Start () {
//		this.node = this.gameObject.AddComponent<CallFunctionNode>();
//		this.node.hideFlags = HideFlags.HideInInspector;
//		this.hideFlags = HideFlags.HideInInspector;
#if MY_DEFINE
		Debug.Log("This is totally working!!!"); 
#endif

#if MY_OTHER_DEFINE
		Debug.Log("This is a test");
#endif
	}
}
