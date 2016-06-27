using UnityEngine;
using System.Collections;


namespace Reactor
{
	[NodeMenu("Sound/Play Sound")]
	public class PlaySoundNode : BaseNode 
	{
		public AudioSource m_AudioSource;
		public AudioClip m_Clip;
		[Range(0,1)]
		public float m_Volume = 1.0f;

		public bool m_WaitForCompelte = false;

		public PlaySoundNode()
		{
			this.NodeName = "Play Sound";
		}

 
		// Use this for initialization
		void Start () 
		{
			if(m_AudioSource == null)
				m_AudioSource = GetComponent<AudioSource>();
			if(m_AudioSource == null)
			{
				Debug.Log("No audiosource attached or assigned");
				this.End();
				return;
			}
			if(m_Clip == null)
			{
				m_AudioSource.volume = m_Volume;
				m_AudioSource.Play();
			}
			else
				m_AudioSource.PlayOneShot(m_Clip,m_Volume);

			if(!m_WaitForCompelte)
				this.End ();

		}
		
		// Update is called once per frame
		void Update () 
		{
			if(!this.m_AudioSource.isPlaying)
				this.End ();
		}
	}
}