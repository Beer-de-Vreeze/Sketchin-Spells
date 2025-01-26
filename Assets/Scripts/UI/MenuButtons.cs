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
        GameManager.Instance.StartDialogue(
            1,
            SketchType.Player,
            GameManager.Instance.b_Player.b_playerName,
            GameManager.Instance.b_Player.b_playerDescription
        );
        
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
}
