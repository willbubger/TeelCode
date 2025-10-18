using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Answers : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager quizManager;
    public void Answer()
    {
        if(isCorrect)
        {
            Debug.Log("Correct Answer");
            quizManager.Correct();
        }
        else
        {
            quizManager.livesLeft--;
            if(quizManager.livesLeft > 0)
            {
                Debug.Log("Wrong Answer, you lost a life!" + " Lives left: " + quizManager.livesLeft);
            } else
            {
                quizManager.backgroundMusic.SetActive(false);
                quizManager.gameOverPanel.SetActive(true);
            }
        }
    }
}
