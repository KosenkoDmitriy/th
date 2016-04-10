using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Facebook.MiniJSON;

// Include Facebook namespace
using Facebook.Unity;
using System.Collections.Generic;

public class LoginForm : MonoBehaviour
{
    void Start()
    {
        
    }

    public void LoginGet() {
        string email = GameObject.Find("Login").GetComponent<InputField>().text;
        string password = GameObject.Find("Password").GetComponent<InputField>().text;
        string url = string.Format("{0}/{1}/?e={2}&p={3}", Settings.host, Settings.actionLogin, email, password);
        if (Settings.isDebug) Debug.Log(url);

        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
    }

    public void LoginPost()
    {
        string email = GameObject.Find("Login").GetComponent<InputField>().text;
        string password = GameObject.Find("Password").GetComponent<InputField>().text;
        string url = string.Format("{0}/{1}", Settings.host, Settings.actionLogin);
        if (Settings.isDebug) Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("e", email);
        form.AddField("p", password);

        WWW www = new WWW(url, form);
        StartCoroutine(WaitForRequest(www));
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        // check for errors
        if (www.error == null)
        {
            // TODO: check
            Settings.key = www.text;
            Settings.isLogined = true;
            if (Settings.isDebug) Debug.Log("WWW Ok!: " + www.data);
            Application.LoadLevel(Settings.levelGame);
        }
        else
        {
            Settings.isLogined = false;
			string msg = string.Format("error login ({0})", www.error);
            if (Settings.isDebug) Debug.Log(msg);
            GameObject.Find("textInfo").GetComponent<Text>().text = msg;
        }
    }

    public void VisitSignUp() {
		Settings.OpenUrl (Settings.urlSignUp);
    }

    public void VisitRestore()
    {
		Settings.OpenUrl (Settings.urlRestore);
    }

    public void VisitCredits()
    {
		Settings.OpenUrl (Settings.urlCredits);
    }


	// Awake function from Unity's MonoBehavior
	void Awake ()
	{
		Debug.Log("Awake()");
		if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
			FB.Init(InitCallback, OnHideUnity);
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp();
		}
	}
	
	private void InitCallback ()
	{
		Debug.Log("InitCallback()");
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			Debug.Log("start ActivateApp()");
			FB.ActivateApp();
			Debug.Log("end ActivateApp()");

			// Continue with Facebook SDK
			// ...
		} else {
			Debug.Log("Failed to Initialize the Facebook SDK");
		}
	}
	
	private void OnHideUnity (bool isGameShown)
	{
		Debug.Log("OnHideUnity()");

		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}

	public void FacebookLogin() {
		Debug.Log("FacebookLogin()");

		//		var perms = new new System.Collections.Generic.List<string>(){"public_profile", "email", "user_friends"};
		var perms = new System.Collections.Generic.List<string>(){"public_profile"};
		FB.LogInWithReadPermissions(perms, AuthCallback);
	}

	public void FacebookLogout() {
		Debug.Log("FacebookLogout()");

		FB.LogOut();
	}

	private void AuthCallback (ILoginResult result) {
		Debug.Log("AuthCallback()");

		if (FB.IsLoggedIn) {
			// AccessToken class will have session details
			var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;

			// Print current access token's User ID
//			Debug.Log(aToken.UserId);
			// Print current access token's granted permissions
			foreach (string perm in aToken.Permissions) {
				Debug.Log(perm);
			}

			string query = "/"+aToken.UserId+"?fields=token_for_business&access_token="+aToken.TokenString;
//			string query = "/"+aToken.UserId;
			var form = new WWWForm();
//			form.AddField("fields", "token_for_business");
//			form.AddField("access_token", aToken.TokenString);
			FB.API(query, HttpMethod.GET, BusinessTokenCallback, form);

			//Load Picture
//			string url = "https" + "://graph.facebook.com/"+ AccessToken.CurrentAccessToken.UserId +"/picture";
//			url += "?access_token=" + AccessToken.CurrentAccessToken.TokenString;
//			WWW www = new WWW(url);
//			yield return www;
//			Texture2d profilePic = www.texture;

//			string url = "https" + "://graph.facebook.com/"+ AccessToken.CurrentAccessToken.UserId +"?fields=token_for_business";
//			url += "&access_token=" + AccessToken.CurrentAccessToken.TokenString;
//			WWW www = new WWW(url);
//			StartCoroutine(WaitForRequest(www));
		} else {
			Settings.isLogined = false;
			string msg = "User cancelled login";
			if (Settings.isDebug) Debug.Log(msg);
			GameObject.Find("textInfo").GetComponent<Text>().text = msg;
		}

	}

	private void BusinessTokenCallback(IGraphResult result){
//		Debug.Log(result);
		Debug.Log("BusinessTokenCallback()");

		var dict = Json.Deserialize(result.RawResult) as Dictionary<string,object>;
		string businessToken = (string)dict["token_for_business"];
		string uId = (string)dict["id"];

		
		string url = string.Format("{0}/{1}", Settings.host, Settings.actionFacebookLogin);
		if (Settings.isDebug) Debug.Log(url);

		WWWForm form = new WWWForm();
		form.AddField("u", businessToken);
		form.AddField("p", "facebook");
		
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www));
	}
	
	
}