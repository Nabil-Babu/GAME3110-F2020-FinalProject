using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Dialog : MonoBehaviour
{
    TextMeshProUGUI text = null;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void SetText(string newText)
    {
        gameObject.SetActive(true);
        text.text = newText;
    }
}
