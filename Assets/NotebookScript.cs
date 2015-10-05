using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
[RequireComponent(typeof(Animator))]
public class NotebookScript : MonoBehaviour {

    /// <summary>
    /// Player controller
    /// </summary>
    public GameObject FPSPlayer;

    /// <summary>
    /// Materials for each page
    /// </summary>
    public Material[] PageMaterials;

    /// <summary>
    /// Mesh of first page
    /// </summary>
    public GameObject FirstPage;

    /// <summary>
    /// Mesh of second page
    /// </summary>
    public GameObject SecondPage;

    /// <summary>
    /// Current page of notebook (0-based)
    /// </summary>
    public int CurrentPage;

    MeshRenderer firstMesh;
    MeshRenderer secondMesh;
    Animator anim;
    AnimatorStateInfo lastInfo;
    StateEventBehaviour beh;
    PlayerController cont;
    AudioSource audioSource;
    //States
    int idleState;
    int forwardState;
    int backwardState;
    int closedState;
    
    // Use this for initialization
    void Start () {

        firstMesh = FirstPage.GetComponent<MeshRenderer>();
        secondMesh = SecondPage.GetComponent<MeshRenderer>();
        cont = FPSPlayer.GetComponent<PlayerController>();

        anim = GetComponent<Animator>();
        beh = anim.GetBehaviour<StateEventBehaviour>();
        beh.StateEntered += Beh_StateEntered;
        beh.StateExited += Beh_StateExited;

        idleState = Animator.StringToHash("Notebook.Idle");
        forwardState = Animator.StringToHash("Notebook.Forward");
        backwardState = Animator.StringToHash("Notebook.Backward");
        closedState = Animator.StringToHash("Notebook.Closed");
        lastInfo = anim.GetCurrentAnimatorStateInfo(0);
        audioSource = GetComponent<AudioSource>();
    }

    public void SetBool(string name, bool val)
    {
        anim.SetBool(name, val);
    }

    private void Beh_StateExited(object sender, StateEventBehaviour.StateEvent e)
    {
        var info = e.Info;
        if(info.fullPathHash == forwardState)
        {
            PageForward();
        }
    }

    private void Beh_StateEntered(object sender, StateEventBehaviour.StateEvent e)
    {
        var info = e.Info;
        if (info.fullPathHash == idleState)
        {
            anim.ResetTrigger("ForwardTrigger");
            anim.ResetTrigger("BackTrigger");
            anim.ResetTrigger("NotebookTrigger");
        }

        if (info.fullPathHash == closedState)
        {
            anim.ResetTrigger("ForwardTrigger");
            anim.ResetTrigger("BackTrigger");
            anim.ResetTrigger("NotebookTrigger");

            cont.RemoveLock(this);
        }
        else
        {
            cont.AddLock(this);
        }

        if (info.fullPathHash == forwardState)
        {
        }

        // start backward
        if (info.fullPathHash == backwardState)
        {
            PageBack();
        }

        // from forward to idle
        if (info.fullPathHash == idleState &&
            lastInfo.fullPathHash == forwardState)
            {
                //Finished turing page
               // PageForward();
            }
        lastInfo = info;
        SetSprites();
    }

    void PageForward()
    {
        CurrentPage = (CurrentPage + 1) % PageMaterials.Length;
    }

    void PageBack()
    {
        CurrentPage--;
        if (CurrentPage < 0)
        {
            CurrentPage = PageMaterials.Length - 1;
        }
    }
    

    void SetSprites()
    {
        firstMesh.material = PageMaterials[CurrentPage % PageMaterials.Length];
        secondMesh.material = PageMaterials[(CurrentPage + 1) % PageMaterials.Length];
    }

    // Update is called once per frame
    void Update ()
    {
        var info = anim.GetCurrentAnimatorStateInfo(0);
        if (info.fullPathHash == idleState) {

            float horizontal = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxis("Horizontal");
            if (
                cont.MainAxisHorizontal() > 0.1
                )
            {
                anim.SetTrigger("ForwardTrigger");
            }
            if (cont.MainAxisHorizontal() < -0.1)
            {
                anim.SetTrigger("BackTrigger");
            }
            if (cont.ButtonNotebook())
            {
                anim.SetTrigger("NotebookTrigger");
            }
        }
        if(info.fullPathHash == closedState && info.normalizedTime >= 1)
        {
            if (cont.ButtonNotebook())
            {
                anim.SetTrigger("NotebookTrigger");
            }
        }

        SetSprites();
        lastInfo = info;
    }
}
