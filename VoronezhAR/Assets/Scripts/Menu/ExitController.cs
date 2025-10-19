using UnityEngine;
using UnityEngine.UI;

public class ExitController : MonoBehaviour
{
    public Button buttonExit;

    private void Start()
    {
        buttonExit.onClick.AddListener(() => ExitGame());
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
