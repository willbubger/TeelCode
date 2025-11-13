using UnityEngine;
using TMPro;


public class HUD : MonoBehaviour
{
    public TextMeshProUGUI LevelTxt;
    public TextMeshProUGUI StreakTxt;
    public TextMeshProUGUI NextQuiz;

    void Start()
    {
        GetLevel();
        GetStreak();
    }

    void Update()
    {

    }

    void GetLevel()
    {
        LevelTxt.text = "Level: " + PlayerDataHolder.CurrentPlayer.level;
    }
    
    void GetStreak()
    {
        StreakTxt.text = "Streak: " + PlayerDataHolder.CurrentPlayer.login_streak;
    }
}
