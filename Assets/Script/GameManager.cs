using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject rowPrefab;      // Drag your "Challenge_Row_1" prefab here
    public int numberOfChallenges = 5;// How many walls to spawn
    public float distanceBetween = 40f; // Meters between each wall
    public Transform startPoint;      // Where the first wall spawns

    [Header("Question Database")]
    public List<QuizData> questions;  // Type your 5 questions here

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        Vector3 spawnPosition = Vector3.zero;

        // Use Start Point if assigned, otherwise use (0,0,0)
        if (startPoint != null) spawnPosition = startPoint.position;

        for (int i = 0; i < numberOfChallenges; i++)
        {
            // 1. Calculate Position (Move forward Z axis)
            Vector3 finalPos = spawnPosition + new Vector3(0, 0, i * distanceBetween);

            // 2. Spawn the Prefab
            GameObject newRow = Instantiate(rowPrefab, finalPos, Quaternion.identity);
            
            // Optional: Parent it to this manager to keep Hierarchy clean
            newRow.transform.SetParent(transform); 
            newRow.name = "Challenge_Row_" + (i + 1);

            // 3. Inject the Data
            DoorRow rowScript = newRow.GetComponent<DoorRow>();
            
            if (rowScript != null)
            {
                // Check if we have enough questions, otherwise reuse the last one
                int questionIndex = i % questions.Count; 
                rowScript.SetupRow(questions[questionIndex]);
            }
        }
    }
}