using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryManager : MonoBehaviour
{
    public void Retry()
    {
        SceneManager.LoadScene("Quiz");
    }
    
    public void Quit()
    {
        SceneManager.LoadScene("Main Menu");
    }
}