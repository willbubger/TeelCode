using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class QuizManager : MonoBehaviour
{
    public List<QuestionAndAnswers> QnA;
    public int questionsLeft;
    public GameObject[] options;
    public GameObject quizCompletePanel;
    public GameObject gameOverPanel;
    public GameObject backgroundMusic;
    public int currentQuestion;

    public TextMeshProUGUI QuestionText;
    public int livesLeft;

    private void Start()
    {
        livesLeft = 3;
        questionsLeft = QnA.Count;
        QnA = QnA.OrderBy(q => Random.value).ToList();
        GenerateQuestion();
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

            // 👇 Update to TMP for the answer text too
            var textComponent = options[i].GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = QnA[currentQuestion].answers[i];
            
            if (QnA[currentQuestion].correctAnswerIndex == i)
            {
                options[i].GetComponent<Answers>().isCorrect = true;
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