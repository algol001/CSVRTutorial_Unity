using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using UnityEngine.UI;
public class MessageScript : MonoBehaviour {
    public GameObject[] LinkedObjects;

    string message;

    Animator anim;
    Animator[] animators;
    CanvasRenderer canvasRenderer;
    int stateIdle;
    int stateGlow;
    Text text;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        animators = LinkedObjects.Select(i => i.GetComponent<Animator>()).ToArray();
        canvasRenderer = GetComponent<CanvasRenderer>();
        stateIdle = Animator.StringToHash("Message.Idle");
        text = GetComponent<Text>();
        message = text.text;
        stateGlow = Animator.StringToHash("Photo.Glow");
    }
	
    bool IsDone()
    {
        foreach (var o in animators)
        {
            if(o.GetCurrentAnimatorStateInfo(0).fullPathHash != stateGlow)
            {
                return false;
            }
        }
        return true;
    }

    // Update is called once per frame
    void Update () {

        if (IsDone())
        {
            anim.SetTrigger("MessageTrigger");
        }        

        var info = anim.GetCurrentAnimatorStateInfo(0);
        if(info.fullPathHash == stateIdle)
        {
            text.text = string.Empty;
        }
        else
        {
            text.text = message;
        }
	}
}
