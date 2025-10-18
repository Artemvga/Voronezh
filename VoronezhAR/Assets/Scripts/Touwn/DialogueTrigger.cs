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
    public Sprite[] dialogueSprites;

    private int currentDialogueIndex = 0;
    private bool isInTrigger = false;

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
        if (other.CompareTag("Player") && !isInTrigger && !AreBanditsDefeated)
        {
            isInTrigger = true;

            // Останавливаем движение через Rigidbody velocity
            if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // Отключаем джойстик
            if (joystick != null)
            {
                joystick.SetActive(false);
            }

            // Активируем панель диалога
            dialoguePanel.SetActive(true);
            currentDialogueIndex = 0;
            ShowDialoguePart(currentDialogueIndex);
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
        if (dialogueImage != null && dialogueSprites != null && dialogueSprites.Length > 0)
        {
            if (index < 4)
            {
                dialogueImage.sprite = dialogueSprites[0];
            }
            else if (index < 9)
            {
                dialogueImage.sprite = dialogueSprites[1];
            }
            else if (index < 12)
            {
                dialogueImage.sprite = dialogueSprites[2];
            }
            else if (index < 17)
            {
                dialogueImage.sprite = dialogueSprites[3];
            }
            else
            {
                dialogueImage.sprite = dialogueSprites[4];
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
        dialoguePanel.SetActive(false);
        menuButton.gameObject.SetActive(true);

        // Включаем джойстик обратно
        if (joystick != null)
        {
            joystick.SetActive(true);
        }
        GameManager.isTouwn = true;
    }

    public void OnBanditsDefeated()
    {
        AreBanditsDefeated = true;
        Invoke(nameof(ShowFinalScene), 3f);
    }

    private void ShowFinalScene()
    {
        if (cityCrowd != null)
            cityCrowd.SetActive(true);

        Invoke(nameof(ShowFinalPanel), 7f);
    }

    private void ShowFinalPanel()
    {
        if (finalPanel != null)
        {
            finalPanel.SetActive(true);

            if (finalText != null)
            {
                finalText.text = "Поздравляем! Вы успешно завершили игру!\n\n" +
                               "Бандиты побеждены, город спасен!\n\n" +
                               "Теперь вы можете отыграть праздничный концерт\n" +
                               "в честь победы над бандитами!\n\n" +
                               "Спасибо за игру!";
            }
        }
    }
}