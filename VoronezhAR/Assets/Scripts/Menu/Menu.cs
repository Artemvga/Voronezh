using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject menuUI;
    public GameObject mainMenuUI;
    public GameObject settingsUI;
    public GameObject loadUI;
    public GameObject canvasUI;
    public Button buttonExit;

    private void Awake()
    {
        buttonExit.onClick.AddListener(() => ExitGame());
    }

    private void Start()
    {
        if (!canvasUI.activeInHierarchy)
        {
            canvasUI.SetActive(true);
        }
        if (!mainMenuUI.activeInHierarchy)
        {
            mainMenuUI.SetActive(true);
        }
        if (!menuUI.activeInHierarchy)
        {
            menuUI.SetActive(true);
        }
        if (settingsUI.activeInHierarchy)
        {
            settingsUI.SetActive(false);
        }
        if(loadUI.activeInHierarchy)
        {
            loadUI.SetActive(false);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
