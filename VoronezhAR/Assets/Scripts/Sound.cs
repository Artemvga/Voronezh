using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip[] sounds;
    private AudioSource audioScr => GetComponent<AudioSource>();
    public static Sounds instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySound(int clip, float value)
    {
        audioScr.PlayOneShot(sounds[clip], value);
    }
}   