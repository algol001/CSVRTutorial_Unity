using UnityEngine;
using System.Collections;

public class LinkEnabledObjects : MonoBehaviour {

    public GameObject[] Objects;

	// Use this for initialization
	void Start () {
	
	}
	
    bool IsEnabled()
    {
        foreach(var o in Objects)
        {
            if (o.activeInHierarchy)
            {
                return true;
            }
        }
        return false;
    }


	// Update is called once per frame
	void Update () {
        if (!IsEnabled())
        {
            this.gameObject.SetActive(false);
        }
	}
}
