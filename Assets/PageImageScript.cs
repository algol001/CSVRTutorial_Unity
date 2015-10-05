using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Image))]
public class PageImageScript : MonoBehaviour {

    public Sprite EmptySprite;
    public Sprite FoundSprite;
    public event EventHandler FoundEvent;

    void OnFound()
    {
        if (FoundEvent != null)
        {
            FoundEvent(this, new EventArgs());
        }
    }

    Animator anim;
    int stateGlow;
    StateEventBehaviour beh;
    Sprite currentSprite;
    Image img;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        stateGlow = Animator.StringToHash("Photo.Glow");
        beh = anim.GetBehaviour<StateEventBehaviour>();
        beh.StateEntered += Beh_StateEntered;
        currentSprite = EmptySprite;
        img = GetComponent<Image>();
	}

    private void Beh_StateEntered(object sender, StateEventBehaviour.StateEvent e)
    {
        if(e.Info.fullPathHash == stateGlow)
        {
            currentSprite = FoundSprite;
            OnFound();
        }
    }

    public void ImageFound()
    {
        anim.SetTrigger("Pickup");
    }

    // Update is called once per frame
    void Update () {
        img.sprite = currentSprite;
	}
}
