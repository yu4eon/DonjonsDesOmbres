using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// #tp3
/// Auteur du code : Antoine Lachance
/// Commentaires ajoutés par : Antoine Lachance
/// </summary>
public class PanneauObjet : MonoBehaviour
{
    [Header("LES DONNÉES")] // Titre de la section des données.
    [SerializeField] SOObjet _donnees; // ScriptableObject contenant les données de l'objet.
    public SOObjet donnees => _donnees; // Propriété publique pour accéder aux données de l'objet.

    [Header("LES CONTENEURS")]
    [SerializeField] TextMeshProUGUI _champNom; // Champ de texte pour afficher le nom de l'objet.
    [SerializeField] TextMeshProUGUI _champPrix; // Champ de texte pour afficher le prix de l'objet.
    [SerializeField] TextMeshProUGUI _champDescription; // Champ de texte pour afficher la description de l'objet.
    [SerializeField] Image _image; // Image pour afficher l'objet.
    [SerializeField] CanvasGroup _canvasGroup; // Groupe de canvas pour gérer l'interaction avec le panneau.

    void Start()
    {
        MettreAJourInfos(); // Met à jour les informations du panneau.
        Boutique.instance.donneesPerso.evenementMiseAJour.AddListener(MettreAJourInfos); // Ajoute un écouteur pour mettre à jour les informations lorsque les données du personnage changent.
    }


    /// <summary>
    /// #tp3 Antoine
    /// Met à jour les informations affichées dans le panneau.
    /// </summary>
    void MettreAJourInfos()
    {
        _champNom.text = _donnees.nom; // Affiche le nom de l'objet.
        _champPrix.text = _donnees.prix + " $"; // Affiche le prix de l'objet suivi de "$".
        _champDescription.text = _donnees.description; // Affiche la description de l'objet.
        _image.sprite = _donnees.sprite; // Affiche l'image de l'objet.
        GererDispo(); // Gère la disponibilité de l'objet en fonction de l'argent du joueur.
    }

    /// <summary>
    /// #tp3 Antoine
    /// Gère la disponibilité de l'objet en fonction de l'argent du joueur.
    /// </summary>
    void GererDispo()
    {
        bool aAssezArgent = Boutique.instance.donneesPerso.argent >= _donnees.prix; // Vérifie si le joueur a assez d'argent pour acheter l'objet.
        if (aAssezArgent) // Si le joueur a assez d'argent.
        {
            _canvasGroup.interactable = true; // Active l'interaction avec le panneau.
            _canvasGroup.alpha = 1; // Affiche le panneau normalement.
        }
        else // Si le joueur n'a pas assez d'argent.
        {
            _canvasGroup.interactable = false; // Désactive l'interaction avec le panneau.
            _canvasGroup.alpha = .5f; // Rend le panneau semi-transparent pour indiquer qu'il n'est pas disponible à l'achat.
        }
    }

    /// <summary>
    /// Tp3 Antoine
    /// Méthode appelée lorsqu'un objet est acheté depuis ce panneau.
    /// </summary>
    public void Acheter()
    {
        Boutique.instance.donneesPerso.Acheter(_donnees); // Appelle la méthode d'achat de l'objet dans les données du personnage.
    }
}
