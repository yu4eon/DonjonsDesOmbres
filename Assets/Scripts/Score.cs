using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Score : MonoBehaviour
{
    public string nom; // Nom du joueur
    public float score; // Score du joueur

    // Variable pour creer un score plustard
    public Score(string nom, float score)
    {
        this.nom = nom;
        this.score = score;
    }
}
