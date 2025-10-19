using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Button continueButton;
    public Button acceptButton;
    public Button menuButton;
    public Image dialogueImage;

    [Header("Object References")]
    public GameObject cityCrowd;
    public GameObject finalPanel;
    public TextMeshProUGUI finalText;
    public Button exitButton;

    [Header("Joystick Reference")]
    public GameObject joystick;

    [Header("Dialogue Images")]
    public Sprite resident1Sprite;
    public Sprite resident2Sprite;
    public Sprite donkeySprite;
    public Sprite dogSprite;
    public Sprite catSprite;
    public Sprite roosterSprite;

    private int currentDialogueIndex = 0;
    private bool isInTrigger = false;
    private bool questAccepted = false;
    private GameObject playerObject;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    public static bool AreBanditsDefeated { get; set; } = false;

    private string[] dialogueParts = new string[]
    {
        "Житель 1: Вчера снова приходили бандиты... Они становятся все наглее с каждым днем! Уже совсем обнаглели!",
        "Житель 2: Да, они уже надоели, сил моих нет! Ничего нам не оставляют, все забирают и к себе в хижину уносят... Просто кошмар!",
        "Житель 1: В какую хижину? Ты что-то знаешь о их убежище?",
        "Житель 2: Ну знаешь её, которая на окраине леса к югу от города? Такая старая, покосившаяся, среди самых мрачных деревьев... Там они и прячутся.",

        "*В это время к жителям подходят Осел, Петух, Кошка и Собака*",
        "Осел: Не печальтесь, добрые люди! Мы слышали о ваших бедах и готовы помочь!",
        "Собака: Да! Мы избавим вас от этих бандитов раз и навсегда!",
        "Кошка: Мяу! Расскажите нам о этих негодяях поподробнее...",
        "Петух: Ку-ка-ре-ку! Мы не оставим от них и мокрого места!",

        "Житель 1: О, спасибо вам, друзья! Вы настоящие герои!",
        "Житель 2: Знайте, эти бандиты до ужаса боятся громких звуков! Это их главная слабость!",
        "Житель 1: Да-да! Когда они слышат громкие крики или пение, они сразу же разбегаются как тараканы!",

        "Житель 2: И мы придумали отличный план! Нужно построить пирамиду из вас четверых!",
        "Житель 1: Именно! Поставьте самого крепкого и сильного вниз, чтобы он выдержал вес остальных!",
        "Житель 2: Затем на него - второго по силе, потом третьего, а самого громкого - на самый верх!",
        "Житель 1: Тогда ваш крик будет разноситься далеко вокруг, и бандиты точно испугаются!",
        "Житель 2: Так вы сможете добраться до окна их хижины и напугать их своим концертом!",

        "Осел: Понятно! Значит, нужно расставить нас в правильном порядке по силе и громкости...",
        "Собака: Интересная задача! Нам нужно хорошо подумать над этим!",
        "Кошка: Мяу... Действительно, важно выбрать правильную последовательность!",
        "Петух: Ку-ка-ре-ку! Я готов занять свое место!",

        "Житель 1: Удачи вам, друзья! Расставьте себя правильно и прогоните бандитов!",
        "Житель 2: Наш город рассчитывает на вашу смекалку и отвагу!",
        "Житель 1: Верните нам спокойную жизнь! Мы будем вечно благодарны!"
    };

    private void Start()
    {
        dialoguePanel.SetActive(false);
        menuButton.gameObject.SetActive(false);
        if (finalPanel != null) finalPanel.SetActive(false);

        if (cityCrowd != null)
            cityCrowd.SetActive(AreBanditsDefeated);

        if (AreBanditsDefeated)
        {
            ShowFinalScene();
        }

        continueButton.onClick.AddListener(ContinueDialogue);
        acceptButton.onClick.AddListener(AcceptQuest);

        acceptButton.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isInTrigger && !AreBanditsDefeated && !questAccepted)
        {
            isInTrigger = true;
            playerObject = other.gameObject;

            // Получаем компоненты для остановки движения
            playerRigidbody = playerObject.GetComponent<Rigidbody>();
            playerAnimator = playerObject.GetComponent<Animator>();

            StopAllMovement();

            // Отключаем джойстик при активации диалога
            DisableJoystick();

            // Активируем панель диалога
            dialoguePanel.SetActive(true);
            currentDialogueIndex = 0;
            ShowDialoguePart(currentDialogueIndex);
        }
    }

    private void StopAllMovement()
    {
        // Останавливаем Rigidbody
        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
            playerRigidbody.isKinematic = true; // Полностью останавливаем физику
        }

        // Останавливаем анимации
        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Speed", 0f);
            playerAnimator.enabled = false; // Полностью отключаем аниматор
        }

        // Отключаем скрипты движения
        MonoBehaviour[] scripts = playerObject.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this && script.enabled)
            {
                // Отключаем скрипты, которые могут управлять движением
                if (script.GetType().Name.Contains("Movement") ||
                    script.GetType().Name.Contains("Controller") ||
                    script.GetType().Name.Contains("Player") && !script.GetType().Name.Contains("Dialogue"))
                {
                    script.enabled = false;
                }
            }
        }
    }

    private void ResumeAllMovement()
    {
        // Восстанавливаем Rigidbody
        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = false;
        }

        // Восстанавливаем анимации
        if (playerAnimator != null)
        {
            playerAnimator.enabled = true;
        }

        // Включаем скрипты движения обратно
        MonoBehaviour[] scripts = playerObject.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this && !script.enabled)
            {
                if (script.GetType().Name.Contains("Movement") ||
                    script.GetType().Name.Contains("Controller") ||
                    script.GetType().Name.Contains("Player") && !script.GetType().Name.Contains("Dialogue"))
                {
                    script.enabled = true;
                }
            }
        }
    }

    // Метод для отключения джойстика
    private void DisableJoystick()
    {
        if (joystick != null)
        {
            joystick.SetActive(false);
        }
    }

    // Метод для включения джойстика
    private void EnableJoystick()
    {
        if (joystick != null)
        {
            joystick.SetActive(true);
        }
    }

    private void ShowDialoguePart(int index)
    {
        if (index < dialogueParts.Length)
        {
            dialogueText.text = dialogueParts[index];
            UpdateDialogueImage(index);

            if (index >= dialogueParts.Length - 3)
            {
                continueButton.gameObject.SetActive(false);
                acceptButton.gameObject.SetActive(true);
            }
            else
            {
                continueButton.gameObject.SetActive(true);
                acceptButton.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateDialogueImage(int index)
    {
        if (dialogueImage != null)
        {
            string dialogueText = dialogueParts[index];

            // Определяем какое изображение показывать в зависимости от говорящего
            if (dialogueText.Contains("*") && dialogueText.Contains("подходят"))
            {
                // Событие - животные подходят, скрываем изображение
                dialogueImage.gameObject.SetActive(false);
            }
            else if (dialogueText.StartsWith("Житель 1:"))
            {
                dialogueImage.gameObject.SetActive(true);
                dialogueImage.sprite = resident1Sprite;
            }
            else if (dialogueText.StartsWith("Житель 2:"))
            {
                dialogueImage.gameObject.SetActive(true);
                dialogueImage.sprite = resident2Sprite;
            }
            else if (dialogueText.StartsWith("Осел:"))
            {
                dialogueImage.gameObject.SetActive(true);
                dialogueImage.sprite = donkeySprite;
            }
            else if (dialogueText.StartsWith("Собака:"))
            {
                dialogueImage.gameObject.SetActive(true);
                dialogueImage.sprite = dogSprite;
            }
            else if (dialogueText.StartsWith("Кошка:"))
            {
                dialogueImage.gameObject.SetActive(true);
                dialogueImage.sprite = catSprite;
            }
            else if (dialogueText.StartsWith("Петух:"))
            {
                dialogueImage.gameObject.SetActive(true);
                dialogueImage.sprite = roosterSprite;
            }
            else
            {
                // Для любых других реплик скрываем изображение
                dialogueImage.gameObject.SetActive(false);
            }
        }
    }

    private void ContinueDialogue()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex < dialogueParts.Length)
        {
            ShowDialoguePart(currentDialogueIndex);
        }
        else
        {
            AcceptQuest();
        }
    }

    private void AcceptQuest()
    {
        // Устанавливаем флаг принятия квеста
        questAccepted = true;

        dialoguePanel.SetActive(false);
        menuButton.gameObject.SetActive(true);

        // Восстанавливаем движение
        if (isInTrigger)
        {
            ResumeAllMovement();
            isInTrigger = false;
        }

        // Включаем джойстик обратно только если не активированы другие панели
        if (!menuButton.gameObject.activeInHierarchy && (finalPanel == null || !finalPanel.activeInHierarchy))
        {
            EnableJoystick();
        }
        GameManager.isTouwn = true;
    }

    private void ShowFinalScene()
    {
        if (cityCrowd != null)
            cityCrowd.SetActive(true);

        // Воспроизводим звук толпы при появлении толпы
        PlayCrowdSound();

        Invoke(nameof(ShowFinalPanel), 7f);
    }

    // Метод для воспроизведения звука толпы
    private void PlayCrowdSound()
    {
        if (Sounds.instance != null)
        {
            Sounds.instance.PlaySound(0, 1f); // Индекс 0, громкость 1f
        }
        else
        {
            Debug.LogWarning("Sounds instance not found!");
        }
    }

    private void ShowFinalPanel()
    {
        if (finalPanel != null)
        {
            // Отключаем джойстик при показе финальной панели
            DisableJoystick();

            finalPanel.SetActive(true);

            if (finalText != null)
            {
                finalText.text = "Поздравляем! Вы успешно завершили игру! " +
                               "Бандиты побеждены, город спасен! " +
                               "Теперь вы можете отыграть праздничный концерт " +
                               "в честь победы над бандитами! " +
                               "Спасибо за игру! ";
            }
        }
    }

    // Метод для кнопки меню (если она активирует какую-то панель)
    public void OnMenuButtonClicked()
    {
        // Отключаем джойстик при активации меню
        DisableJoystick();

        // Здесь добавьте код для показа меню панели
    }

    // Метод для закрытия меню (если есть)
    public void OnMenuCloseButtonClicked()
    {
        // Включаем джойстик при закрытии меню, если нет других активных панелей
        if (!dialoguePanel.activeInHierarchy && (finalPanel == null || !finalPanel.activeInHierarchy))
        {
            EnableJoystick();
        }
    }

    public void OnExitButtonClicked()
    {
        Debug.Log("Выход из игры");
    }
}