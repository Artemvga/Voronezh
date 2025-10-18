using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class AnimalRiddleGame : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject riddlePanel;
    public TextMeshProUGUI riddleText;
    public Button[] answerButtons;
    public TextMeshProUGUI scoreText;
    public Image animalImage;

    [Header("Animal Images")]
    public Sprite catSprite;
    public Sprite dogSprite;
    public Sprite cockSprite;

    [Header("Animals")]
    public GameObject catObject;
    public GameObject dogObject;
    public GameObject cockObject;

    [Header("End Game")]
    public GameObject endTrigger;
    public GameObject exitPanel;
    public TextMeshProUGUI exitMessageText;
    public Button exitSuccessButton;
    public Button exitFailButton;

    [Header("Correction Quest")]
    public GameObject correctionQuestPanel;
    public Button tapCircleButton;
    public TextMeshProUGUI tapCounterText;
    public Button correctionCompleteButton;
    public TextMeshProUGUI correctionTitleText;
    public TextMeshProUGUI correctionDescriptionText;

    [Header("Player Controls")]
    public GameObject joystick;

    private int score = 0;
    private int requiredScore = 6;
    private int currentTaps = 0;
    private int requiredTaps = 50;
    private bool correctionQuestCompleted = false;

    // Компоненты для управления движением
    private Rigidbody playerRigidbody;
    private CharacterController playerCharacterController;
    private MonoBehaviour[] movementScripts;

    // Список использованных вопросов
    private List<int> usedQuestionIndices = new List<int>();

    // База данных из 20 вопросов с подробными вопросами и короткими ответами
    private List<Riddle> grimmRiddles = new List<Riddle>
    {
        new Riddle
        {
            question = "В каком году в немецком городе Ханау родился старший из братьев Гримм - Якоб?",
            correctAnswer = "1785",
            wrongAnswers = new List<string> {"1790", "1780", "1775"}
        },
        new Riddle
        {
            question = "Сколько полных лет прожил Вильгельм Гримм, младший из знаменитых братьев-сказочников?",
            correctAnswer = "73",
            wrongAnswers = new List<string> {"68", "75", "70"}
        },
        new Riddle
        {
            question = "В каком году свет увидел первый том знаменитого сборника 'Детские и семейные сказки' братьев Гримм?",
            correctAnswer = "1812",
            wrongAnswers = new List<string> {"1808", "1815", "1820"}
        },
        new Riddle
        {
            question = "На сколько лет Якоб Гримм был старше своего брата Вильгельма?",
            correctAnswer = "1",
            wrongAnswers = new List<string> {"2", "3", "4"}
        },
        new Riddle
        {
            question = "Сколько детей оставил после себя Вильгельм Гримм, продолжив свой род?",
            correctAnswer = "3",
            wrongAnswers = new List<string> {"2", "4", "1"}
        },
        new Riddle
        {
            question = "Сколько объемных томов планировалось создать в монументальном 'Немецком словаре' братьев Гримм?",
            correctAnswer = "32",
            wrongAnswers = new List<string> {"20", "25", "30"}
        },
        new Riddle
        {
            question = "Сколько различных изданий их знаменитого сборника сказок было опубликовано при жизни братьев?",
            correctAnswer = "7",
            wrongAnswers = new List<string> {"5", "10", "3"}
        },
        new Riddle
        {
            question = "В каком возрасте Якоб Гримм начал углубленно изучать юриспруденцию в Марбургском университете?",
            correctAnswer = "20",
            wrongAnswers = new List<string> {"18", "22", "25"}
        },
        new Riddle
        {
            question = "Сколько лет было Якобу Гримму, когда он начал свою преподавательскую карьеру в Берлинском университете?",
            correctAnswer = "30",
            wrongAnswers = new List<string> {"25", "35", "40"}
        },
        new Riddle
        {
            question = "В каком году ушел из жизни Вильгельм Гримм, оставив богатое литературное наследие?",
            correctAnswer = "1859",
            wrongAnswers = new List<string> {"1855", "1860", "1865"}
        },
        new Riddle
        {
            question = "В каком году завершил свой земной путь Якоб Гримм, переживший младшего брата на несколько лет?",
            correctAnswer = "1863",
            wrongAnswers = new List<string> {"1860", "1865", "1870"}
        },
        new Riddle
        {
            question = "Сколько уникальных сказочных историй вошло в окончательную версию знаменитого сборника братьев Гримм?",
            correctAnswer = "200",
            wrongAnswers = new List<string> {"100", "150", "250"}
        },
        new Riddle
        {
            question = "В каком году была впервые опубликована всеми любимая сказка 'Бременские музыканты'?",
            correctAnswer = "1819",
            wrongAnswers = new List<string> {"1805", "1825", "1830"}
        },
        new Riddle
        {
            question = "Сколько лет упорного труда посвятили братья Гримм работе над своим 'Немецким словарем'?",
            correctAnswer = "10",
            wrongAnswers = new List<string> {"5", "15", "20"}
        },
        new Riddle
        {
            question = "В каком веке появились на свет будущие великие собиратели немецкого фольклора - братья Гримм?",
            correctAnswer = "18",
            wrongAnswers = new List<string> {"17", "19", "16"}
        },
        new Riddle
        {
            question = "Сколько полных лет жизни было отпущено Якобу Гримму, старшему из знаменитых братьев?",
            correctAnswer = "78",
            wrongAnswers = new List<string> {"75", "80", "73"}
        },
        new Riddle
        {
            question = "Какой общий стаж в годах провели братья Гримм в активном собирании немецкого фольклора и сказок?",
            correctAnswer = "10",
            wrongAnswers = new List<string> {"5", "15", "20"}
        },
        new Riddle
        {
            question = "В каком году братья Гримм приступили к работе над своим грандиозным 'Немецким словарем'?",
            correctAnswer = "1838",
            wrongAnswers = new List<string> {"1840", "1835", "1828"}
        },
        new Riddle
        {
            question = "Сколько лет составляла разница между рождением Якоба и Вильгельма Гримм?",
            correctAnswer = "1",
            wrongAnswers = new List<string> {"2", "3", "4"}
        },
        new Riddle
        {
            question = "В каком году скончался Якоб Гримм, завершив эпоху великих немецких филологов?",
            correctAnswer = "1863",
            wrongAnswers = new List<string> {"1860", "1865", "1870"}
        }
    };

    private List<Riddle> currentRiddles;
    private int currentRiddleIndex = 0;
    private string currentAnimalTag;

    [System.Serializable]
    public class Riddle
    {
        public string question;
        public string correctAnswer;
        public List<string> wrongAnswers;
    }

    void Start()
    {
        // Получаем компоненты движения
        playerRigidbody = GetComponent<Rigidbody>();
        playerCharacterController = GetComponent<CharacterController>();

        // Находим все скрипты движения (кроме этого)
        movementScripts = GetComponents<MonoBehaviour>()
            .Where(script => script != this && script.enabled)
            .ToArray();

        // Скрываем все UI элементы при старте
        riddlePanel.SetActive(false);
        exitPanel.SetActive(false);
        correctionQuestPanel.SetActive(false);

        // Выключаем все кнопки изначально
        exitSuccessButton.gameObject.SetActive(false);
        exitFailButton.gameObject.SetActive(false);
        correctionCompleteButton.gameObject.SetActive(false);

        animalImage.gameObject.SetActive(false);

        // Назначаем обработчики для кнопок ответов
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int buttonIndex = i;
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(buttonIndex));
        }

        // Обработчик для кнопки тапа по кругу
        tapCircleButton.onClick.AddListener(OnCircleTapped);

        // Обработчик для кнопки перехода к исправительному квесту
        exitFailButton.onClick.AddListener(StartCorrectionQuest);

        UpdateScoreUI();

        Debug.Log($"Всего вопросов в базе: {grimmRiddles.Count}");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cat") || other.CompareTag("Dog") || other.CompareTag("Cock"))
        {
            currentAnimalTag = other.tag;
            StartRiddleSession();
        }
        else if (other.CompareTag("EndTrigger"))
        {
            ShowExitPanel();
        }
    }

    void ShowExitPanel()
    {
        // Останавливаем движение при показе панели выхода
        StopPlayerMovement();

        // Выключаем джойстик
        if (joystick != null)
            joystick.SetActive(false);

        // Показываем панель выхода
        exitPanel.SetActive(true);
        exitMessageText.text = "О, странник... Твой путь еще не завершен. Не все тайны братьев Гримм раскрыты, не все спутники обретены.";

        // Проверяем условия для активации кнопок
        if (score >= requiredScore)
        {
            // Все ответы верны - активируем кнопку успеха
            exitSuccessButton.gameObject.SetActive(true);
            exitFailButton.gameObject.SetActive(false);
            exitMessageText.text = "Поздравляю, путник! Ты обрел верных спутников и постиг мудрость сказок братьев Гримм. Теперь ваш ансамбль готов покорить Бремен!";
            GameManager.isForest = true;
        }
        else
        {
            // Не все ответы верны - активируем кнопку неудачи
            exitSuccessButton.gameObject.SetActive(false);
            exitFailButton.gameObject.SetActive(true);
            exitMessageText.text = "О, странник... Твой путь еще не завершен. Не все тайны братьев Гримм раскрыты, не все спутники обретены. Чтобы продолжить путь, нажми кнопку ниже.";
        }
    }

    void StartCorrectionQuest()
    {
        // Скрываем панель выхода и показываем исправительный квест
        exitPanel.SetActive(false);
        correctionQuestPanel.SetActive(true);

        // Устанавливаем текст для исправительного квеста
        SetCorrectionQuestText();

        // Сбрасываем счетчик тапов (если квест еще не завершен)
        if (!correctionQuestCompleted)
        {
            currentTaps = 0;
        }
        UpdateTapCounter();

        // Выключаем кнопку завершения (включится после 50 тапов)
        if (!correctionQuestCompleted)
        {
            correctionCompleteButton.gameObject.SetActive(false);
        }
        else
        {
            correctionCompleteButton.gameObject.SetActive(true);
            correctionDescriptionText.text = "Магический круг уже наполнен силой! Ты доказал свою мудрость и готовность к путешествию. Теперь путь открыт!";
        }
    }

    void SetCorrectionQuestText()
    {
        // Устанавливаем заголовок и описание исправительного квеста
        correctionTitleText.text = "Испытание Мудрости";
        if (!correctionQuestCompleted)
        {
            correctionDescriptionText.text = "Чтобы доказать свою готовность к путешествию, коснись магического круга 50 раз.\n\nКаждое прикосновение наполняет его силой древних сказок, открывая путь к новым знаниям и приключениям.";
        }
    }

    void StartRiddleSession()
    {
        // Останавливаем движение игрока
        StopPlayerMovement();

        // Выключаем джойстик при открытии панели загадок
        if (joystick != null)
            joystick.SetActive(false);

        // Устанавливаем изображение животного
        SetAnimalImage();

        // Выбираем 2 случайные загадки из НЕИСПОЛЬЗОВАННЫХ вопросов
        currentRiddles = GetRandomUnusedRiddles(2);

        // Если не хватает неиспользованных вопросов, начинаем сначала
        if (currentRiddles.Count < 2)
        {
            Debug.Log("Неиспользованные вопросы закончились, начинаем заново");
            usedQuestionIndices.Clear();
            currentRiddles = GetRandomUnusedRiddles(2);
        }

        currentRiddleIndex = 0;
        ShowCurrentRiddle();
        riddlePanel.SetActive(true);
        animalImage.gameObject.SetActive(true);

        Debug.Log($"Выбрано загадок для {currentAnimalTag}: {currentRiddles.Count}");
        Debug.Log($"Осталось неиспользованных вопросов: {grimmRiddles.Count - usedQuestionIndices.Count}");
    }

    void EndRiddleSession()
    {
        // Возобновляем движение игрока
        ResumePlayerMovement();

        // Включаем джойстик обратно после закрытия панели
        if (joystick != null)
            joystick.SetActive(true);
    }

    void StopPlayerMovement()
    {
        // Останавливаем Rigidbody
        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
            playerRigidbody.isKinematic = true;
        }

        // Выключаем CharacterController
        if (playerCharacterController != null)
        {
            playerCharacterController.enabled = false;
        }

        // Выключаем все скрипты движения
        foreach (var script in movementScripts)
        {
            if (script != null)
                script.enabled = false;
        }
    }

    void ResumePlayerMovement()
    {
        // Включаем Rigidbody
        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = false;
        }

        // Включаем CharacterController
        if (playerCharacterController != null)
        {
            playerCharacterController.enabled = true;
        }

        // Включаем все скрипты движения
        foreach (var script in movementScripts)
        {
            if (script != null)
                script.enabled = true;
        }
    }

    void SetAnimalImage()
    {
        switch (currentAnimalTag)
        {
            case "Cat":
                animalImage.sprite = catSprite;
                break;
            case "Dog":
                animalImage.sprite = dogSprite;
                break;
            case "Cock":
                animalImage.sprite = cockSprite;
                break;
        }
    }

    void ShowCurrentRiddle()
    {
        if (currentRiddleIndex < currentRiddles.Count)
        {
            Riddle currentRiddle = currentRiddles[currentRiddleIndex];
            riddleText.text = currentRiddle.question;

            // Создаем список всех ответов (1 правильный + 3 неправильных)
            List<string> allAnswers = new List<string>();
            allAnswers.Add(currentRiddle.correctAnswer);
            allAnswers.AddRange(currentRiddle.wrongAnswers);

            // Перемешиваем ответы
            allAnswers = allAnswers.OrderBy(x => Random.value).ToList();

            // Назначаем текст кнопкам
            for (int i = 0; i < answerButtons.Length && i < allAnswers.Count; i++)
            {
                TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = allAnswers[i];
                }
                else
                {
                    // Если в кнопке обычный Text (для обратной совместимости)
                    Text legacyText = answerButtons[i].GetComponentInChildren<Text>();
                    if (legacyText != null)
                        legacyText.text = allAnswers[i];
                }
            }
        }
    }

    void OnAnswerSelected(int buttonIndex)
    {
        string selectedAnswer = "";

        // Получаем текст из кнопки (для TextMeshPro)
        TextMeshProUGUI buttonText = answerButtons[buttonIndex].GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            selectedAnswer = buttonText.text;
        }
        else
        {
            // Если в кнопке обычный Text (для обратной совместимости)
            Text legacyText = answerButtons[buttonIndex].GetComponentInChildren<Text>();
            if (legacyText != null)
                selectedAnswer = legacyText.text;
        }

        Riddle currentRiddle = currentRiddles[currentRiddleIndex];

        // Проверяем правильность ответа
        bool isCorrect = selectedAnswer == currentRiddle.correctAnswer;

        if (isCorrect)
        {
            // Правильный ответ - начисляем очко
            score++;
            UpdateScoreUI();
            Debug.Log($"Правильно! +1 очко. Текущий счет: {score}");
        }
        else
        {
            Debug.Log($"Неправильно! Выбрано: '{selectedAnswer}', Правильный: '{currentRiddle.correctAnswer}'");
        }

        // Переходим к следующей загадке независимо от правильности ответа
        currentRiddleIndex++;

        if (currentRiddleIndex < currentRiddles.Count)
        {
            // Показываем следующую загадку
            ShowCurrentRiddle();
        }
        else
        {
            // Все загадки отгаданы - помечаем их как использованные
            MarkCurrentRiddlesAsUsed();

            riddlePanel.SetActive(false);
            animalImage.gameObject.SetActive(false);
            EndRiddleSession();
            DeactivateCurrentAnimal();
        }
    }

    void MarkCurrentRiddlesAsUsed()
    {
        foreach (var riddle in currentRiddles)
        {
            int index = grimmRiddles.FindIndex(r => r.question == riddle.question);
            if (index != -1 && !usedQuestionIndices.Contains(index))
            {
                usedQuestionIndices.Add(index);
                Debug.Log($"Вопрос добавлен в использованные: {riddle.question.Substring(0, Mathf.Min(30, riddle.question.Length))}...");
            }
        }
    }

    void DeactivateCurrentAnimal()
    {
        switch (currentAnimalTag)
        {
            case "Cat":
                if (catObject != null) catObject.SetActive(false);
                break;
            case "Dog":
                if (dogObject != null) dogObject.SetActive(false);
                break;
            case "Cock":
                if (cockObject != null) cockObject.SetActive(false);
                break;
        }
    }

    void OnCircleTapped()
    {
        // Если квест уже завершен, не добавляем больше тапов
        if (correctionQuestCompleted)
        {
            Debug.Log("Испытание уже завершено! Максимум 50 тапов.");
            return;
        }

        currentTaps++;

        // Ограничиваем максимальное количество тапов 50
        if (currentTaps > requiredTaps)
        {
            currentTaps = requiredTaps;
        }

        UpdateTapCounter();

        if (currentTaps >= requiredTaps)
        {
            // Завершаем квест и активируем кнопку
            correctionQuestCompleted = true;
            correctionCompleteButton.gameObject.SetActive(true);

            // Обновляем текст при завершении
            correctionDescriptionText.text = "Магический круг наполнен силой! Ты доказал свою мудрость и готовность к путешествию. Теперь путь открыт!";

            Debug.Log("Исправительный квест завершен! Максимум 50 тапов достигнут.");
        }
        else if (currentTaps % 10 == 0) // Обновляем текст каждые 10 тапов
        {
            UpdateCorrectionQuestProgress();
        }

        Debug.Log($"Тапов: {currentTaps}/50");
    }

    void UpdateCorrectionQuestProgress()
    {
        // Обновляем описание в зависимости от прогресса
        if (currentTaps < 10)
        {
            correctionDescriptionText.text = "Коснись магического круга 50 раз. Начинается твое испытание... Каждое прикосновение приближает тебя к цели.";
        }
        else if (currentTaps < 25)
        {
            correctionDescriptionText.text = "Продолжай! Круг начинает светиться. Ты на верном пути к открытию древних знаний братьев Гримм.";
        }
        else if (currentTaps < 40)
        {
            correctionDescriptionText.text = "Сила круга растет! Еще немного усердия, и тайны сказок откроются перед тобой.";
        }
        else
        {
            correctionDescriptionText.text = "Почти у цели! Осталось совсем немного. Круг почти наполнен магической энергией сказок.";
        }
    }

    void UpdateTapCounter()
    {
        tapCounterText.text = $"{currentTaps}/{requiredTaps}";
    }

    void UpdateScoreUI()
    {
        scoreText.text = $"Собрано: {score}/{requiredScore}";
    }

    List<Riddle> GetRandomUnusedRiddles(int count)
    {
        // Создаем список неиспользованных вопросов
        List<Riddle> unusedRiddles = new List<Riddle>();
        for (int i = 0; i < grimmRiddles.Count; i++)
        {
            if (!usedQuestionIndices.Contains(i))
            {
                unusedRiddles.Add(grimmRiddles[i]);
            }
        }

        // Перемешиваем неиспользованные вопросы и берем нужное количество
        List<Riddle> shuffled = unusedRiddles.OrderBy(x => Random.value).ToList();
        List<Riddle> selected = shuffled.Take(count).ToList();

        Debug.Log($"Выбраны неиспользованные загадки: {selected.Count} из {unusedRiddles.Count} доступных");

        return selected;
    }
}