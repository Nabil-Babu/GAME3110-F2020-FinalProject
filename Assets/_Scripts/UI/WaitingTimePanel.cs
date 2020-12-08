using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class WaitingTimePanel : MonoBehaviour
{
    RectTransform rectTransform = null;

    [SerializeField] NetworkClient networkClient = null;
    [SerializeField] TextMeshProUGUI waitingTimeText = null;

    Vector2 originPos = new Vector2(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        networkClient.onWaitingTimeChanged.AddListener(OnWaitingTimeChangedCallback);
        networkClient.onStartConnectToMatchMakingServer.AddListener(OnStartConnectToMatchMakingServerCallback);
        networkClient.onMatchFound.AddListener(OnMatchFoundCallback);

        //Save original position of this panel
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            originPos = rectTransform.anchoredPosition;

            Vector2 temp = new Vector2(0, 1000);
            rectTransform.anchoredPosition = temp;
        }

        //gameObject.SetActive(false);
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void OnDestroy()
    {
        networkClient.onWaitingTimeChanged.RemoveListener(OnWaitingTimeChangedCallback);
        networkClient.onStartConnectToMatchMakingServer.RemoveListener(OnStartConnectToMatchMakingServerCallback);
        networkClient.onMatchFound.RemoveListener(OnMatchFoundCallback);
    }

    void OnWaitingTimeChangedCallback(int currentWaitingTime)
    {
        //Debug.Log("Update waiting time text");

        //waitingTimeText.text = currentWaitingTime.ToString();
        var ts = TimeSpan.FromSeconds(currentWaitingTime);
        waitingTimeText.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
    }

    void OnStartConnectToMatchMakingServerCallback()
    {
        Debug.Log("Started connecting to match making server");

        rectTransform.anchoredPosition = originPos;

        gameObject.SetActive(true);
    }

    void OnMatchFoundCallback()
    {
        Debug.Log("Match found");
        waitingTimeText.text = "Match Found!!";

        Invoke("GoToMainScene", 3.0f);
    }

    void GoToMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
