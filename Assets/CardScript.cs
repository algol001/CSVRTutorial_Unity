using UnityEngine;
using System.Collections;

public class CardScript : MonoBehaviour {

	//public variables
	public GameObject FPSPlayer;
	
	public GameObject Card;

	public float speed;
	public float finalScale; //final scale of the card
	public float finalDistance; //distance from face
	public float finalHeight; 

	//private variables
	bool pickedUp;  //whether or not picked up
	Vector3 positionStart; //position vector at which FPS start
	float timeStart; //time started

	// Use this for initialization
	void Start () {
		pickedUp = false; //card initially not picked up
	}

	void OnTriggerEnter ( Collider other){ //collider other - object type collider named other (first person controller)
		if (!pickedUp) {
			//set to picked up
			pickedUp = true;

			var particle = GetComponentInChildren<ParticleSystem>();
			particle.emissionRate = 0;
			//get the audio
			var audio = GetComponent<AudioSource> ();
			audio.Play();

			//get animation
			var anim = GetComponentInChildren <Animation>();
			anim.Stop(); //starts off spinning so stop

			positionStart = transform.position; //call transform position function
			timeStart = Time.time; // calling time property from time class

		}
	}
	
	// Update is called once per frame
	void Update () {
		if (pickedUp) {
			var step = speed * (Time.time - timeStart)/ 100.0f; //100 is scaling var
			var target = FPSPlayer.GetComponent<Transform>(); //transform is position, getting current position of player
			var cardTransform = Card.GetComponent<Transform>();

			cardTransform.localRotation = Quaternion.Slerp(cardTransform.localRotation, Quaternion.identity, step);

			transform.position = Vector3.Slerp(positionStart, //vector3 = 3d vector
			                                   target.position + target.forward*finalDistance + target.up*finalHeight//calculate vector in front of face 
			                                   // target.forward is player direction, distance is how far in direction card should be
			                                   , step);
			transform.rotation = Quaternion.Slerp (transform.rotation //quaternion is 4d vector, representation rotation
			                                       ,target.rotation
			                                       , step);

			var scale = Mathf.Lerp(1, finalScale, step);
			transform.localScale = new Vector3(scale, scale, scale); //?
		}
	
	}
}
