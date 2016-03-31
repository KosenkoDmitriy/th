using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Include Facebook namespace
using Facebook.Unity;

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
            string msg = "error login";// "WWW Error: " + www.error;
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
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp();
			// Continue with Facebook SDK
			// ...
		} else {
//			Debug.Log("Failed to Initialize the Facebook SDK");
		}
	}
	
	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}

	public void FacebookLogin() {
		//		var perms = new new System.Collections.Generic.List<string>(){"public_profile", "email", "user_friends"};
		var perms = new System.Collections.Generic.List<string>(){"public_profile"};
		FB.LogInWithReadPermissions(perms, AuthCallback);
	}
	
	private void AuthCallback (ILoginResult result) {
		if (FB.IsLoggedIn) {
			// AccessToken class will have session details
			var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
			// Print current access token's User ID
			Debug.Log(aToken.UserId);
			// Print current access token's granted permissions
			foreach (string perm in aToken.Permissions) {
//				Debug.Log(perm);
			}
		} else {
//			Debug.Log("User cancelled login");
		}
	}

}