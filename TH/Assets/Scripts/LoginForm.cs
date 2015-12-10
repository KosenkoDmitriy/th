using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        Application.OpenURL(Settings.urlSignUp);
    }

    public void VisitRestore()
    {
        Application.OpenURL(Settings.urlRestore);
    }
}