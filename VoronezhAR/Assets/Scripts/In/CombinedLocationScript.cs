using UnityEngine;
using TMPro;

public class CombinedLocationScript : MonoBehaviour
{
    // Forest variables
    [SerializeField] private GameObject buttonForest;
    [SerializeField] private TextMeshProUGUI textForest;
    [SerializeField] private string _textTrue;
    [SerializeField] private string _textFalse;

    // House variables
    [SerializeField] private GameObject buttonHouse;
    [SerializeField] private TextMeshProUGUI textHouse;
    [SerializeField] private string texttTrue;
    [SerializeField] private string textFalse;

    // Town variables
    [SerializeField] private GameObject buttonTouwn;
    [SerializeField] private TextMeshProUGUI textTouwn;
    [SerializeField] private string texttTrueTown;
    [SerializeField] private string texttTrueTownAndHouse;
    [SerializeField] private string textFalseTown;

    public static CombinedLocationScript instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
    private void Start()
    {
        Check();
    }

    public void Check()
    {
        // Forest logic
        if (GameManager.isCasthen)
        {
            buttonForest.SetActive(true);
            textForest.text = _textTrue;
        }
        else
        {
            buttonForest.SetActive(false);
            textForest.text = _textFalse;
        }

        // House logic
        if (GameManager.isTouwn)
        {
            buttonHouse.SetActive(true);
            textHouse.text = texttTrue;
        }
        else
        {
            buttonHouse.SetActive(false);
            textHouse.text = textFalse;
        }

        // Town logic
        if (GameManager.isForest)
        {
            buttonTouwn.SetActive(true);
            textTouwn.text = texttTrueTown;
        }
        else if (GameManager.isTouwn || GameManager.isHouseBandits)
        {
            buttonTouwn.SetActive(true);
            textTouwn.text = texttTrueTownAndHouse;
        }
        else
        {
            buttonTouwn.SetActive(false);
            textTouwn.text = textFalseTown;
        }
    }
}