using UnityEngine;
using System.Collections;

namespace Reactor
{

	/// <summary>
	/// A base node class for the node editor
	/// </summary>
	public abstract class BaseNode : MonoBehaviour
	{
		[HideInInspector]
		public int id;

		[HideInInspector]
		public string NodeName;

		/// <summary>
		/// The next nodes in the sequence
		/// </summary>
		[HideInInspector]
		public BaseNode[] next = null;	///@TODO: Protect this somehow

        /// <summary>
		///the previous node that called this one(can be used to get previous data in wierd situations)
		/// </summary>
        [HideInInspector]
        public BaseNode prev = null;

		/// <summary>
		/// The position rectangle for the editor
		/// </summary>
		[HideInInspector]
		public Rect position = new Rect(0,0,200,100);///@TODO: Protect this somehow

		/// <summary>
		/// Used to call start so that Start() can be overridden without having to call base.Start()
		/// </summary>
		public void Init ()//@TODO: Change back to internal after testing
		{
			//this.Start ();
		}




		/// <summary>
		/// Call in Start() or Update() to indicate that this node has completed its lifecycle
		/// </summary>
		public void End ()
		{            
			this.OnComplete();
		}

        /// <summary>
        /// Override to have custom exit functionality
        /// </summary>
		public virtual void OnComplete()
        {
			if(this.next == null)
				return;
			foreach(BaseNode n in this.next)
			{
				n.enabled = true;
			}
			this.enabled = false;
        }

        /// <summary>
        /// Adds a child node top the current node
        /// </summary>
        /// <param name="a">The alpha component.</param>
        public void AddOutput(BaseNode a)
        {
            //don't allow looping
            if (a == this)
                return;
            if (this.next == null)
            {
                this.next = new BaseNode[] { a };
                return;
            }
            BaseNode[] newList = new BaseNode[(this.next.Length + 1)];
            int i = 0;
            for (; i < this.next.Length; i++)
                newList[i] = this.next[i];
            newList[i] = a;
            this.next = newList;
        }

		/// <summary>
        /// Adds a child node top the current node
        /// </summary>
        /// <param name="a"></param>
		public virtual void AddInput(BaseNode a)
		{
			if(this.prev != null)
				this.prev.Remove(this);
			this.prev = a;
		}


		public void Remove(BaseNode n)
		{
			if(this.next == null || this.next.Length < 1)
				return;
			BaseNode[] newNode = new BaseNode[this.next.Length - 1];
			int i = 0;
			int j = 0;
			while(i < this.next.Length && j < newNode.Length)
			{
				if(this.next[i] != n)
				{
					newNode[j] = this.next[i];
					j++;
				}
				i++;
			}
				
			this.next = newNode;
		}




        //-------------- @TODO: Move to editor script and call editor scrit from NodeEditor -------------
#if UNITY_EDITOR
        public virtual void OnNodeEditorGUI(int id)
		{
			// Do GUI stuff form editor script here
			GUILayout.Button("Hello world!!!");
		}
		#endif
	}
}