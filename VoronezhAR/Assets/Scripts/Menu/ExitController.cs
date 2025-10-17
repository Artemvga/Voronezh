using UnityEngine;
using UnityEngine.UI;

public class ExitController : MonoBehaviour
{
    public Button buttonExit;

    public void ExitGame()
    {
        Application.Quit();
    }
}
