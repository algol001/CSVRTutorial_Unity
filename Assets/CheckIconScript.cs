using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

[RequireComponent(typeof(PageImageScript))]
public class CheckIconScript : MonoBehaviour {

    public GameObject[] Collectibles;

    PageImageScript imageScript;
    PageImageScript[] imageScripts;
    IList<PageImageScript> found;

	// Use this for initialization
	void Start () {
        found = new List<PageImageScript>();
        imageScript = GetComponent<PageImageScript>();
        imageScripts = Collectibles.Select(i => i.GetComponent<PageImageScript>()).ToArray();
        foreach(var s in imageScripts)
        {
            s.FoundEvent += Img_FoundEvent;
        }
	}

    private void Img_FoundEvent(object sender, System.EventArgs e)
    {
        var s = sender as PageImageScript;
        if (s != null)
        {
            if (!found.Contains(s))
            {
                found.Add(s);
            }
        }
        if(found.Count == imageScripts.Length)
        {
            imageScript.ImageFound();
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
