using UnityEngine;
using UnityEngine.UI;

public class SettingsInGame : MonoBehaviour
{
    public GameObject settingsButton;
    public Button settingsButtonUI;
    public Button continueButtonUI;
    public GameObject panelSettings;
    private void Start()
    {
        settingsButtonUI.onClick.AddListener(() => SettingsButton());
        continueButtonUI.onClick.AddListener(() => ContinueClick());
    }
    public void ContinueClick()
    {
        settingsButton.SetActive(true);
        panelSettings.SetActive(false);
    }

    public void SettingsButton()
    {
        settingsButton.SetActive(false);
        panelSettings.SetActive(true);
    }


}
