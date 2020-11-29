using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using NetworkMessages;
public class RegistPlayer : MonoBehaviour
{
    //[SerializeField] TextMeshProUGUI userNameText = null;
    //[SerializeField] TextMeshProUGUI passwordText = null;
    [SerializeField] TMP_InputField userNameText = null;
    [SerializeField] TMP_InputField passwordText = null;

    bool IsRegistingNewPlayer = false;

    [SerializeField] Dialog dialog = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    //// Update is called once per frame
    //void Update()
    //{

    //}


    public void RegisterNewPlayer()
    {
        if (userNameText.text == "" || passwordText.text == "")
        {
            Debug.LogWarning("Plese type username and password");
            dialog.SetText("Plese type username and password");
        }
        else
        {
            if (IsRegistingNewPlayer == false)
            {
                IsRegistingNewPlayer = true;
                StartCoroutine(RegisterNewPlayerCoroutine());
            }
            else
            {
                Debug.LogWarning("Already registing player, please wait");
            }
        }
    }


    IEnumerator RegisterNewPlayerCoroutine()
    {
        var uwr = new UnityWebRequest("https://44gomupt18.execute-api.us-east-2.amazonaws.com/default/FinalProject_RegistNewPlayer", "POST");

        UserIDandPassword userInfo = new UserIDandPassword();
        userInfo.user_id = userNameText.text;
        userNameText.text = "";

        userInfo.password = passwordText.text;
        passwordText.text = "";

        
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(JsonUtility.ToJson(userInfo));
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            dialog.SetText("Network error, check RegisterPlayer");
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            dialog.SetText("Register success. UserID : " + userInfo.user_id + ", password : " + userInfo.password);
        }

        IsRegistingNewPlayer = false;
    }
}
