using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanneauBonus : MonoBehaviour
{

    [SerializeField] SOPerso _donneesPerso; // Données du personnage
    [SerializeField] TextMeshProUGUI _champTemps; // Champ de texte pour le temps de jeu
    [SerializeField] TextMeshProUGUI _champPoints;
    [SerializeField] TextMeshProUGUI _champScore; // Champ de texte pour le score du personnage
    
    public void CalculerPoints()
    {
        int tempsRestants = Niveau.instance.temps;
        
        int minutes = (tempsRestants / 60);
        int secondes = (tempsRestants % 60);
        _champTemps.text = "Temps Restant : " + ((minutes < 10)? "0" +minutes: minutes) + ":" + ((secondes < 10)? "0" +secondes: secondes);

        int points = tempsRestants * 10;
        _champPoints.text = "Points Bonus : +" + points + " points";

        _donneesPerso.score += points;
        _champScore.text = "Score : " + _donneesPerso.score;

    }
}
