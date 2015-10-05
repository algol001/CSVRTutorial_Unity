using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class PlayerController : MonoBehaviour {

    UnityStandardAssets.Characters.FirstPerson.FirstPersonController fps;
    IList<Object> locks;
    public float MoveSpeed;
	MonoBehaviour motor;

    // Use this for initialization
    void Start () {
        fps = GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        locks = new List<Object>();
		motor = GetComponent("CharacterMotor") as MonoBehaviour;
	}
    
    public void AddLock(Object obj)
    {
        if (!locks.Contains(obj))
        {
            locks.Add(obj);
        }
		motor.enabled = false;
		//var cont = UnityStandardAssets.Characters.FirstPerson.
        //fps.m_WalkSpeed = 0;
		//var input = GetComponent ("FPSInputController") as MonoBehaviour;
		//motor.enabled = false;
    }
    public void RemoveLock(Object obj)
    {
        if (locks.Contains(obj))
        {
            locks.Remove(obj);
        }
        if(locks.Count == 0)
		{
			motor.enabled=true;
			//var input = GetComponent ("FPSInputController") as MonoBehaviour;
			//motor.enabled = true;
			//fps.m_WalkSpeed = MoveSpeed;
        }
    }

    public bool ButtonNotebook()
    {
        if (Input.GetButton("NotebookButton") == true)
        {
            return true;
        }
        if (Input.GetAxis("LeftTrigger") > 0)
        {
            return true;
        }
        if (Input.GetButton("LeftBumper") == true)
        {
            return true;
        }
        return false;
    }
    public bool ButtonSelect()
    {
        if (Input.GetButton("Submit") == true)
        {
            return true;
        }
        if (Input.GetAxis("RightTrigger") > 0)
        {
            return true;
        }
        if (Input.GetButton("RightBumper") == true)
        {
            return true;
        }
        return false;
    }
    public float MainAxisHorizontal()
    {
        return Input.GetAxis("Horizontal");
    }
    public float SecondAxisHorizontal()
    {
        return Input.GetAxis("RightStickX");
    }

    // Update is called once per frame
    void Update () {
	
	}
}
