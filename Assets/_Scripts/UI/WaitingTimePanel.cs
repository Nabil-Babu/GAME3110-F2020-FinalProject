using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    void OnWaitingTimeChangedCallback(int currentWaitingTime)
    {
        Debug.Log("Update waiting time text");
        waitingTimeText.text = currentWaitingTime.ToString();
    }

    void OnStartConnectToMatchMakingServerCallback()
    {
        Debug.Log("Started connecting to match making server");

        rectTransform.anchoredPosition = originPos;

        gameObject.SetActive(true);
    }
}
