using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using UnityEditor;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class QuizManager : MonoBehaviour
{
    public List<QuestionAndAnswers> QnA;
    public int questionsLeft;
    public GameObject[] options;
    public GameObject quizCompletePanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI livesText;
    public GameObject backgroundMusic;
    public int currentQuestion;

    public TextMeshProUGUI QuestionText;
    public int livesLeft;
    public string FileName;
    public TextAsset quizFile;
    public void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "Lives: " + livesLeft;
    }

private void Start()
    {
        Debug.Log(PlayerDataHolder.CurrentPlayer.user_id);
        livesLeft = 3;
        UpdateLivesUI();

        // ✅ Load quiz data from CSV first
        if (quizFile != null)
        {
            LoadQuizFromCSV(quizFile);
        }
        else
        {
            // Default load from Resources folder (in case not assigned)
            string selectedQuiz = PlayerPrefs.GetString("SelectedQuiz", "CSBasics");
            Debug.Log("Selected quiz: " + selectedQuiz);

            TextAsset csv = Resources.Load<TextAsset>($"Quizzes/{selectedQuiz}");
            if (csv != null)
            {
                FileName = LoadQuizFromCSV(csv);
                questionsLeft = QnA.Count;
                QnA = QnA.OrderBy(q => UnityEngine.Random.value).ToList();
                GenerateQuestion();
            }
            else
            {
                Debug.LogError($"⚠️ Could not load quiz: {selectedQuiz}");
                return;
            }
        }

    }

    // ✅ Reads CSV and populates QnA list
    public string LoadQuizFromCSV(TextAsset csvFile)
    {
        QnA = new List<QuestionAndAnswers>();
        string[] lines = csvFile.text.Split('\n');

        for (int i = 3; i < lines.Length; i++) // skip header
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] values = line.Split(',');

            if (values.Length < 6)
            {
                Debug.LogWarning($"Skipping malformed line {i + 1}: {line}");
                continue;
            }

            QuestionAndAnswers qa = new QuestionAndAnswers();
            qa.question = values[0];
            qa.answers = new string[4] { values[1], values[2], values[3], values[4] };
            qa.correctAnswerIndex = int.Parse(values[5]);

            QnA.Add(qa);
        }

        Debug.Log($"Loaded {QnA.Count} questions from {csvFile.name}");
        return csvFile.name;
    }
    public void Correct()
    {
        GenerateQuestion();
    }

    void SetAnswers()
    {
        for (int i = 0; i < options.Length; i++)
        {
            var answerComp = options[i].GetComponent<Answers>();
            answerComp.isCorrect = false;

            var textComponent = options[i].GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = QnA[currentQuestion].answers[i];

            if (QnA[currentQuestion].correctAnswerIndex == i)
            {
                answerComp.isCorrect = true;
            }
        }
    }

    void GenerateQuestion()
    {
        Debug.Log(PlayerDataHolder.CurrentPlayer.user_id);
        int charLocation = FileName.IndexOf("_", StringComparison.Ordinal);
        String Category = FileName.Substring(0, charLocation);
        Debug.Log(Category);


        string rawJson = "";

        if (FileName.Contains("Easy"))
        {
            rawJson = $@"{{
  ""user_id"": {PlayerDataHolder.CurrentPlayer.user_id},
  ""category"": ""{Category}"",
  ""difficulty"": ""easy"",
  ""lives_left"": {livesLeft}
}}";
        }
        else if (FileName.Contains("Medium"))
        {
            rawJson = $@"{{
  ""user_id"": {PlayerDataHolder.CurrentPlayer.user_id},
  ""category"": ""{Category}"",
  ""difficulty"": ""medium"",
  ""lives_left"": ""{livesLeft}""
}}";
        }
        else if (FileName.Contains("Hard"))
        {
            rawJson = $@"{{
  ""user_id"": {PlayerDataHolder.CurrentPlayer.user_id},
  ""category"": ""{Category}"",
  ""difficulty"": ""hard"",
  ""lives_left"": {livesLeft}
}}";
        }
        Debug.Log("Quest complete JSON: " + rawJson);

        string uri = "https://teelcode-backend-148419202297.us-east1.run.app/player/quest_result";

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(rawJson);

        if (questionsLeft <= 0)
        {
            StartCoroutine(SendQuestResult(uri, bodyRaw, rawJson, Category));
            quizCompletePanel.SetActive(true);
            return;
        }

        currentQuestion = questionsLeft - 1;
        QuestionText.text = QnA[currentQuestion].question;
        questionsLeft--;
        SetAnswers();
    }
    
    IEnumerator SendQuestResult(string uri, byte[] bodyRaw, string rawJson, string Category)
    {
        Debug.Log(rawJson);
        Debug.Log(Category);
        using (UnityWebRequest request = new UnityWebRequest(uri, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json; charset=utf-8");

            yield return request.SendWebRequest();

            Debug.Log("Sending JSON: " + rawJson);

            if (request.result != UnityWebRequest.Result.Success)
                Debug.Log(request.downloadHandler.text);
            else
                Debug.Log(request.downloadHandler.text);
        }
    }
}
