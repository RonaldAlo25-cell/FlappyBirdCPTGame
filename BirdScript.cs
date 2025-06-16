using UnityEngine;
using System.Collections.Generic;

public class BirdScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float flapStrength;
    public LogicScript logic;
    public bool birdIsAlive = true;

    // score system
    private List<int> highScores = new List<int>();
    private int currentScore = 0;

    public void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    
        // sample scores
        highScores.AddRange(new int[] { 15, 8, 23, 12, 7, 19 });
        SortHighScores();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && birdIsAlive)
        {
            myRigidbody.linearVelocity = Vector2.up * flapStrength;
        }
    
        // press 'R' to check current rank (testing)
        if (Input.GetKeyDown(KeyCode.R))
        {
            DisplayCurrentRank();
        }
    }


    // bubble sort
    public void SortHighScores()
    {
        int n = highScores.Count;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (highScores[j] < highScores[j + 1]) // descending order
                {
                    int temp = highScores[j];
                    highScores[j] = highScores[j + 1];
                    highScores[j + 1] = temp;
                }
            }
        }
        Debug.Log("High scores sorted: " + string.Join(", ", highScores));
    }

    // when bird dies, add the score in the game
    public void AddScore(int score)
    {
        highScores.Add(score);
        SortHighScores();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // this will get the current score from logic script before game over
        if (logic != null)
        {
            currentScore = logic.playerScore;
            AddScore(currentScore);
        }

        logic.gameOver();
        birdIsAlive = false;
    }
    
    // binary search
    public int FindScoreRank(int targetScore)
    {
        if (highScores.Count == 0) return -1;
    
        int left = 0;
        int right = highScores.Count - 1;
    
        while (left <= right)
        {
            int mid = left + (right - left) / 2;
        
        if (highScores[mid] == targetScore)
        {
            Debug.Log($"Score {targetScore} found at rank {mid + 1}");
            return mid + 1;
        }
        
        if (highScores[mid] > targetScore) // this will sort in descending order
        {
            left = mid + 1;
        }
        else
            {
                right = mid - 1;
            }
        }
    
        Debug.Log($"Score {targetScore} not found. Would be inserted at rank {left + 1}");
        return -(left + 1);
    }

// this is a method to display the player's current rank
    public void DisplayCurrentRank()
    {
        if (currentScore > 0)
        {
            int rank = FindScoreRank(currentScore);
            if (rank > 0)
            {
                Debug.Log($"Your current score {currentScore} ranks #{rank} out of {highScores.Count} games");
            }
        }
    }
}
