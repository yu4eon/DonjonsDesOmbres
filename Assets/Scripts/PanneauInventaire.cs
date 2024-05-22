using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// #synthese Léon
/// Classe qui gère le panneau d'inventaire du jeu
/// Auteur : Léon Yu
/// Commentaires : Léon Yu, Antoine Lachance
/// </summary>
public class PanneauInventaire : MonoBehaviour
{
    [SerializeField] PanneauVignette _panneauVignetteModele; // Modèle de panneau vignette utilisé pour afficher les objets dans l'inventaire

    void Start()
    {
        // Vérifie si l'instance de la boutique existe
        if (Boutique.instance != null)
        {
            // Affiche l'inventaire du personnage en utilisant les données de la boutique
            Boutique.instance.donneesPerso.AfficherInventaire();
        }
    }

    /// <summary>
    /// Méthode pour ajouter une vignette à l'inventaire
    /// </summary>
    /// <param name="donnees">Les donnees de l'objet</param>
    /// <returns></returns>
    public PanneauVignette Ajouter(SOObjet donnees)
    {
        // Instantie un nouveau panneau vignette comme enfant de l'objet transform
        PanneauVignette vignette = Instantiate(_panneauVignetteModele, transform);
        // Assigne les données de l'objet à la vignette
        vignette.donneesObjet = donnees;
        return vignette; // Retourne la vignette nouvellement créée
    }
    
    /// <summary>
    /// Méthode pour vider le panneau d'inventaire
    /// </summary>
    public void Vider()
    {
        // Boucle à travers tous les enfants de l'objet transform
        foreach (Transform enfant in transform)
        {
            // Détruit chaque enfant (vignette) dans l'inventaire
            Destroy(enfant.gameObject);
        }
    }
}
