using UnityEngine;

[System.Serializable]
public class QuizData
{
    [TextArea]
    public string question;
    public string answerA;
    public string answerB;
    public string answerC;
    
    [Tooltip("0 = Left, 1 = Middle, 2 = Right")]
    [Range(0, 2)] 
    public int correctIndex; 
}