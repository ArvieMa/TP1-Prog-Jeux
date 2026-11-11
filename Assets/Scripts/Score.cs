using UnityEngine;

public class Score : MonoBehaviour
{
    private int currentScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentScore = 0;
        UIManager.instance.HudController.setScore(currentScore);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void increaseScore(int points)
    {

        currentScore += points;

        UIManager.instance.HudController.setScore(currentScore);

    }
}
