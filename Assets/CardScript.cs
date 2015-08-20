using UnityEngine;
using System.Collections;

public class CardScript : MonoBehaviour {

	//public variables
	public GameObject FPSPlayer;
	public float speed;
	public float finalScale; //final scale of the card
	public float finalDistance; //ending distance for card?

	//private variables
	bool pickedUp;  //whether or not picked up
	Vector3 positionStart; //position vector at which FPS start
	float timeStart; //time started

	// Use this for initialization
	void Start () {
		pickedUp = false; //card initially not picked up
	}

	void OnTriggerEnter ( Collider other){ //passing in box collider info?
		if (!pickedUp) {
			//set to picked up
			pickedUp = true;

			//get the audio
		//	var audio = GetComponent<AudioSource> ();
			//audio.play ();

			//get animation
			var anim = GetComponent <Animation>();
			anim.Stop(); //would you want to play first?

			positionStart = transform.position; //call transform position function
			timeStart = Time.time; //?

		}
	}
	
	// Update is called once per frame
	void Update () {
		if (pickedUp) {
			var step = speed * (Time.time - timeStart)/ 100.0f; //100 is the total width board?
			var target = FPSPlayer.GetComponent<Transform>(); //target is player?

			transform.position = Vector3.Slerp(positionStart,
			                                   target.position + target.forward*finalDistance  
			                                   , step);
			transform.rotation = Quaternion.Slerp (transform.rotation
			                                       ,target.rotation
			                                       , step);

			var scale = Mathf.Lerp(1, finalScale, step);
			transform.localScale = new Vector3(scale, scale, scale); //?
		}
	
	}
}
