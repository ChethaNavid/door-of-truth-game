using UnityEngine;
using System.Collections.Generic;

public enum Difficulty { Easy, Medium, Hard, Infinite }

public class LevelGenerator : MonoBehaviour
{
    [Header("Game Mode")]
    public Difficulty selectedDifficulty = Difficulty.Easy;
    public bool isInfinite = false;
    public int totalQuestions = 10; // only used for non-infinite

    [Header("Data Source")]
    public TextAsset jsonFile;

    [Header("References")]
    public GameObject tilePrefab;
    public GameObject wallPrefab;
    public GameObject finishLinePrefab;
    public Transform player;

    [Header("Start Positions (Z Axis)")]
    public float tileStartZ = 0f;
    public float wallStartZ = 100f;

    [Header("Position Offsets")]
    public float levelHeight = 15f;
    public float wallYOffset = 11.5f;
    public float wallXOffset = -3.3f;

    [Header("Spacing Settings")]
    public float distanceBetweenWalls = 50f;
    public float tileSize = 5f;
    public float spawnDistance = 75f;

    // Internal State
    private List<QuizData> activeQuestionList = new List<QuizData>();
    private float currentSpawnZ = 0;
    private int questionsSpawnedCount = 0;
    private bool levelComplete = false;
    private List<int> shuffledIndices = new List<int>();

    void Start()
    {
        // Get difficulty from menu
        selectedDifficulty = GameSettings.selectedDifficulty;
        isInfinite = selectedDifficulty == Difficulty.Infinite;

        LoadQuestions();

        if (activeQuestionList == null || activeQuestionList.Count == 0)
        {
            Debug.LogError("No questions found for " + selectedDifficulty + " mode!");
            return;
        }

        // Shuffle indices
        for (int i = 0; i < activeQuestionList.Count; i++) shuffledIndices.Add(i);
        ShuffleList(shuffledIndices);

        // Spawn initial floor tiles
        currentSpawnZ = tileStartZ;
        while (currentSpawnZ < wallStartZ) SpawnFloorTile();

        SpawnWall();
        questionsSpawnedCount++;

        // Spawn a few sections ahead
        for (int i = 0; i < 2; i++) CheckAndSpawn();
    }

    void LoadQuestions()
    {
        if (jsonFile == null) return;

        QuestionCollection loadedData = JsonUtility.FromJson<QuestionCollection>(jsonFile.text);
        activeQuestionList = new List<QuizData>();

        switch (selectedDifficulty)
        {
            case Difficulty.Easy:
                if (loadedData.easy != null) activeQuestionList.AddRange(loadedData.easy);
                break;
            case Difficulty.Medium:
                if (loadedData.medium != null) activeQuestionList.AddRange(loadedData.medium);
                break;
            case Difficulty.Hard:
                if (loadedData.hard != null) activeQuestionList.AddRange(loadedData.hard);
                break;
            case Difficulty.Infinite:
                if (loadedData.easy != null) activeQuestionList.AddRange(loadedData.easy);
                if (loadedData.medium != null) activeQuestionList.AddRange(loadedData.medium);
                if (loadedData.hard != null) activeQuestionList.AddRange(loadedData.hard);
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
        int maxQuestions = isInfinite ? activeQuestionList.Count : totalQuestions;

        if (questionsSpawnedCount < maxQuestions)
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
