using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanneauBonus : MonoBehaviour
{
    [SerializeField] SOPerso _donneesPerso; // Référence aux données du personnage (ScriptableObject)
    [SerializeField] TextMeshProUGUI _champTemps; // Champ de texte pour afficher le temps restant
    [SerializeField] TextMeshProUGUI _champPoints; // Champ de texte pour afficher les points bonus
    [SerializeField] TextMeshProUGUI _champScore; // Champ de texte pour afficher le score total du personnage
    
    // Méthode pour calculer et afficher les points bonus en fonction du temps restant
    public void CalculerPoints()
    {
        int tempsRestants = Niveau.instance.temps; // Récupère le temps restant depuis une instance de niveau

        // Calcule les minutes et les secondes à partir du temps restant en secondes
        int minutes = (tempsRestants / 60);
        int secondes = (tempsRestants % 60);

        // Met à jour le champ de texte pour afficher le temps restant au format MM:SS
        _champTemps.text = "Temps Restant : " + ((minutes < 10) ? "0" + minutes : minutes) + ":" + ((secondes < 10) ? "0" + secondes : secondes);

        // Calcule les points bonus en fonction du temps restant (10 points par seconde)
        int points = tempsRestants * 10;
        _champPoints.text = "Points Bonus : +" + points + " points"; // Met à jour le champ de texte pour afficher les points bonus

        // Ajoute les points bonus au score total du personnage
        _donneesPerso.score += points;
        _champScore.text = "Score : " + _donneesPerso.score; // Met à jour le champ de texte pour afficher le score total
    }
}
