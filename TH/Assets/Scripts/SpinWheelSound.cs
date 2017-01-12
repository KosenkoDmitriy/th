using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWheelSound : MonoBehaviour {
	public AudioSource audio;
	public AudioClip soundBtnClicked;

	// Use this for initialization
	void Start () {
		audio = gameObject.AddComponent<AudioSource> ();
		audio.volume = 0.1f;
		soundBtnClicked = Resources.Load<AudioClip> ("Sounds/spin-tick");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

//	void OnCollisionEnter2D(){
//		Debug.Log ("collider : " );
//	}

	void OnCollisionEnter2D (Collision2D col)
	{		

		if(col.gameObject.tag == "fwCollider")
		{
			Debug.Log ("obj : " + col.gameObject.name);
			audio.PlayOneShot(soundBtnClicked);
			//Destroy(col.gameObject);
		}
	}
}
