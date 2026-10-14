using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    public void GoToHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Home");
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1");
    }
}