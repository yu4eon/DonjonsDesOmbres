using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public RowUI rowUI;
    public ScoreManager scoreManager;


    void Start()
    {
        scoreManager.AddScore(new Score("Antoine", 100));
        scoreManager.AddScore(new Score("Léon", 80));

        var scores = scoreManager.GetHighScores().ToArray();
        for (int i = 0; i < scores.Length; i++)
        {
            var row = Instantiate(rowUI, transform).GetComponent<RowUI>();
            row.rank.text = (i + 1).ToString();
            row.nom.text = scores[i].nom;
            row.score.text = scores[i].score.ToString();
        }
    }
}
