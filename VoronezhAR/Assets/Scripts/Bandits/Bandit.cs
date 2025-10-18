using UnityEngine;
using System.Collections;

public class Bandit : MonoBehaviour
{
    [Header("Bandit Settings")]
    public float runSpeed = 3f;
    public float rotationSpeed = 5f;

    private Animator animator;
    private bool isRunning = false;

    // Параметры аниматора
    private const string RUN_PARAMETER = "isRunning";
    private const string IDLE_PARAMETER = "isIdle";
    private const string ATTACK_PARAMETER = "isAttacking";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Метод для запуска анимации бега (вызывается из AnimalStackManager)
    public void StartRunAnimation()
    {
        if (animator != null)
        {
            // Выключаем все другие анимации
            animator.SetBool(RUN_PARAMETER, true);
            animator.SetBool(IDLE_PARAMETER, false);
            animator.SetBool(ATTACK_PARAMETER, false);

            // Останавливаем все текущие анимации и переходим к бегу
            animator.Play("Run", 0, 0f);
        }
    }

    // Метод для сброса анимации (вызывается при перезапуске игры)
    public void ResetAnimation()
    {
        if (animator != null)
        {
            // Возвращаем в idle состояние
            animator.SetBool(RUN_PARAMETER, false);
            animator.SetBool(IDLE_PARAMETER, true);
            animator.SetBool(ATTACK_PARAMETER, false);

            // Воспроизводим анимацию покоя
            animator.Play("Idle", 0, 0f);
        }
    }

    public void RunAway(Transform targetPoint)
    {
        if (!isRunning)
        {
            StartCoroutine(RunAwayCoroutine(targetPoint));
        }
    }

    private System.Collections.IEnumerator RunAwayCoroutine(Transform targetPoint)
    {
        isRunning = true;

        // Убеждаемся, что анимация бега запущена
        StartRunAnimation();

        Vector3 direction = (targetPoint.position - transform.position).normalized;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        // Двигаемся к точке
        while (Vector3.Distance(transform.position, targetPoint.position) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, runSpeed * Time.deltaTime);
            yield return null;
        }

        // Останавливаем анимацию и скрываем бандита
        if (animator != null)
        {
            animator.SetBool(RUN_PARAMETER, false);
        }
        gameObject.SetActive(false);

        isRunning = false;
    }
}