using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    TMP_InputField UserField;
    TMP_InputField PassField;
    public GameObject FailureText;
    public GameObject RegisterPanel;
    public static PlayerStats CurrentPlayer; 
    public String username;
    public String password;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        UserField = GameObject.Find("Username").GetComponent<TMP_InputField>();
        PassField = GameObject.Find("Password").GetComponent<TMP_InputField>();
        GameObject.Find("Login").GetComponent<Button>().onClick.AddListener(LoginUser);
        GameObject.Find("newRegister").GetComponent<Button>().onClick.AddListener(OpenRegister);
    }

    void Update()
    {
        username = UserField.text;
        password = PassField.text;
    }

    void OpenRegister()
    {
        RegisterPanel.SetActive(true);
    }

    void LoginUser() => StartCoroutine(LoginUser_Coroutine());

    IEnumerator LoginUser_Coroutine()
    {
        string uri = "https://teelcode-backend-148419202297.us-east1.run.app/users/login";

        string rawJson = $@"{{
  ""username_or_email"": ""{username}"",
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
                Debug.LogError($"{request.responseCode}: {request.error}\n{request.downloadHandler.text}");
            else
                if (request.responseCode == 200)
            {
                string json = request.downloadHandler.text;
                Debug.Log(json);
                PlayerDataHolder.CurrentPlayer = JsonUtility.FromJson<PlayerStats>(json);
                Debug.Log(PlayerDataHolder.CurrentPlayer.user_id);
                SceneManager.LoadScene("Town Square");
            }
            FailureText.SetActive(true);
            //Debug.Log("Login Response: " + request.downloadHandler.text);
        }
    }
}