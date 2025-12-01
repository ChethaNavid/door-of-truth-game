using UnityEngine;
using TMPro; // Needed for TextMeshPro

public class DoorRow : MonoBehaviour
{
    [Header("The Sign")]
    public TextMeshPro questionText; // Drag the text on the black sign here

    [Header("Door 1 (Left)")]
    public DoorScript.Door door1;    // Drag Child Door 1 (Logic) here
    public TextMeshPro text1;        // Drag Child Door 1 (Text) here

    [Header("Door 2 (Middle)")]
    public DoorScript.Door door2;
    public TextMeshPro text2;

    [Header("Door 3 (Right)")]
    public DoorScript.Door door3;
    public TextMeshPro text3;

    // This is the function the GameManager calls
    public void SetupRow(QuizData data)
    {
        // 1. Write the Question on the big sign
        if(questionText != null) 
            questionText.text = data.question;

        // 2. Write the Answers on the doors
        if(text1 != null) text1.text = data.answerA;
        if(text2 != null) text2.text = data.answerB;
        if(text3 != null) text3.text = data.answerC;

        // 3. Reset all doors to WRONG first
        door1.isCorrectDoor = false;
        door2.isCorrectDoor = false;
        door3.isCorrectDoor = false;

        // 4. Set the CORRECT door based on the index (0, 1, or 2)
        switch (data.correctIndex)
        {
            case 0: door1.isCorrectDoor = true; break; // Left is correct
            case 1: door2.isCorrectDoor = true; break; // Middle is correct
            case 2: door3.isCorrectDoor = true; break; // Right is correct
        }
    }
}