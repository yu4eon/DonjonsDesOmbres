using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// #synthese Léon
/// Classe qui gère les armes du personnage.
/// Auteur : Léon Yu
/// Commentaires : Léon Yu
/// </summary>

[CreateAssetMenu(fileName = "Arme", menuName = "Arme Perso")]
public class SOArme : ScriptableObject
{
    [Header("LES DONNÉES")] // En-tête pour regrouper les données
    [SerializeField] string _nom; // Nom de l'arme
    [SerializeField] float _degatsBase; // Dégâts infligés par l'arme
    [SerializeField] TypePouvoir _typePouvoir; // Type de pouvoir de l'arme
    [SerializeField] AudioClip _sonAttaque; // Son d'attaque de l'arme
    
    public AudioClip sonAttaque { get => _sonAttaque; set => _sonAttaque = value; } // Getter et setter pour le son d'attaque de l'arme
    public float degats { get => _degatsBase; set => _degatsBase = value; } // Getter et setter pour les dégâts de l'arme
    public TypePouvoir typePouvoir { get => _typePouvoir; set => _typePouvoir = value; } // Getter et setter pour le type de pouvoir de l'arme
    public string nom { get => _nom; set => _nom = value; } // Getter et setter pour le nom de l'arme
        
}
