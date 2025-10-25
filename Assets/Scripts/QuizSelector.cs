using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class QuizMenu : MonoBehaviour
{
    [Header("UI References")]
    public Transform buttonContainer;      // Where buttons appear
    public GameObject quizButtonTemplate;  // The disabled button prefab

    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject questPanel;
    public GameObject cosmeticPanel;

    void Start()
    {
        GenerateQuizButtons();
        ShowMainPanel(); // ensure main menu is visible at start
    }

    // -----------------------------
    // PANEL LOGIC
    // -----------------------------
    public void ShowQuestPanel()
    {
        mainPanel.SetActive(false);
        questPanel.SetActive(true);
    }

    public void ShowMainPanel()
    {
        questPanel.SetActive(false);
        cosmeticPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void ShowCosmeticPanel()
    {
        mainPanel.SetActive(false);
        cosmeticPanel.SetActive(true);
    }

    // -----------------------------
    // BUTTON GENERATION
    // -----------------------------
    void GenerateQuizButtons()
    {
        // Load all quizzes from Resources/Quizzes
        TextAsset[] allQuizzes = Resources.LoadAll<TextAsset>("Quizzes");

        if (allQuizzes.Length == 0)
        {
            Debug.LogWarning("⚠️ No quiz files found in Resources/Quizzes!");
            return;
        }

        foreach (TextAsset quiz in allQuizzes)
        {
            GameObject newButton = Instantiate(quizButtonTemplate, buttonContainer);
            newButton.SetActive(true);

            TextMeshProUGUI btnText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            btnText.text = quiz.name;

            Button btn = newButton.GetComponent<Button>();
            string quizName = quiz.name;
            btn.onClick.AddListener(() => LoadQuiz(quizName));
        }
    }

    public void LoadQuiz(string quizName)
    {
        PlayerPrefs.SetString("SelectedQuiz", quizName);
        SceneManager.LoadScene("Quiz"); // your quiz gameplay scene
    }
}
