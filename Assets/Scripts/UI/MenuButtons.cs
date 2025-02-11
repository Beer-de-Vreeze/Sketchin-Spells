using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    public void StartGame()
    {
        if (GameManager.Instance == null || UIManager.Instance == null)
        {
            Debug.LogError("Required manager instances are null");
            return;
        }

        UIManager.Instance.CloseMenuCanvas();
        UIManager.Instance.OpenGameCanvas();
        GameManager.Instance.StartCoroutine(GameManager.Instance.StartGameSequence());
    }

    public void RestartGame()
    {
        GameManager.Instance.ResetGame();
    }

    public void QuitGame()
    {
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
        Debug.Log(
            this.name
                + " : "
                + this.GetType()
                + " : "
                + System.Reflection.MethodBase.GetCurrentMethod().Name
        );
#endif
#if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE)
        Application.Quit();
#elif (UNITY_WEBGL)
        Application.OpenURL("itch url ");
#endif
    }

    private void OnApplicationQuit()
    {
        GameManager.Instance.ResetGame();
    }
}
