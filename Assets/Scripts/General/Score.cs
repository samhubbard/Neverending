using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    int score = 0;

    public void UpdateScore (int _score) {
        score += _score;
        Text scoreText = GetComponent<Text>();
        scoreText.text = "Score: " + score;
    }
}
