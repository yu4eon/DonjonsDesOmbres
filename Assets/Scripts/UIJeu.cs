using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// #tp4
/// Auteur du code : Léon Yu
/// Commentaires ajoutés par : Léon Yu
/// Classe qui gère l'affichage des informations du personnage dans le jeu
/// </summary>
public class UIJeu : MonoBehaviour
{
    [SerializeField] private SOPerso _donneesPerso; // Données du personnage, (ScriptableObject)

    [SerializeField] private TextMeshProUGUI _champNiveau; // Champ de texte pour le niveau du personnage
    [SerializeField] private TextMeshProUGUI _champTemps; // Champ de texte pour le temps de jeu
    [SerializeField] private TextMeshProUGUI _champScore; // Champ de texte pour le score du personnage
    [SerializeField] private TextMeshProUGUI _champArgent; // Champ de texte pour l'argent du personnage
    [SerializeField] private Image _barreVie; // Barre de vie du personnage (non utilisée)
    [SerializeField] private Image[] _tCrystalsPouvoir; // Tableau d'images pour les cristaux de pouvoir
    Dictionary<TypePouvoir, Image> _dCrystalsPouvoir = new Dictionary<TypePouvoir, Image>(); // Dictionnaire de cristaux de pouvoir



    
    // Start is called before the first frame update
    void Start()
    {
        // Initialisation des champs de texte
        _champNiveau.text = "Lvl : " + _donneesPerso.niveau; 
        _champScore.text = _donneesPerso.score +"";
        _champArgent.text = _donneesPerso.argent +" Gold";
        _donneesPerso.evenementMiseAJour.AddListener(MettreAJourInfo); // Ajoute l'événement de mise à jour



        // Initialisation du dictionnaire de cristaux de pouvoir
        //J'aurais pu faire un loop for mais avec les types de pouvoirs, c'est plus facile de le faire manuellement
        _dCrystalsPouvoir.Add(TypePouvoir.Poison, _tCrystalsPouvoir[0]);
        _dCrystalsPouvoir.Add(TypePouvoir.Ombre, _tCrystalsPouvoir[1]);
        _dCrystalsPouvoir.Add(TypePouvoir.Foudre, _tCrystalsPouvoir[2]);
        _dCrystalsPouvoir.Add(TypePouvoir.Glace, _tCrystalsPouvoir[3]);

    }


    /// <summary>
    /// Méthode qui met à jour les informations du personnage
    /// </summary>
    void MettreAJourInfo()
    {
        _champScore.text = _donneesPerso.score + "";
        _champArgent.text = _donneesPerso.argent + " Gold";

        //Mettre à jour les cristaux de pouvoir
        foreach (KeyValuePair<TypePouvoir, Image> entry in _dCrystalsPouvoir)
        {
            //Si le personnage a le pouvoir, on l'active
            if (_donneesPerso.ContientPouvoir(entry.Key)) // Assuming SOPerso has a method to check this
            {
                entry.Value.enabled = true;
            }
            else
            {
                entry.Value.enabled = false;
            }
        }
    }
}
