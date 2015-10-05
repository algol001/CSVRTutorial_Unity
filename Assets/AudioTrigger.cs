using UnityEngine;
using System.Collections;
using System.Linq;
using System.Xml;
using System;
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class AudioTrigger : MonoBehaviour {

    [Serializable]
    public class Trigger
    {
        public AudioClip Clip;
        public string State;
    }

    public Trigger[] Triggers;
    

    Animator anim;
    StateEventBehaviour beh;
    AudioSource audioSource;
    int[] hashStates;



    void OnEnable()
    {
        if (anim != null)
        {
            anim.gameObject.SetActive(true);
            anim.enabled = true;
            beh = anim.GetBehaviour<StateEventBehaviour>();
            beh.StateEntered += Beh_StateEntered;
        }
        // hidden = false;
    }

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        beh = anim.GetBehaviour<StateEventBehaviour>();
        hashStates = Triggers.Select(i => Animator.StringToHash(i.State)).ToArray();
        audioSource = GetComponent<AudioSource>();
        beh.StateEntered += Beh_StateEntered;
	}

    private void Beh_StateEntered(object sender, StateEventBehaviour.StateEvent e)
    {
        for(int i=0; i< hashStates.Length; i++)
        {
            if(hashStates[i] == e.Info.fullPathHash)
            {
                audioSource.PlayOneShot(Triggers[i].Clip);
            }
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
