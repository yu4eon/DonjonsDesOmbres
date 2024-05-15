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
    [SerializeField] AudioClip _sonCle;
    [SerializeField] Retroaction _modeleRetro; // Modèle de rétroaction #synthese Leon
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
            Porte.aCle = true; // Indique que le joueur a la clé
            Retroaction retro = Instantiate(_modeleRetro, transform.position, Quaternion.identity, transform.parent); // Crée une rétroaction
            retro.ChangerTexte("Clé obtenue", "#FFFFFF", 0.5f); // Change le texte de la rétroaction
            SoundManager.instance.JouerEffetSonore(_sonCle); // Joue le son de la clé
            Destroy(gameObject); //Détruit l'objet clé
        }
    }
}
