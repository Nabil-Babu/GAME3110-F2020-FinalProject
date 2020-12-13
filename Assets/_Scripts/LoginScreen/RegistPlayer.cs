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
            dialog.SetText("Plese type username and password", true);
        }
        else
        {
            if (IsRegistingNewPlayer == false)
            {
                IsRegistingNewPlayer = true;
                dialog.SetText("Registering new player...", false);
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

        UserInfo userInfo = new UserInfo();
        userInfo.user_id = userNameText.text;
        userNameText.text = "";

        userInfo.password = passwordText.text;
        passwordText.text = "";

        userInfo.userName = "UnknownSoldier";
        userInfo.skillLevel = Random.Range(0, 1001).ToString();


        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(JsonUtility.ToJson(userInfo));
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            if (uwr.responseCode == 401)
            {
                Debug.LogWarning("User ID already exist, try other user ID");
                dialog.SetText("User ID already exist, try other user ID", true);
            }
            else
            {
                Debug.Log("Error While Sending: " + uwr.error);
                dialog.SetText("Network error, check RegisterPlayer", true);
            }
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            dialog.SetText("Register success. UserID : " + userInfo.user_id + ", password : " + userInfo.password, true);
        }

        IsRegistingNewPlayer = false;
    }
}
