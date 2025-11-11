using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.IO;
using UnityEngine.UI; // 👈 for text/file handling

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
    public TextAsset quizFile;
    public void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "Lives: " + livesLeft;
    }

private void Start()
{
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
            LoadQuizFromCSV(csv);
            questionsLeft = QnA.Count;
            QnA = QnA.OrderBy(q => Random.value).ToList();
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
    void LoadQuizFromCSV(TextAsset csvFile)
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
        if (questionsLeft <= 0)
        {
            quizCompletePanel.SetActive(true);
            return;
        }

        currentQuestion = questionsLeft - 1;
        QuestionText.text = QnA[currentQuestion].question;
        questionsLeft--;
        SetAnswers();
    }
}
