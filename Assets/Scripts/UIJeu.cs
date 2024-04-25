using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Classe responsable de la mise à jour de l'interface utilisateur (UI) du jeu.
public class UIJeu : MonoBehaviour
{
    [SerializeField] private SOPerso _donneesPerso; // Référence aux données du personnage.

    [SerializeField] private TextMeshProUGUI _champNiveau; // Champ de texte pour afficher le niveau.
    [SerializeField] private TextMeshProUGUI _champTemps; // Champ de texte pour afficher le temps (non utilisé dans le code fourni).
    [SerializeField] private TextMeshProUGUI _champScore; // Champ de texte pour afficher le score.
    [SerializeField] private TextMeshProUGUI _champArgent; // Champ de texte pour afficher la quantité d'argent.

    // Fonction appelée au démarrage de l'objet.
    void Start()
    {
        // Met à jour les informations affichées initiales.
        _champNiveau.text = "Lvl : " + _donneesPerso.niveau;
        _champScore.text = _donneesPerso.score + "";
        _champArgent.text = _donneesPerso.argent + " Gold";

        // Ajoute la fonction MettreAJourInfo comme auditeur de l'événement de mise à jour des informations du personnage.
        _donneesPerso.evenementMiseAJour.AddListener(MettreAJourInfo);
    }

    // Fonction appelée pour mettre à jour les informations affichées.
    void MettreAJourInfo()
    {
        // Met à jour le champ de texte du score avec la nouvelle valeur du score.
        _champScore.text = _donneesPerso.score + "";
        // Met à jour le champ de texte de la quantité d'argent avec la nouvelle valeur d'argent.
        _champArgent.text = _donneesPerso.argent + " Gold";
    }
}
