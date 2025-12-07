using System.Collections.Generic;

[System.Serializable]
public class QuestionCollection
{
    public List<QuizData> easy;
    public List<QuizData> medium;
    public List<QuizData> hard;
}

[System.Serializable]
public class QuizData
{
    public string questionText;
    public string[] answers;
    public int correctIndex;
}