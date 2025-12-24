using UnityEngine;

public static class GameSettings
{
    public static LevelGenerator.Difficulty selectedDifficulty = LevelGenerator.Difficulty.Easy;
    public static string selectedLevelName = "easy"; // default, overwritten by menu selection
}
