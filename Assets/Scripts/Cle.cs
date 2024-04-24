using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// #Tp3
/// Auteur du code : Antoine Lachance
/// Commentaires ajoutés par : Antoine Lachance
/// Classe qui gère la clé
/// </summary>
public class Cle : MonoBehaviour
{
    [SerializeField] Retroaction _retroModele; // Modèle de rétroaction lorsque le joueur pogne la clé #tp4 Leon
    /// <summary>
    /// #Tp3 Antoine
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) //Si le joueur entre en collision avec la clé
        {
            Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent); // Instantie l'objet de retro
            retro.ChangerTexte("Clé obtenu");

            Porte.aCle = true;
            Destroy(gameObject);    
        }
    }
}
