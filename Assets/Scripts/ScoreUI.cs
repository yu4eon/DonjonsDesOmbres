using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Classe responsable de l'affichage des scores dans l'interface utilisateur.
public class ScoreUI : MonoBehaviour
{
    public RowUI rowUI; // Référence à la ligne d'interface utilisateur pour afficher les scores.
    public ScoreManager scoreManager; // Référence au gestionnaire de scores.

    // Fonction appelée au démarrage de l'objet.
    void Start()
    {
        // Ajoute des scores de démonstration.
        scoreManager.AddScore(new Score("Antoine", 100));
        scoreManager.AddScore(new Score("Léon", 80));

        // Récupère les meilleurs scores et les convertit en tableau.
        var scores = scoreManager.GetHighScores().ToArray();
        
        // Pour chaque score dans le tableau.
        for (int i = 0; i < scores.Length; i++)
        {
            // Instancie une nouvelle ligne d'interface utilisateur à partir du prefab 'rowUI' et l'attache au transform parent.
            var row = Instantiate(rowUI, transform).GetComponent<RowUI>();

            // Met à jour le texte de la colonne du rang avec la position du score dans le classement.
            row.rank.text = (i + 1).ToString();

            // Met à jour le texte de la colonne du nom avec le nom du joueur associé au score.
            row.nom.text = scores[i].nom;

            // Met à jour le texte de la colonne du score avec la valeur du score.
            row.score.text = scores[i].score.ToString();
        }
    }
}
