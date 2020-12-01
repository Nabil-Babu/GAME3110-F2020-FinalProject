using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkMessages;

public class GlobalData : MonoBehaviour
{
    #region Singleton
    public static GlobalData instance;

    private void Awake()
    {
        //Make sure there is only one instance of SoundManager;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //Make it persist through levels
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public UserInfo userInfo = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
