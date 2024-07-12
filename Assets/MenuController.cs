using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("level 1");
    }

    public void ExitGame()
    {
        Application.Quit();

        Debug.Log("Game is exiting");
    }
}
