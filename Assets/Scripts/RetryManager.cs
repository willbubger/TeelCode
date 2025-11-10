using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryManager : MonoBehaviour
{
    public void Retry()
    {
        SceneManager.LoadScene("Quiz");
    }
    
    public void ReturnTown()
    {
        SceneManager.LoadScene("Town Square");
    }

    public void Quit()
    {
        SceneManager.LoadScene("Main Menu");
    }
}