using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections;
using System.Collections.Generic;

// Include Facebook namespace
using Facebook.Unity;
using Facebook.MiniJSON;


public class LoginForm : MonoBehaviour
{
	EventSystem system;

    void Start()
    {
		system = EventSystem.current;
		var inputField = GameObject.Find("Login").GetComponent<InputField>();
		//system.SetSelectedGameObject(inputField.gameObject, null);
		//inputField.OnPointerClick(new PointerEventData(EventSystem.current));
		//inputField.ActivateInputField();
		//inputField.Select();
    }

	// Update is called once per frame
	public void Update()
	{
		
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			Selectable next = null;
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				if (system.currentSelectedGameObject != null) next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
				if (next == null && system.lastSelectedGameObject != null )
					next = system.lastSelectedGameObject.GetComponent<Selectable>();
			}
			else
			{
				if (system.currentSelectedGameObject != null) next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
				if (next == null && system.lastSelectedGameObject != null)
					next = system.firstSelectedGameObject.GetComponent<Selectable>();
			}
			
			if (next != null)
			{
				
				InputField inputfield = next.GetComponent<InputField>();
				if (inputfield != null) inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret
				
				system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
			}
			//else Debug.Log("next nagivation element not found");
			
		}
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
        if (string.IsNullOrEmpty(www.error))
        {
            // TODO: check
            Settings.key = www.text;
            Settings.isLogined = true;
			Settings.avatar = Resources.Load<Sprite>(Settings.avatarDefault);

            if (Settings.isDebug) Debug.Log("WWW Ok!: " + www.text);
            Application.LoadLevel(Settings.levelGame);
        }
        else 
		{
            Settings.isLogined = false;
//			string errorFromBytes3 = System.Text.Encoding.UTF8.GetString(www.bytes, 3, www.bytes.Length - 3);  // Skip thr first 3 bytes (i.e. the UTF8 BOM)
//			string errorFromBytes = System.Text.Encoding.UTF8.GetString(www.bytes);
//			Debug.Log(www.error);
//			Debug.Log(www.text);
//			Debug.Log(www.data);
//			Debug.Log(errorFromBytes3);
//			string msg = string.Format("error login ({0}\n{1})", errorFromBytes, www.error);
			string msg = ""; 
			if (string.IsNullOrEmpty(www.text))
				msg = "please login on website first and try again";
			else {
				msg = www.text;
				Debug.Log(www.text);
			}
            if (Settings.isDebug) Debug.Log(msg);
            GameObject.Find("textInfo").GetComponent<Text>().text = msg;
        }
    }

    public void VisitSignUp() {
		UnityEngine.Application.OpenURL(Settings.urlSignUp);
		//Settings.OpenUrl (Settings.urlSignUp);
    }

    public void VisitRestore()
    {
		Settings.OpenUrl (Settings.urlRestore);
    }

    public void VisitCredits()
    {
		UnityEngine.Application.OpenURL(Settings.urlCredits);
		//Settings.OpenUrl (Settings.urlCredits);
    }


	// Awake function from Unity's MonoBehavior
	void Awake ()
	{
		Debug.Log("Awake()");
		if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
			FB.Init(InitCallback, OnHideUnity);
			Debug.Log("FB.Init()");
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp();
			Debug.Log("FB.ActivateApp()");
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

			//Load Picture/Avatar for real/live player
			StartCoroutine(LoadAvatar());
		} else {
			Settings.isLogined = false;
			string msg = "User cancelled login";
			if (Settings.isDebug) Debug.Log(msg);
			GameObject.Find("textInfo").GetComponent<Text>().text = msg;
		}

	}

	public IEnumerator LoadAvatar() {
		var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
		
		string url = string.Format (Settings.facebookGraphPictureUrl, aToken.UserId);
		url += "?access_token=" + aToken.TokenString + "&width="+Settings.avatarWidth+"&height="+Settings.avatarHeight;
//		string urlTest = "https://th.shopomob.ru/assets/logo-ae05cc58d17983b5c41cd54530d1071ba7f03b7a5a92e75461873292c558bd56.png";
		Dictionary<string, string> headers = new Dictionary<string, string>();
//		headers.Add("Origin", Settings.facebookImageHost);
		string urlFinal = Settings.facebookImageUrl + url;
		WWW www = new WWW(urlFinal, null, headers);
		Debug.Log(urlFinal);
		yield return www;
		Texture2D profilePic = www.texture;
		Rect rect = new Rect(0, 0, profilePic.width, profilePic.height);
		Settings.avatar = Sprite.Create(profilePic, rect, new Vector2(0.5f, 0.5f), 100);
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