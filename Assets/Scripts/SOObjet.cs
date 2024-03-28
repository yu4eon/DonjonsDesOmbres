using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

/// <summary>
/// #tp3
/// Auteur du code : Antoine Lachance
/// Commentaires ajoutés par : Antoine Lachance
/// ScriptableObject pour définir les attributs d'un objet disponible dans la boutique.
/// </summary>
[CreateAssetMenu(fileName = "Objet", menuName = "Objet boutique")]
public class SOObjet : ScriptableObject
{
    [Header("LES DONNÉES")] // En-tête pour regrouper les données

    [SerializeField] string _nom = "Trèfle"; // Nom de l'objet
    [SerializeField, Tooltip("Îcone de l'objet pour la boutique")] Sprite _sprite; // Sprite de l'objet
    [SerializeField][Range(0, 200)] int _prixDeBase = 30; // Prix de base de l'objet
    [SerializeField, TextArea] string _description; // Description de l'objet
    [SerializeField][Tooltip("Cet objet a-t'il déjà été acheté?")] bool _estAcheter = false; // Indique si l'objet a été acheté
    [SerializeField] TypeObjet _typeObjet; // Type de l'objet

    /// <summary>
    /// Propriété pour accéder au type de l'objet.
    /// </summary>
    public TypeObjet typeObjet { get => _typeObjet; set => _typeObjet = value; }

    /// <summary>
    /// Propriété pour accéder et modifier le nom de l'objet.
    /// </summary>
    public string nom { get => _nom; set => _nom = value; }

    /// <summary>
    /// Propriété pour accéder et modifier le sprite de l'objet.
    /// </summary>
    public Sprite sprite { get => _sprite; set => _sprite = value; }

    /// <summary>
    /// Propriété en lecture seule pour accéder au prix de l'objet.
    /// </summary>
    public int prix => _prixDeBase;

    /// <summary>
    /// Propriété pour accéder et modifier la description de l'objet.
    /// </summary>
    public string description { get => _description; set => _description = value; }

    /// <summary>
    /// Propriété pour accéder et modifier l'état d'achat de l'objet.
    /// </summary>
    public bool estAcheter { get => _estAcheter; set => _estAcheter = value; }
}
