using UnityEngine;

public class ReturnGame : MonoBehaviour
{
    private void Start()
    {
        GameManager.isCasthen = false;
        GameManager.isForest = false;
        GameManager.isTouwn = false;
        GameManager.isHouseBandits = false;
        Kattsena.isPlaying = false;
    }
}
