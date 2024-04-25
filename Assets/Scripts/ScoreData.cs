using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScoreData : MonoBehaviour
{
    public List<Score> scores; // Liste des scores

    public ScoreData() // Constructeur de la classe ScoreData
    {
        scores = new List<Score>();
    }
}
