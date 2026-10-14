using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public void ContinueToLevel2()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level2");
    }
}