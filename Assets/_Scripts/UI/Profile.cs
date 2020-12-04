using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NetworkMessages;

public class Profile : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI userIDText = null;
    [SerializeField] TextMeshProUGUI passwordText = null;
    [SerializeField] TextMeshProUGUI userNameText = null;
    [SerializeField] TextMeshProUGUI skillLevelText = null;

    // Start is called before the first frame update
    void Start()
    {
        if (GlobalData.instance != null)
        {
            UserInfo info = GlobalData.instance.userInfo;

            userIDText.text = "UserID: " + info.user_id;
            passwordText.text = "Password: " + info.password;
            userNameText.text = "UserName: " + info.userName;
            skillLevelText.text = "SkillLevel: " + info.skillLevel;
        }
        else
        {
            Debug.LogWarning("GlobalData.instance is null, check Profile");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
