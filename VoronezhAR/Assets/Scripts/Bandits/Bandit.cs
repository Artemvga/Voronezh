using UnityEngine;
using System.Collections;

public class Bandit : MonoBehaviour
{
    [Header("Bandit Settings")]
    public float runSpeed = 3f;
    public float rotationSpeed = 5f;

    private Animator animator;
    private bool isRunning = false;

    // ��������� ���������
    private const string RUN_PARAMETER = "isRunning";
    private const string IDLE_PARAMETER = "isIdle";
    private const string ATTACK_PARAMETER = "isAttacking";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // ����� ��� ������� �������� ���� (���������� �� AnimalStackManager)
    public void StartRunAnimation()
    {
        if (animator != null)
        {
            // ��������� ��� ������ ��������
            animator.SetBool(RUN_PARAMETER, true);
            animator.SetBool(IDLE_PARAMETER, false);
            animator.SetBool(ATTACK_PARAMETER, false);

            // ������������� ��� ������� �������� � ��������� � ����
            animator.Play("Run", 0, 0f);
        }
    }

    // ����� ��� ������ �������� (���������� ��� ����������� ����)
    public void ResetAnimation()
    {
        if (animator != null)
        {
            // ���������� � idle ���������
            animator.SetBool(RUN_PARAMETER, false);
            animator.SetBool(IDLE_PARAMETER, true);
            animator.SetBool(ATTACK_PARAMETER, false);

            // ������������� �������� �����
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

        // ����������, ��� �������� ���� ��������
        StartRunAnimation();

        Vector3 direction = (targetPoint.position - transform.position).normalized;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        // ��������� � �����
        while (Vector3.Distance(transform.position, targetPoint.position) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, runSpeed * Time.deltaTime);
            yield return null;
        }

        // ������������� �������� � �������� �������
        if (animator != null)
        {
            animator.SetBool(RUN_PARAMETER, false);
        }
        gameObject.SetActive(false);

        isRunning = false;
    }
}