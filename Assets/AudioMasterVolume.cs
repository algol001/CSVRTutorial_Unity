using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioMasterVolume : MonoBehaviour {
	public float Volume;
	AudioSource source;
	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		Volume = source.volume;
		AudioListener.volume = Volume;
	}
}
