using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Classe responsable de la gestion des scores dans le jeu.
public class ScoreManager : MonoBehaviour
{
    private ScoreData sd; // Données de score utilisées par le gestionnaire de scores.

    // Fonction appelée lors de l'initialisation de l'objet.
    void Awake()
    {
        // Initialise les données de score.
        sd = new ScoreData();
    }

    // Renvoie les meilleurs scores triés par ordre décroissant.
    public IEnumerable<Score> GetHighScores()
    {
        // Trie les scores par ordre décroissant en fonction du score.
        return sd.scores.OrderByDescending(x => x.score);
    }

    // Ajoute un score à la liste des scores.
    public void AddScore(Score score)
    {
        // Ajoute le score à la liste des scores.
        sd.scores.Add(score);
    }
}
