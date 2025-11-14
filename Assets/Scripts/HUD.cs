using UnityEngine;
using TMPro;
using System;
using UnityEngine.Networking;
using System.Collections;


public class HUD : MonoBehaviour
{
    public TextMeshProUGUI LevelTxt;
    public TextMeshProUGUI StreakTxt;
    public TextMeshProUGUI NextQuiz;
    public int CurrentLowest = int.MaxValue;
    public string RecQuiz;

    void Start()
    {
        StartCoroutine(InitializeHUD());
    }

    IEnumerator InitializeHUD()
    {
        // Wait until player data is fully loaded
        yield return StartCoroutine(GetInfo_Coroutine());

        // NOW update the HUD
        GetLevel();
        GetStreak();
        GetQuestsPro();
    }

    void GetInfo() => StartCoroutine(GetInfo_Coroutine());

    IEnumerator GetInfo_Coroutine()
    {
        string uri = "https://teelcode-backend-148419202297.us-east1.run.app/player/" + PlayerDataHolder.CurrentPlayer.user_id;

        Debug.Log(uri);

        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();

            string json = request.downloadHandler.text;
            Debug.Log($"[INFO] Raw JSON: {json}");
            JsonUtility.FromJsonOverwrite(json, PlayerDataHolder.CurrentPlayer);
            Debug.Log("User level: " + PlayerDataHolder.CurrentPlayer.level + "!");
            Debug.Log("Streak: " + PlayerDataHolder.CurrentPlayer.login_streak + "!");
            Debug.Log("Proficiency: " + PlayerDataHolder.CurrentPlayer.proficiency);
            }
    }

    void GetLevel()
    {
        LevelTxt.text = "Level: " + PlayerDataHolder.CurrentPlayer.level;
    }

    void GetStreak()
    {
        StreakTxt.text = "Streak: " + PlayerDataHolder.CurrentPlayer.login_streak;
    }
    
    void GetQuestsPro()
    {
        CurrentLowest = int.MaxValue;
        RecQuiz = "";
        TextAsset[] allQuizzes = Resources.LoadAll<TextAsset>("Quizzes");
        foreach (TextAsset quiz in allQuizzes)
        {
            string[] lines = quiz.text.Split('\n');
            foreach (string line in lines)
            {
                if (line.Contains("RecommendedProficiency"))
                {
                    String[] LineParts = line.Split(',');
                    int RecPro = int.Parse(LineParts[1]);
                    if (RecPro >= PlayerDataHolder.CurrentPlayer.proficiency && RecPro < CurrentLowest)
                    {
                        CurrentLowest = RecPro;
                        Debug.Log(quiz.name);
                        RecQuiz = quiz.name;
                        NextQuiz.text = "Next quiz: " + RecQuiz;
                    }
                    break;
                }
            }
        }
    }
}
