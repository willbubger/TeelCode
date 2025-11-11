using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
public class QuizMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject categoryButtonTemplate;
    public Transform categoryContainer;
    public GameObject quizButtonTemplate;
    public Transform quizContainer;
    public TextMeshProUGUI LeaderboardText;
    public TextMeshProUGUI LowLevelText;

    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject questPanel;
    public GameObject cosmeticPanel;
    public GameObject leaderboardPanel;

    private Dictionary<string, List<TextAsset>> quizzesByCategory = new();

    void Start()
    {
        GenerateCategoryButtons();
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
        leaderboardPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void ShowCosmeticPanel()
    {
        mainPanel.SetActive(false);
        cosmeticPanel.SetActive(true);
    }

    public void showLeaderboard()
    {
        mainPanel.SetActive(false);
        leaderboardPanel.SetActive(true);
    }

    // -----------------------------
    // BUTTON GENERATION
    // -----------------------------
    // Dictionary to store categories and their quizzes
    void GenerateCategoryButtons()
    {
        // Load all quiz files from Resources/Quizzes
        TextAsset[] allQuizzes = Resources.LoadAll<TextAsset>("Quizzes");

        if (allQuizzes.Length == 0)
        {
            Debug.LogWarning("⚠️ No quiz files found in Resources/Quizzes!");
            return;
        }

        // Clear any existing buttons (optional safety)
        foreach (Transform child in categoryContainer)
            Destroy(child.gameObject);

        // Group by category (prefix before underscore)
        foreach (TextAsset quiz in allQuizzes)
        {
            string[] parts = quiz.name.Split('_');
            string category = parts.Length > 1 ? parts[0] : "Uncategorized";

            if (!quizzesByCategory.ContainsKey(category))
                quizzesByCategory[category] = new List<TextAsset>();

            quizzesByCategory[category].Add(quiz);
        }

        // Create category buttons dynamically
        foreach (var entry in quizzesByCategory)
        {
            string categoryName = entry.Key;

            GameObject newButton = Instantiate(categoryButtonTemplate, categoryContainer);
            newButton.SetActive(true);

            TextMeshProUGUI btnText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            btnText.text = categoryName;

            Button btn = newButton.GetComponent<Button>();
            btn.onClick.AddListener(() => ShowQuizzes(categoryName));
        }

        // Show category list, hide quiz list initially
        categoryContainer.gameObject.SetActive(true);
        quizContainer.gameObject.SetActive(false);
    }

    void ShowQuizzes(string categoryName)
    {
        // Hide categories, show quiz list
        categoryContainer.gameObject.SetActive(false);
        quizContainer.gameObject.SetActive(true);

        // Clear any existing quiz buttons
        foreach (Transform child in quizContainer)
            Destroy(child.gameObject);

        // Generate quiz buttons for the selected category
        foreach (TextAsset quiz in quizzesByCategory[categoryName])
        {
            GameObject newButton = Instantiate(quizButtonTemplate, quizContainer);
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
        Debug.Log($"[QUIZ] PlayerDataHolder.CurrentPlayer = {(PlayerDataHolder.CurrentPlayer == null ? "NULL" : PlayerDataHolder.CurrentPlayer.username)}");
        Debug.Log($"Loading quiz: {quizName}");

        TextAsset quizFile = Resources.Load<TextAsset>($"Quizzes/{quizName}");
        if (quizFile == null)
        {
            Debug.LogError($"❌ Quiz file not found: Resources/Quizzes/{quizName}");
            return;
        }

        int requiredLevel = GetRequiredLevel(quizFile);

        /*
        if (Login.CurrentPlayer == null)
        {
            Debug.LogError("❌ No current player loaded. You probably ran this scene directly without logging in first.");
            return;
        }
        */

        if (PlayerDataHolder.CurrentPlayer.level >= requiredLevel)
        {
            PlayerPrefs.SetString("SelectedQuiz", quizName);
            PlayerPrefs.Save();
            SceneManager.LoadScene("Quiz");
        }
        else
        {
            ShowLevelTooLowMessage(requiredLevel);
        }
    }


    int GetRequiredLevel(TextAsset quiz)
    {
        string[] lines = quiz.text.Split('\n');
        foreach (string rawLine in lines)
        {
            string line = rawLine.Trim();
            if (line.StartsWith("RequiredLevel"))
            {
                string[] parts = line.Split(',');
                if (parts.Length > 1 && int.TryParse(parts[1], out int level))
                    return level;
            }

            // stop scanning once we hit question header
            if (line.StartsWith("Question"))
                break;
        }
        return 1; // default if not found
    }

    void ShowLevelTooLowMessage(int requiredLevel)
    {
        LowLevelText.text = $"You need to be at least level {requiredLevel} to access this quiz.";
        LowLevelText.gameObject.SetActive(true);
        Debug.Log($"You need to be at least level {requiredLevel} to access this quiz.");
    }

    // Optional: add a "Back" button in UI to return to categories
    public void BackToCategories()
    {
        quizContainer.gameObject.SetActive(false);
        categoryContainer.gameObject.SetActive(true);
    }

    public void LeaderboardButton()
    {
        mainPanel.SetActive(false);
        leaderboardPanel.SetActive(true);
        GetLeaderboard();
    }


    void GetLeaderboard() => StartCoroutine(GetLeaderboard_Coroutine());

    IEnumerator GetLeaderboard_Coroutine()
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
                foreach (LeaderboardPlayer p in leaderboard.players)
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