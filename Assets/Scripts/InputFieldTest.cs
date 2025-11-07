using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class InputFieldTest : MonoBehaviour
{
        TMP_InputField outputArea;
        public String username;

    void Start()
    {
        outputArea = GameObject.Find("TestField").GetComponent<TMP_InputField>();
        GameObject.Find("GetUser").GetComponent<Button>().onClick.AddListener(GetUsername);
    }

    void Update()
    {
        username = outputArea.text;
    }
    
    void GetUsername()
    {
        Debug.Log("Username: " + username);
    }
}
