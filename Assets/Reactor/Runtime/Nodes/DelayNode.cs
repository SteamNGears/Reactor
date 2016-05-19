using UnityEngine;
using System.Collections;
using Reactor;

public class DelayNode : BaseNode
{
    [Range(0.0f, float.MaxValue)]
    public float delay = 1.0f;
	public float timer = 0.0f;

	public DelayNode()
	{
		this.NodeName = "DELAY";
	}

   	void Start()
	{
		this.timer = 0.0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= this.delay)
		{
			timer = delay;
			this.End();
		}
    }

}
