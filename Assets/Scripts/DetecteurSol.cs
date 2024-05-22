using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Auteur du code : Léon Yu, Antoine Lachance
/// Commentaires ajoutés par : Léon Yu, Antoine Lachance
/// Classe qui gère la détection du sol pour les personnages.
/// #synthese leon Sera utilisé pour les ennemis aussi.
/// </summary>
public class DetecteurSol : MonoBehaviour
{
    [SerializeField] Vector2 _posCentreBoite = new Vector2(0, -1f); // Position du centre de la boîte de collision.
    [SerializeField] Vector2 _tailleBoite = new Vector2(0.5f, 0.1f); // Taille de la boîte de collision.
    [SerializeField] LayerMask _mask; // Les Layermasks qui sont considérés comme sol.
    protected bool _estAuSol; // Si le personnage est au sol.

    /// <summary>
    /// Méthode qui est appelée à chaque frame à un rythme fixe.
    /// </summary>
    virtual protected void FixedUpdate()
    {
        VerifierSol(); // Vérifie si le personnage est au sol.
    }

    /// <summary>
    /// Méthode qui vérifie si le personnage est au sol par OverlapBox.
    /// </summary>
    void VerifierSol()
    {
        Vector2 pointDepart = (Vector2)transform.position + _posCentreBoite; // Calcule le point de départ de la boîte de collision.
        Collider2D collision = Physics2D.OverlapBox(pointDepart, _tailleBoite, 0, _mask); // Vérifie la présence d'une collision avec le sol.
        _estAuSol = collision != null; // Met à jour l'état du sol.
        // Debug.Log("Est au sol?" + _estAuSol);
    }

    /// <summary>
    /// Méthode qui dessine la boîte de collision dans l'éditeur.
    /// </summary>
    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            VerifierSol(); // Vérifie l'état du sol même si le jeu n'est pas en cours d'exécution.
        }
        Gizmos.color = _estAuSol ? Color.green : Color.red; // Change la couleur de la boîte en fonction de l'état du sol.
        Vector2 pointDepart = (Vector2)transform.position + _posCentreBoite; // Calcule le point de départ de la boîte de collision.
        Gizmos.DrawWireCube(pointDepart, _tailleBoite); // Dessine la boîte de collision.
    }
}
