using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class myScreenshot : MonoBehaviour {
	public string keyToPress = "p";
	public int resolutionModifier = 1;
	public string prefix = "ss";
	int id;
	bool take = false;

	void Start () 
	{
		if (!System.IO.Directory.Exists(Application.dataPath + "/../Screenshots"))
			System.IO.Directory.CreateDirectory (Application.dataPath + "/../Screenshots");
	}

	void Update ()
	{
		if (Input.GetKeyUp (keyToPress)) 
		{
			take = true;

		}
	}


	void OnPostRender () {

		if (take) 
		{
			string dateTime =     DateTime.Now.Month.ToString()+ "-" + 
				DateTime.Now.Day.ToString() + "_" + 
				DateTime.Now.Hour.ToString() + "-" + 
				DateTime.Now.Minute.ToString() + "-" + 
				DateTime.Now.Second.ToString();
			string filename = prefix + id.ToString() + "_" + dateTime + ".png";
			Application.CaptureScreenshot((Application.dataPath + "/../Screenshots/" + filename), resolutionModifier);
			id++;
			take = false;
		}
	}
}
