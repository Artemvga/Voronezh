using UnityEngine;

public class PreparationScene : MonoBehaviour
{
    public GameObject[] activationItems;
    public GameObject[] deactivationItems;

    private void Start()
    {
        for (int i = 0; i < activationItems.Length; i++)
        {
            activationItems[i].SetActive(true);
        }
        for (int i = 0; i < deactivationItems.Length; i++)
        {
            deactivationItems[i].SetActive(false);
        }
    }
}
