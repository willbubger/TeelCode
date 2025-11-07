using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class Register : MonoBehaviour
{
    TMP_InputField EmailField;
    TMP_InputField UsernameField;
    TMP_InputField PasswordField;
    public GameObject RegisterPanel;
    public GameObject FailText;
    public GameObject SuccessText;
    public String email;
    public String username;
    public String password;
    void Start()
    {
        EmailField = GameObject.Find("Email").GetComponent<TMP_InputField>();
        UsernameField = GameObject.Find("RegUsername").GetComponent<TMP_InputField>();
        PasswordField = GameObject.Find("RegPassword").GetComponent<TMP_InputField>();
        GameObject.Find("RegisterButton").GetComponent<Button>().onClick.AddListener(RegisterUser);
        GameObject.Find("BackButton").GetComponent<Button>().onClick.AddListener(BackButton);   
    }

    void Update()
    {
        email = EmailField.text;
        username = UsernameField.text;
        password = PasswordField.text;
    }

    void BackButton()
    {
        RegisterPanel.SetActive(false);
    }

    void RegisterUser() => StartCoroutine(RegisterUser_Coroutine());

    IEnumerator RegisterUser_Coroutine()
    {
        string uri = "https://teelcode-backend-148419202297.us-east1.run.app/users/register";

        string rawJson = $@"{{
  ""username"": ""{username}"",
  ""email"": ""{email}"",
  ""password"": ""{password}""
}}";

        Debug.Log("Login JSON: " + rawJson);

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(rawJson);

        using (UnityWebRequest request = new UnityWebRequest(uri, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json; charset=utf-8");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                SuccessText.SetActive(false);
                FailText.SetActive(true);
                Debug.LogError($"{request.responseCode}: {request.error}\n{request.downloadHandler.text}");
            }
            else
                if (request.responseCode == 200)
                {
                    FailText.SetActive(false);
                    SuccessText.SetActive(true);
                }
                Debug.Log("Login Response: " + request.downloadHandler.text);
        }
    }
}
