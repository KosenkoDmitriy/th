﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWheelSound : MonoBehaviour {
	private AudioSource audio;
	private AudioClip soundBtnClicked;

	// Use this for initialization
	void Start () {
		audio = gameObject.AddComponent<AudioSource> ();
		audio.volume = Settings.audioVolume;
		soundBtnClicked = Resources.Load<AudioClip> ("Sounds/spin_tick");//spin_tick");
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
