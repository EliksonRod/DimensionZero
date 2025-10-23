using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject PreviousUI;
    [SerializeField] GameObject NextUI;
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void OpenNewUI()
    {
        PreviousUI.SetActive(false);
        NextUI.SetActive(true);
    }
    public void CloseGame()
    {
        Application.Quit();
    }
}
