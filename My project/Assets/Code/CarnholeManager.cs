using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarnholeManager : MonoBehaviour
{
    public static CarnholeManager Instance;

    public bool isAlternatingTurns;
    public bool isPlayerOnesTurn;
    public bool player1GoesFirst;
    public GameObject player1Indicator;
    public GameObject player2Indicator;
    private int _turnsCompletedInRounds;

    public int p1RoundScore;
    public int p2RoundScore;
    public TextMeshProUGUI p1RoundScoreText;
    public TextMeshProUGUI p2RoundScoreText;

    public int p1TotalScore;
    public int p2TotalScore;
    public TextMeshProUGUI p1TotalScoreText;
    public TextMeshProUGUI p2TotalScoreText;

    public Button nextRoundButton;
    public Button newGameButton;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartNewGame();
    }

    private void OnEnable()
    {
        nextRoundButton.onClick.AddListener(CompleteRound);
        newGameButton.onClick.AddListener(StartNewGame);
    }

    private void OnDisable()
    {
        nextRoundButton.onClick.RemoveListener(CompleteRound);
        newGameButton.onClick.RemoveListener(StartNewGame);
    }

    public void AwardPointsForRound(bool isPlayer1, int pointsToAward, bool goToNextPlayer)
    {
        if (isPlayer1) 
        {
            if (p2RoundScore == 0)
            {
                p1RoundScore += pointsToAward;
            }
            else if (p2RoundScore >= pointsToAward)
            {
                p2RoundScore -= pointsToAward;
            }
            else if (p2RoundScore > 0 && p2RoundScore < pointsToAward) 
            {
                p1RoundScore = p1RoundScore + pointsToAward - p2RoundScore;
                p2RoundScore = 0;
            }
        }
        else
        {
            if (p1RoundScore == 0)
            {
                p2RoundScore += pointsToAward;
            }
            else if (p1RoundScore >= pointsToAward)
            {
                p1RoundScore -= pointsToAward;
            }
            else if (p1RoundScore > 0 && p1RoundScore < pointsToAward)
            {
                p2RoundScore = p2RoundScore + pointsToAward - p1RoundScore;
                p1RoundScore = 0;
            }
        }
        if (goToNextPlayer)
        {
            DetermineNextPlayer();
        }
        UpdateUI();
    }

    public void DetermineNextPlayer()
    {
        _turnsCompletedInRounds++;
        if (_turnsCompletedInRounds < 8)
        {
            if (isAlternatingTurns)
            {
                isPlayerOnesTurn = !isPlayerOnesTurn;
            }
            else
            {
                if (_turnsCompletedInRounds < 4)
                {
                    if (player1GoesFirst)
                    {
                        isPlayerOnesTurn = true;
                    }
                    else
                    {
                        isPlayerOnesTurn = false;
                    }
                }
                else
                {
                    if (player1GoesFirst)
                    {
                        isPlayerOnesTurn = false;
                    }
                    else
                    {
                        isPlayerOnesTurn = true;
                    }
                }
            }
        }
        else
        {
            ShowEndOfRoundUI();
            _turnsCompletedInRounds = 0;
        }
    }

    public void ShowEndOfRoundUI()
    {
        if (p1TotalScore >= 21)
        {
            print("P1 WINS");
            newGameButton.gameObject.SetActive(true);
        }
        else if (p2TotalScore >= 21)
        {
            newGameButton.gameObject.SetActive(true);
            print("P2 WINS");
        }
        else
        {
            nextRoundButton.gameObject.SetActive(true);
        }
    }

    void CompleteRound()
    {
        if (p1RoundScore > p2RoundScore)
        {
            player1GoesFirst = true;
            isPlayerOnesTurn = true;
        }
        else if (p1RoundScore < p2RoundScore)
        {
            player1GoesFirst = false;
            isPlayerOnesTurn = false;
        }
        p1TotalScore += p1RoundScore;
        p2TotalScore += p2RoundScore;
        p1RoundScore = 0;
        p2RoundScore = 0;
        UpdateUI();
        Bag[] currentBags = FindObjectsOfType<Bag>();
        foreach (Bag bag in currentBags)
        {
            Destroy(bag.gameObject);
        }
        BagSpawner.Instance.SpawnBags();
        nextRoundButton.gameObject.SetActive(false);
    }

    void UpdateUI()
    {
        p1RoundScoreText.SetText("Blue: " + p1RoundScore.ToString());
        p2RoundScoreText.SetText("Red: " + p2RoundScore.ToString());
        p1TotalScoreText.SetText("Blue: " + p1TotalScore.ToString());
        p2TotalScoreText.SetText("Red: " + p2TotalScore.ToString());
        if (isPlayerOnesTurn)
        {
            player1Indicator.SetActive(true);
            player2Indicator.SetActive(false);
        }
        else
        {
            player1Indicator.SetActive(false);
            player2Indicator.SetActive(true);
        }
    }

    public void StartNewGame()
    {
        Bag[] currentBags = FindObjectsOfType<Bag>();
        if (currentBags != null)
        {
            foreach (Bag bag in currentBags)
            {
                Destroy(bag.gameObject);
            }
        }
        BagSpawner.Instance.SpawnBags();
        isPlayerOnesTurn = true;
        player1GoesFirst = true;
        p1RoundScore = 0;
        p2RoundScore = 0;
        p1TotalScore = 0;
        p2TotalScore = 0;
        UpdateUI();
        newGameButton.gameObject.SetActive(false);
    }
}
