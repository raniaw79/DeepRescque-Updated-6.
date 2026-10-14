using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButtons : MonoBehaviour
{
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1");
    }
}