using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpinWheel : MonoBehaviour
{
	public AudioSource audio;
	public AudioClip soundBtnClicked;

	public List<int> prize;
	public List<AnimationCurve> animationCurves;
	
	private bool spinning;	
	private float anglePerItem;	
	private int randomTime;
	private int itemNumber;
	private Text text;

	void Start(){
		//text = GameObject.Find("FwScrollView").GetComponentInChildren<Text>();
		text = GameObject.Find("FwText").GetComponent<Text>();
		spinning = false;
		anglePerItem = 360/prize.Count;

		audio = gameObject.AddComponent<AudioSource> ();
		audio.volume = Settings.audioVolume;
		soundBtnClicked = Resources.Load<AudioClip> ("Sounds/VideoWin");
	}
	
	void  Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space) && !spinning) {
			SpinWheelClick();
		}
	}

	public void SpinWheelClick() {
		text.text = string.Format("pls wait ...");

		randomTime = Random.Range (3, 8);
		itemNumber = Random.Range (0, prize.Count);
		float maxAngle = 360 * randomTime + (itemNumber * anglePerItem);

		StartCoroutine (SpinTheWheel (5 * randomTime, maxAngle));
	}

	IEnumerator SpinTheWheel (float time, float maxAngle)
	{
		spinning = true;
		
		float timer = 0.0f;		
		float startAngle = transform.eulerAngles.z;		
		maxAngle = maxAngle - startAngle;
		
		int animationCurveNumber = Random.Range (0, animationCurves.Count);
		Debug.Log ("Animation Curve No. : " + animationCurveNumber);
		
		while (timer < time) {
		//to calculate rotation
			float angle = maxAngle * animationCurves [animationCurveNumber].Evaluate (timer / time) ;
			transform.eulerAngles = new Vector3 (0.0f, 0.0f, angle + startAngle);
			timer += Time.deltaTime;
			yield return 0;
		}
		
		transform.eulerAngles = new Vector3 (0.0f, 0.0f, maxAngle + startAngle);
		spinning = false;

		Debug.Log ("Prize: " + prize [itemNumber]);//use prize[itemNumnber] as per requirement
		//text.text = "Prize: " + prize [itemNumber];
		Add(prize[itemNumber].ToString());
	}

	public void Add(string amount)
	{
		string url = string.Format("{0}/{1}", Settings.host, "fw_th");
		if (Settings.isDebug) Debug.Log(url);

		WWWForm form = new WWWForm();
		form.AddField("a", amount);
		form.AddField("k", Settings.key);

		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www));
	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		audio.PlayOneShot(soundBtnClicked);

		// check for errors
		if (www.error == null)
		{
			Settings.freeCredits = (double)prize[itemNumber];
			text.text = string.Format("you win {0} free credits", Settings.freeCredits.f());
			if (Settings.isDebug) Debug.Log("fw api Ok!: " + www.data);
		}
		else
		{
			//Settings.freeCredits = 0;
			string msg = "error fw api: " + www.error;

			//TODO when no response - empty string
			text.text = www.text; //or www.data (response)
			if (string.IsNullOrEmpty(www.text)) text.text = "please check your internet connection ";
			if (Settings.isDebug) Debug.Log(msg);
		}
	}
}
