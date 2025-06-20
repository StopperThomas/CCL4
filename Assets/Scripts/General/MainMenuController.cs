using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene Management")]
    public string sceneToLoad = "RoomSquishy";
    public string mainMenuScene = "Main Menu";

    [Header("UI Panels")]
    public GameObject infoPanel;

    public void StartGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ShowInfo()
    {
        infoPanel.SetActive(true);
    }

    public void CloseInfo()
    {
        infoPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}