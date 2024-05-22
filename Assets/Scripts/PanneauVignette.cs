using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// #synthese Léon
/// Classe qui gère les vignettes d'objets dans l'inventaire
/// Auteur : Léon Yu
/// Commentaires : Léon Yu, Antoine Lachance
/// </summary>
public class PanneauVignette : MonoBehaviour
{
    [SerializeField] Image _image; // Image de la vignette représentant l'objet
    [SerializeField] TextMeshProUGUI _champ; // Champ de texte pour afficher le nombre d'objets

    SOObjet _donneesObjet; // Données de l'objet associé à la vignette

    // Propriété pour obtenir et définir les données de l'objet
    public SOObjet donneesObjet
    {
        get { return _donneesObjet; }
        set
        {
            _donneesObjet = value; // Assigne les nouvelles données de l'objet
            _image.sprite = _donneesObjet.sprite; // Met à jour l'image de la vignette avec le sprite de l'objet
        }
    }	

    int _nb = 1; // Nombre d'objets

    // Propriété pour obtenir et définir le nombre d'objets
    public int nb
    {
        get { return _nb; }
        set
        {
            _nb = value; // Assigne la nouvelle valeur du nombre d'objets
            _champ.text = _nb.ToString(); // Met à jour le champ de texte avec le nombre d'objets
        }
    }

    // Méthode appelée avant la première frame d'update
    void Start()
    {
        nb = nb; // Initialise le champ de texte avec la valeur actuelle de _nb
    }
}
