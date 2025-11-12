using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Text;

public class BackendTester : MonoBehaviour
{
    TMP_InputField outputArea;

    void Start()
    {
        outputArea = GameObject.Find("OutputArea").GetComponent<TMP_InputField>();
        GameObject.Find("GetButton").GetComponent<Button>().onClick.AddListener(GetData);
        GameObject.Find("PostButton").GetComponent<Button>().onClick.AddListener(PostData);
    }

    // Get method
    void GetData() => StartCoroutine(GetData_Coroutine());

    IEnumerator GetData_Coroutine()
    {
        outputArea.text = "Loading GET...";
        string uri = "https://teelcode-backend-148419202297.us-east1.run.app/leaderboard?limit=10";

        Debug.Log(uri);

        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
                outputArea.text = $"{request.responseCode}: {request.error}";
            else
            {
                string json = "{\"players\":" + request.downloadHandler.text + "}";
                PlayerList leaderboard = JsonUtility.FromJson<PlayerList>(json);
            }
        }
    }

    // Post method
    void PostData() => StartCoroutine(PostData_Coroutine());

    IEnumerator PostData_Coroutine()
    {
        string uri = "https://teelcode-backend-148419202297.us-east1.run.app/player/quest_result";

        string rawJson = outputArea.text.Trim();
        outputArea.text = "Loading POST...";

        byte[] bodyRaw = Encoding.UTF8.GetBytes(rawJson);

        using (UnityWebRequest request = new UnityWebRequest(uri, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json; charset=utf-8");

            yield return request.SendWebRequest();

            Debug.Log("Sending JSON: " + rawJson);

            if (request.result != UnityWebRequest.Result.Success)
                outputArea.text = $"{request.responseCode}: {request.error}\n{request.downloadHandler.text}";
            else
                outputArea.text = request.downloadHandler.text;
        }
    }
}
