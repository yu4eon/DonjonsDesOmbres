using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanneauInventaire : MonoBehaviour
{
    [SerializeField] PanneauVignette _panneauVignetteModele; // Modèle de panneau vignette utilisé pour afficher les objets dans l'inventaire

    // Méthode appelée au démarrage du script
    void Start()
    {
        // Vérifie si l'instance de la boutique existe
        if (Boutique.instance != null)
        {
            // Affiche l'inventaire du personnage en utilisant les données de la boutique
            Boutique.instance.donneesPerso.AfficherInventaire();
        }
    }

    // Méthode pour ajouter un objet à l'inventaire et afficher sa vignette
    public PanneauVignette Ajouter(SOObjet donnees)
    {
        // Instantie un nouveau panneau vignette comme enfant de l'objet transform
        PanneauVignette vignette = Instantiate(_panneauVignetteModele, transform);
        // Assigne les données de l'objet à la vignette
        vignette.donneesObjet = donnees;
        return vignette; // Retourne la vignette nouvellement créée
    }
    
    // Méthode pour vider l'inventaire en supprimant tous les objets enfants
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
