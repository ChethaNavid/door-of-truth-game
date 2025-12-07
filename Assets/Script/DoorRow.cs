using UnityEngine;
using TMPro;

public class DoorRow : MonoBehaviour
{
    [Header("The Sign")]
    public TextMeshPro questionText;

    [Header("Door 1 (Left)")]
    public DoorScript.Door door1;
    public TextMeshPro text1;

    [Header("Door 2 (Middle)")]
    public DoorScript.Door door2;
    public TextMeshPro text2;

    [Header("Door 3 (Right)")]
    public DoorScript.Door door3;
    public TextMeshPro text3;

    // This is the function the GameManager calls
    public void SetupRow(QuizData data)
    {
        if (questionText != null)
            questionText.text = data.questionText;

        if (data.answers != null && data.answers.Length >= 3)
        {
            if (text1 != null) text1.text = data.answers[0];
            if (text2 != null) text2.text = data.answers[1];
            if (text3 != null) text3.text = data.answers[2];
        }

        // Reset all doors to WRONG first
        if (door1 != null) door1.isCorrectDoor = false;
        if (door2 != null) door2.isCorrectDoor = false;
        if (door3 != null) door3.isCorrectDoor = false;

        // Set the CORRECT door based on the index (0, 1, or 2)
        switch (data.correctIndex)
        {
            case 0: if (door1 != null) door1.isCorrectDoor = true; break; // Left 
            case 1: if (door2 != null) door2.isCorrectDoor = true; break; // Middle 
            case 2: if (door3 != null) door3.isCorrectDoor = true; break; // Right 
        }
    }
}