using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public enum Difficulty { Easy, Medium, Hard, Infinite }
    [Header("Game Mode")]
    public Difficulty selectedDifficulty = Difficulty.Easy;
    public bool isInfinite = false;
    public int totalQuestions = 10;

    [Header("Data Source")]
    [Tooltip("Drag your questions.json file here")]
    public TextAsset jsonFile;

    [Header("References")]
    public GameObject tilePrefab;
    public GameObject wallPrefab;
    public GameObject finishLinePrefab;
    public Transform player;

    [Header("Start Positions (Z Axis)")]
    public float tileStartZ = 10f; 
    public float wallStartZ = 30f; 

    [Header("Position Offsets")]
    public float levelHeight = 15f; 
    public float wallYOffset = 11.5f; 
    public float wallXOffset = -3.3f; 
    
    [Header("Spacing Settings")]
    public float distanceBetweenWalls = 50f; 
    public float tileSize = 5.0f;       
    public float spawnDistance = 50f;   

    // Internal State
    private List<QuizData> activeQuestionList = new List<QuizData>();
    private float currentSpawnZ = 0;
    private int questionsSpawnedCount = 0;
    private bool levelComplete = false;
    private List<int> shuffledIndices = new List<int>();

    void Start()
    {
        LoadQuestions();

        // Safety Check
        if (activeQuestionList == null || activeQuestionList.Count == 0)
        {
            Debug.LogError("No questions found for " + selectedDifficulty + " mode!");
            return;
        }

        // Create Shuffle List
        for (int i = 0; i < activeQuestionList.Count; i++) shuffledIndices.Add(i);
        ShuffleList(shuffledIndices);

        // Start Spawning
        currentSpawnZ = tileStartZ;
        while (currentSpawnZ < wallStartZ) SpawnFloorTile();
        
        currentSpawnZ = wallStartZ;
        SpawnWall();
        questionsSpawnedCount++; 

        for (int i = 0; i < 2; i++) CheckAndSpawn();
    }
    void LoadQuestions()
    {
        if (jsonFile == null) return;

        QuestionCollection loadedData = JsonUtility.FromJson<QuestionCollection>(jsonFile.text);
        activeQuestionList = new List<QuizData>();

        // 2. NEW: Pick the list based on Difficulty Dropdown
        switch (selectedDifficulty)
        {
            case Difficulty.Easy:
                activeQuestionList = loadedData.easy;
                break;
            case Difficulty.Medium:
                activeQuestionList = loadedData.medium;
                break;
            case Difficulty.Hard:
                activeQuestionList = loadedData.hard;
                break;
            case Difficulty.Infinite:
                // Combine ALL lists for Infinite mode
                if(loadedData.easy != null) activeQuestionList.AddRange(loadedData.easy);
                if(loadedData.medium != null) activeQuestionList.AddRange(loadedData.medium);
                if(loadedData.hard != null) activeQuestionList.AddRange(loadedData.hard);
                break;
        }
        
        Debug.Log("Loaded " + activeQuestionList.Count + " questions for " + selectedDifficulty + " mode.");
    }

    void Update()
    {
        if (levelComplete) return;

        if (player.position.z > currentSpawnZ - spawnDistance)
        {
            CheckAndSpawn();
        }
    }

    void CheckAndSpawn()
    {
        if (isInfinite)
        {
            SpawnSection();
        }
        else
        {
            if (questionsSpawnedCount < totalQuestions)
            {
                SpawnSection();
                questionsSpawnedCount++;
            }
            else if (!levelComplete)
            {
                SpawnFinishLine();
                levelComplete = true;
            }
        }
    }

    void SpawnSection()
    {
        int tilesNeeded = Mathf.RoundToInt(distanceBetweenWalls / tileSize);
        for (int i = 0; i < tilesNeeded; i++) SpawnFloorTile();
        SpawnWall();
    }

    void SpawnFloorTile()
    {
        Vector3 pos = new Vector3(0, levelHeight, currentSpawnZ);
        Instantiate(tilePrefab, pos, Quaternion.identity);
        currentSpawnZ += tileSize;
    }

    void SpawnWall()
    {
        Instantiate(tilePrefab, new Vector3(0, levelHeight, currentSpawnZ), Quaternion.identity);
        
        GameObject newWall = Instantiate(wallPrefab, new Vector3(wallXOffset, levelHeight + wallYOffset, currentSpawnZ), Quaternion.identity);
        DoorRow rowScript = newWall.GetComponent<DoorRow>();
        
        if (rowScript != null && activeQuestionList.Count > 0)
        {
            // Use Modulo (%) so we never crash even if we run out of unique questions
            int uniqueIndex = shuffledIndices[questionsSpawnedCount % shuffledIndices.Count];
            rowScript.SetupRow(activeQuestionList[uniqueIndex]);
        }
        currentSpawnZ += tileSize; 
    }

    void SpawnFinishLine()
    {
        for (int i = 0; i < 3; i++) SpawnFloorTile();
        Vector3 finishPos = new Vector3(0, levelHeight + 0.05f, currentSpawnZ);
        Instantiate(finishLinePrefab, finishPos, Quaternion.identity);
    }

    void ShuffleList(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
