using UnityEngine;
using System.Collections;
using Reactor;



public class PlayAnimationNode : BaseNode {

	public string AnimationName = "Door_Open";
	public Animator anim;

	// Use this for initialization
	void Start () {
		anim = this.gameObject.GetComponent<Animator>();
		if(anim == null)
		{
			Debug.Log("No animator attached to gameobject!!!");
			this.End();
			return;
		}
		 anim.Play(AnimationName);
		this.End();
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
}
