using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Text;

[System.Serializable]
public class Player
{
    public string username;
    public int level;
    public int xp;
    public int proficiency;
}

[System.Serializable]
public class PlayerList
{
    public Player[] players;
}

public class LeaderboardScript : MonoBehaviour
{
    public TextMeshProUGUI LeaderboardText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //LeaderboardText = GameObject.Find("LeaderboardText").GetComponent<TextMeshProUGUI>();
        LeaderboardText.text = "Loading...";
        GetData();
    }

    void GetData() => StartCoroutine(GetData_Coroutine());

    IEnumerator GetData_Coroutine()
    {
        LeaderboardText.text = "Loading GET...";
        string uri = "https://teelcode-backend-148419202297.us-east1.run.app/leaderboard?limit=10";

        Debug.Log(uri);

        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
                LeaderboardText.text = $"{request.responseCode}: {request.error}";
            else
            {
                string json = "{\"players\":" + request.downloadHandler.text + "}";
                PlayerList leaderboard = JsonUtility.FromJson<PlayerList>(json);

                string output = "";
                foreach (Player p in leaderboard.players)
                {
                    output += $"{p.username}: Level {p.level}\n";
                }

                LeaderboardText.text = output;

                //LeaderboardText.text = request.downloadHandler.text;
                //Debug.Log(leaderboard);
            }
        }
    }

}
