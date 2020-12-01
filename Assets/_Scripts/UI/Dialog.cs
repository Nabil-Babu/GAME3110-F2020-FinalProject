using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Dialog : MonoBehaviour
{
    TextMeshProUGUI text = null;
    Button button = null;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponentInChildren<Button>();
        gameObject.SetActive(false);
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void SetText(string newText, bool showButton)
    {
        gameObject.SetActive(true);
        text.text = newText;

        if(showButton == true)
        {
            button.gameObject.SetActive(true);
        }
        else
        {
            button.gameObject.SetActive(false);
        }
    }
}
