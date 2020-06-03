using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoring : MonoBehaviour
{
    public int score = 0;
    public int multiplier = 1;
    private Text scoreText;

    void Start() {
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        resetScore();
    }

    public void increment() {
        score += 1 * multiplier;
        multiplier++;
        scoreText.text = score.ToString();
    }

    public void resetMultiplier() {
        multiplier = 1;
    }

    public void resetScore() {
        score = 0;
        scoreText.text = "0";
    }
}
