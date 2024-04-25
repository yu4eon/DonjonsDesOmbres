using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Score : MonoBehaviour
{
    public string nom;
    public float score;

    public Score(string nom, float score)
    {
        this.nom = nom;
        this.score = score;
    }
}
