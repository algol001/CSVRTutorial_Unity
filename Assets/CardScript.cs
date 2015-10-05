using UnityEngine;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(ParticleSystem))]
public class CardScript : MonoBehaviour {

	// Public Variables
    /// <summary>
    /// Player controller
    /// </summary>
	public GameObject FPSPlayer;
    /// <summary>
    /// Notebook
    /// </summary>
    public GameObject Notebook;
    /// <summary>
    /// Target image
    /// </summary>
    public GameObject TargetImage;

    /// <summary>
    /// Linked game objects will be disabled until card is picked up
    /// </summary>
    public GameObject[] LinkedCards;
    
	public float speed;
	public float finalScale;
	public Vector3 finalPosition;

    /// <summary>
    /// Page Number
    /// </summary>
    public int PageNumber;

    // Private Variables
    PlayerController cont;
    ParticleSystem parts;
    PageImageScript targetScript;
    NotebookScript nbscript;
    Animator anim;
    StateEventBehaviour beh;

    int stateSpinning;
    int stateStopped;
    int statePutaway;

	Vector3 positionStart;
	float timeStart;

    /*
    void SetHidden(bool val)
    {
        this.gameObject.SetActive(!val);
    }
    */
    void OnEnable()
    {
        if(anim!= null)
        {
            anim.gameObject.SetActive(true);
            anim.enabled = true;
            beh = anim.GetBehaviour<StateEventBehaviour>();
            beh.StateEntered += Beh_StateEntered;
            beh.StateExited += Beh_StateExited;
        }
       // hidden = false;
    }
    void Awake()
    {
       // hidden = false;
    }

    // Use this for initialization
    void Start()
    {
        cont = FPSPlayer.GetComponent<PlayerController>();
        parts = GetComponent<ParticleSystem>();
        targetScript = TargetImage.GetComponent<PageImageScript>();
        nbscript = Notebook.GetComponent<NotebookScript>();

        stateSpinning = Animator.StringToHash("Card.Spinning");
        stateStopped = Animator.StringToHash("Card.Stopped");
        statePutaway = Animator.StringToHash("Card.Putaway");

        anim = GetComponentInChildren<Animator>();
        beh = anim.GetBehaviour<StateEventBehaviour>();
        beh.StateEntered += Beh_StateEntered;
        beh.StateExited += Beh_StateExited;
//        linkedScripts = LinkedCards.Select(i => i.GetComponent<CardScript>()).ToArray();
    }

    private void Beh_StateExited(object sender, StateEventBehaviour.StateEvent e)
    {
        if (e.Info.fullPathHash == stateSpinning)
        {
            cont.AddLock(this);

            positionStart = transform.position;
            timeStart = Time.time;
            //var particles =	GetComponentInChildren<ParticleSystem>();
            parts.emissionRate = 0;

        }
        if (e.Info.fullPathHash == stateStopped)
        {

            //parts.emissionRate = 100;
        }
        if(e.Info.fullPathHash == statePutaway)
        {
            parts.emissionRate = 0;
            cont.RemoveLock(this);
            nbscript.SetBool("ForceOpen", false);
            foreach (var card in LinkedCards)
            {
                card.SetActive(true);
            }
            this.gameObject.SetActive(false);
//            DestroyObject(this.gameObject);
        }
    }

    private void Beh_StateEntered(object sender, StateEventBehaviour.StateEvent e)
    {
        if(e.Info.fullPathHash == stateStopped)
        {
            nbscript.SetBool("ForceClosed", true);
        }
        if(e.Info.fullPathHash == statePutaway)
        {
            parts.emissionRate = 100;
            nbscript.SetBool("ForceClosed", false);
            nbscript.SetBool("ForceOpen", true);
            nbscript.CurrentPage = PageNumber;

            targetScript.ImageFound();
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject == FPSPlayer)
        {
            var info = anim.GetCurrentAnimatorStateInfo(0);
            if (info.fullPathHash == stateSpinning)
            {
                anim.SetTrigger("Pickup");
            }
        }
	}

	void Update () {
        var info = anim.GetCurrentAnimatorStateInfo(0);
        if(info.fullPathHash == stateStopped)
        {
            var step = speed * (Time.time - timeStart) / 100.0f;
            var target = FPSPlayer.GetComponent<Transform>();

            transform.position = Vector3.Slerp(positionStart,
                                               target.position + target.forward * finalPosition.z + target.up * finalPosition.y
                                                     , step);
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      target.rotation
                                                  , step);
            //transform.rotation = target.rotation;
            var scale = Mathf.Lerp(1, finalScale, step);
            transform.localScale = new Vector3(scale, scale, scale);

            if (step > 1)
            {
            //    state = States.PickedUp;
            }
            if (cont.ButtonNotebook())
            {
                anim.SetTrigger("Putaway");
            }
        }
        if (info.fullPathHash == stateSpinning)
        {
            foreach (var card in LinkedCards)
            {
                card.SetActive(false);
            }
        }        
    }
}
