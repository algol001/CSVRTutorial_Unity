using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Linq;

[RequireComponent(typeof(Text))]
public class ButtonLabel : MonoBehaviour {

    public GameObject Notebook;
    [Serializable]
    public class StateLabel
    {
        public String State;
        public String Label;
    }
    public StateLabel[] Labels;
    int[] hashStates;
    Animator anim;
    Text text;
	// Use this for initialization
	void Start () {
        anim = Notebook.GetComponent<Animator>();
        hashStates = Labels.Select(i => Animator.StringToHash(i.State)).ToArray();
        text = GetComponent<Text>();
	}
	
    String GetText()
    {
        var info = anim.GetCurrentAnimatorStateInfo(0);
        for (int i = 0; i < hashStates.Length; i++)
        {
            if (hashStates[i] == info.fullPathHash)
            {
                return Labels[i].Label;
            }
        }
        return String.Empty;
    }

	// Update is called once per frame
	void Update () {
        text.text = GetText();
        
	}
}
