using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Town Square");
    }
    public void BackToTown()
    {
        SceneManager.LoadScene("Town Square");
    }
}