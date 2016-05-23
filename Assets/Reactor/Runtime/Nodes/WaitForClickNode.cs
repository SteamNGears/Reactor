using UnityEngine;
using System.Collections;
using Reactor;

[NodeMenu("Wait/Wait for Click")]
public class WaitForClickNode : BaseNode 
{
	public enum ClickTypes{LEFT = 0, RIGHT = 1, MIDDLE = 2};
	public ClickTypes ClickType = ClickTypes.LEFT;
	public Collider target;
	Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	RaycastHit hit;

	public WaitForClickNode()
	{
		this.NodeName = "WAIT FOR CLICK";
	}


	// Use this for initialization
	void Start () 
	{
		if(this.target == null)
			this.target = this.gameObject.GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown((int)ClickType))
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 10000000)) 
			{

				if(hit.collider == this.target)
				{
					Debug.DrawLine(ray.origin, hit.point);
					this.End();
				}
			}
		}
	}
}
