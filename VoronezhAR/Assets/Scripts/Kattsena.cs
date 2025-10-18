using UnityEngine;
using System.Collections;

public class Kattsena : MonoBehaviour
{
    [Header("Настройки звука")]
    public int soundClipIndex = 0;
    public float soundVolume = 1f;

    [Header("Ссылки")]
    public Animator exitHouseAnimator;
    public string animationName = "YourAnimationName";

    public static bool isPlaying = false;
    // Публичный метод для запуска извне
    public void StartSequence()
    {
        if (!isPlaying)
        {
            StartCoroutine(PlaySequence());
        }
    }

    private IEnumerator PlaySequence()
    {
        isPlaying = true;

        // 1. Проигрываем звук
        if (Sounds.instance != null)
        {
            Sounds.instance.PlaySound(soundClipIndex, soundVolume);

            // 2. Ждем завершения звуковой фразы
            AudioClip currentClip = Sounds.instance.sounds[soundClipIndex];
            yield return new WaitForSeconds(currentClip.length);
        }
        else
        {
            Debug.LogError("Sounds instance not found!");
        }

        // 3. Запускаем анимацию объекта "ExitHouse"
        if (exitHouseAnimator != null)
        {
            exitHouseAnimator.Play(animationName);
        }
        else
        {
            // Ищем объект по имени, если ссылка не установлена
            GameObject exitHouse = GameObject.Find("ExitHouse");
            if (exitHouse != null)
            {
                Animator animator = exitHouse.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.Play(animationName);

                }
                else
                {
                    Debug.LogError("Animator component not found on ExitHouse object!");
                }
            }
            else
            {
                Debug.LogError("ExitHouse object not found!");
            }
        }
        GameManager.isCasthen = true;
        CombinedLocationScript.instance.Check();
    }
}