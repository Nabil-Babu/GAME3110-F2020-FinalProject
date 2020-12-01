using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using NetworkMessages;

public class LogIn : MonoBehaviour
{
    [SerializeField] TMP_InputField userNameText = null;
    [SerializeField] TMP_InputField passwordText = null;

    bool IsLogingIn = false;

    [SerializeField] Dialog dialog = null;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void LogInFunction()
    {
        if (userNameText.text == "" || passwordText.text == "")
        {
            Debug.LogWarning("Plese type username and password");
            dialog.SetText("Plese type username and password", true);
        }
        else
        {
            if (IsLogingIn == false)
            {
                IsLogingIn = true;
                dialog.SetText("Loging in...", false);
                StartCoroutine(LogInFunctionCoroutine());
            }
            else
            {
                Debug.LogWarning("Already Loging in, please wait");
            }
        }
    }


    IEnumerator LogInFunctionCoroutine()
    {
        string queryParams = "?" + "user_id=" + userNameText.text + "&password=" + passwordText.text;
        userNameText.text = "";
        passwordText.text = "";

        string fullURL = "https://4oubdkjt3g.execute-api.us-east-2.amazonaws.com/default/FinalProject_GetUserInfo" + queryParams;

        //var uwr = new UnityWebRequest(fullURL);

        //yield return uwr.SendWebRequest();

        //if (uwr.isNetworkError)
        //{
        //    Debug.Log("Error While Sending: " + uwr.error);
        //}
        //else
        //{
        //    uwr.text
        //    Debug.Log("Received: " + uwr.downloadHandler.text);
        //}

        UnityWebRequest www = UnityWebRequest.Get(fullURL);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //Debug.Log(www.error);
            Debug.LogWarning("Check user id and password");
            dialog.SetText("Check user id and password", true);
        }
        else
        {
            Debug.Log("Log in success");
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            UserInfo data = JsonUtility.FromJson<UserInfo>(www.downloadHandler.text);
            string s = data.user_id;

            dialog.SetText("Log in success", true);
        }

        IsLogingIn = false;
    }
}
