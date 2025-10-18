using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class AnimalStackManager : MonoBehaviour
{
    [Header("Animal Buttons")]
    public Button donkeyButton;
    public Button dogButton;
    public Button catButton;
    public Button roosterButton;

    [Header("Bandits")]
    public Bandit[] bandits;
    public Transform[] banditRunAwayPoints;

    [Header("UI - Main Elements")]
    public GameObject resultPanel;
    public TextMeshProUGUI resultTitle;
    public TextMeshProUGUI resultMessage;
    public Button exitButton;
    public Button retryButton;

    [Header("UI - Facts Panel")]
    public GameObject factsPanel;
    public TextMeshProUGUI factText;
    public TextMeshProUGUI factCounter;
    public Button nextFactButton;
    public Button restartButton;
    public GameObject completionMessage;
    public TextMeshProUGUI completionText;

    [Header("Animal Models")]
    public GameObject animalStackModel;

    [Header("Settings")]
    public float banditRunDelay = 2f;

    // ������ ������ � ������
    private List<string> theaterFacts = new List<string>()
    {
        "����� ������� � 1925 ���� � �������� ����� �� ��������� ��������� ������� ������.",
        "� 1984 ���� ����� ����� ��� ������� ���������� ������������ - ����������� ��������-����������.",
        "������ ������ ����������� � ������������ ������ �������� �� ��������� ���������, 50.",
        "� ������ �������� ���������� ����� �����, ��� �������� ����� 1000 ���������� ������ ����.",
        "����� �������� ������������ ��������� ������������ ����������� ������ �������� �����.",
        "�������� ����� �������� ������������� ��������� ������� ����� ������������ ��������� ���.",
        "� ���������� ������ ����� 50 ���������� ��� ����� � ��������.",
        "����� ������������� � ��������, �������, ������, �����, �������� � ������ �������.",
        "���������� ������ - ������� ���������� ���������, ��������� ���������� ���������.",
        "� 1970-� ���� ����� ���������� �������� ������ ������ ������ ����������� ������.",
        "����� ����� ����������� ���������� ���������� �������� ����� �� �����-����.",
        "��������� ���������� ����� �������� ���� �� ����� ������ ����� 30 ���.",
        "� ������ �������� ��� �����: ��������, ����� � ��������.",
        "�������������� ������������� ������ �������� ����������� ������ ������.",
        "����� ������� ������������ � ������������ �������, ������� ��������������� ���������.",
        "� 2015 ���� ����� ������� 90-������ ������ ���������� �������������� ������.",
        "����� ������ � ������ �������� ��������� ��������� �� ������ ������� � ���������.",
        "� ������ ������ �������� ������� ������ ��������� - �� ��������� �� ������� �����������.",
        "����� �������� ����������������� ������ ��� ����� �� ���������������� �����.",
        "���������� ������� �������� ����� ��������� ��������� ������� ��������� �������������.",
        "����� ������ ����������� ������ ���������� ������� � ����� ������.",
        "� 2020 ���� ����� ������� �� ������-���������� ���������� �� ����� ��������.",
        "����� ������������ � ����������� ��������������� ���������� ��������.",
        "�������� ����� �������� ����� 80 000 ��������.",
        "����� ����� ����������� ������ ��� ���������� ������� ��������-����������."
    };

    private enum AnimalType { Donkey, Dog, Cat, Rooster }
    private AnimalType[] currentOrder = new AnimalType[4];
    private int currentPositionIndex = 0;
    private bool isCompleted = false;

    // ���������� ��� ������ ������
    private int currentFactIndex = 0;
    private int factsViewed = 0;
    private const int FACTS_NEEDED = 5;
    private List<int> usedFactIndexes = new List<int>();

    private void Start()
    {
        donkeyButton.onClick.AddListener(() => PlaceAnimal(AnimalType.Donkey));
        dogButton.onClick.AddListener(() => PlaceAnimal(AnimalType.Dog));
        catButton.onClick.AddListener(() => PlaceAnimal(AnimalType.Cat));
        roosterButton.onClick.AddListener(() => PlaceAnimal(AnimalType.Rooster));

        exitButton.onClick.AddListener(OnExitButtonClicked);
        retryButton.onClick.AddListener(OnRetryButtonClicked);
        nextFactButton.onClick.AddListener(ShowNextFact);
        restartButton.onClick.AddListener(OnRestartButtonClicked);

        InitializeGame();
    }

    private void InitializeGame()
    {
        resultPanel.SetActive(false);
        factsPanel.SetActive(false);
        exitButton.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

        if (animalStackModel != null)
            animalStackModel.SetActive(false);

        currentPositionIndex = 0;
        isCompleted = false;

        SetAnimalButtonsActive(true);
        ResetButtonPositions();

        ResetFactsState();

        // ���������� �������� �������� ��� ������������� ����
        ResetBanditsAnimations();
    }

    private void ResetFactsState()
    {
        factsViewed = 0;
        currentFactIndex = 0;
        usedFactIndexes.Clear();
    }

    private void PlaceAnimal(AnimalType animal)
    {
        if (currentPositionIndex >= 4 || isCompleted) return;

        currentOrder[currentPositionIndex] = animal;
        currentPositionIndex++;

        if (currentPositionIndex == 4)
        {
            CheckOrder();
        }
    }

    private void CheckOrder()
    {
        bool isCorrect = currentOrder[0] == AnimalType.Donkey &&
                        currentOrder[1] == AnimalType.Dog &&
                        currentOrder[2] == AnimalType.Cat &&
                        currentOrder[3] == AnimalType.Rooster;

        if (isCorrect)
        {
            SuccessSequence();
        }
        else
        {
            FailSequence();
        }
    }

    private void SuccessSequence()
    {
        isCompleted = true;
        SetAnimalButtonsActive(false);

        if (animalStackModel != null)
            animalStackModel.SetActive(true);

        Sounds.instance.PlaySound(0, 1f);

        // ��������� �������� ���� � �������� ��� ������
        StartBanditsRunAnimation();

        StartCoroutine(RunBanditsAway());
    }

    private void FailSequence()
    {
        SetAnimalButtonsActive(false);
        ShowResultPanel(false);
        ResetButtonPositions();
    }

    private System.Collections.IEnumerator RunBanditsAway()
    {
        for (int i = 0; i < bandits.Length; i++)
        {
            if (i < banditRunAwayPoints.Length)
            {
                bandits[i].RunAway(banditRunAwayPoints[i]);
            }
        }

        yield return new WaitForSeconds(banditRunDelay);

        ShowResultPanel(true);
        DialogueTrigger.AreBanditsDefeated = true;
    }

    private void ShowResultPanel(bool isVictory)
    {
        resultPanel.SetActive(true);

        if (isVictory)
        {
            resultTitle.text = "������!";
            resultMessage.text = "�� �������� ��������! ������������� � �����.";
            exitButton.gameObject.SetActive(true);
            retryButton.gameObject.SetActive(false);
        }
        else
        {
            resultTitle.text = "�����������!";
            resultMessage.text = "������� �� ����������. ������� ������ � ����������� ������ �����!";
            exitButton.gameObject.SetActive(false);
            retryButton.gameObject.SetActive(true);
        }

        ForceRefreshTextMeshPro(resultTitle);
        ForceRefreshTextMeshPro(resultMessage);
    }

    private void OnRetryButtonClicked()
    {
        resultPanel.SetActive(false);
        ShowFactsPanel();
    }

    private void ShowFactsPanel()
    {
        factsPanel.SetActive(true);

        ResetFactsState();

        completionMessage.SetActive(false);
        nextFactButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);
        nextFactButton.interactable = true;

        ShowFirstFact();
        UpdateFactCounter();

        ForceRefreshTextMeshPro(factText);
        ForceRefreshTextMeshPro(factCounter);
    }

    private void ShowFirstFact()
    {
        currentFactIndex = GetRandomUnusedFactIndex();
        usedFactIndexes.Add(currentFactIndex);
        factsViewed = 1;

        ShowCurrentFact();
        UpdateFactCounter();
    }

    private void ShowNextFact()
    {
        if (factsViewed >= FACTS_NEEDED) return;

        currentFactIndex = GetRandomUnusedFactIndex();
        usedFactIndexes.Add(currentFactIndex);
        factsViewed++;

        ShowCurrentFact();
        UpdateFactCounter();

        if (factsViewed >= FACTS_NEEDED)
        {
            nextFactButton.gameObject.SetActive(false);
            restartButton.gameObject.SetActive(true);
            completionMessage.SetActive(true);

            ForceRefreshTextMeshPro(factText);
            ForceRefreshTextMeshPro(factCounter);
        }
    }

    private int GetRandomUnusedFactIndex()
    {
        if (usedFactIndexes.Count >= theaterFacts.Count)
        {
            usedFactIndexes.Clear();
        }

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, theaterFacts.Count);
        }
        while (usedFactIndexes.Contains(randomIndex));

        return randomIndex;
    }

    private void ShowCurrentFact()
    {
        if (theaterFacts.Count > currentFactIndex && factText != null)
        {
            factText.text = theaterFacts[currentFactIndex];
            Debug.Log($"���������� ���� {factsViewed}/{FACTS_NEEDED}: {theaterFacts[currentFactIndex]}");

            ForceRefreshTextMeshPro(factText);
        }
    }

    private void UpdateFactCounter()
    {
        if (factCounter != null)
        {
            factCounter.text = $"������ �������: {factsViewed}/{FACTS_NEEDED}";
            ForceRefreshTextMeshPro(factCounter);
        }
    }

    // ����� ��� ������� �������� ���� � ���� ��������
    private void StartBanditsRunAnimation()
    {
        foreach (Bandit bandit in bandits)
        {
            if (bandit != null)
            {
                bandit.StartRunAnimation();
            }
        }
    }

    // ����� ��� ������ �������� ��������
    private void ResetBanditsAnimations()
    {
        foreach (Bandit bandit in bandits)
        {
            if (bandit != null)
            {
                bandit.ResetAnimation();
            }
        }
    }

    private void ForceRefreshTextMeshPro(TextMeshProUGUI tmpText)
    {
        if (tmpText != null)
        {
            tmpText.ForceMeshUpdate();
            tmpText.SetAllDirty();
            Canvas.ForceUpdateCanvases();
        }
    }

    private void OnRestartButtonClicked()
    {
        factsPanel.SetActive(false);
        InitializeGame();
    }

    public void OnExitButtonClicked()
    {
        Debug.Log("Exit button clicked!");
    }

    private void SetAnimalButtonsActive(bool active)
    {
        donkeyButton.gameObject.SetActive(active);
        dogButton.gameObject.SetActive(active);
        catButton.gameObject.SetActive(active);
        roosterButton.gameObject.SetActive(active);
    }

    private void ResetButtonPositions()
    {
        // ���������� ������� ������
    }
}