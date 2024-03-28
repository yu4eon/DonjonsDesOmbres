using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// #tp3
/// Auteur du code : Léon Yu
/// Commetaires ajoutés par : Léon Yu
/// classe quigère les rétroactions, qui sont des objets qui apparaissent à l'écran pour donner des informations au joueur
/// Présentement seulement appelé pour donner de l'or au joueur
/// </summary>
public class Retroaction : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _champ; // Champ de texte de la rétroaction
    
    /// <summary>
    /// Méthode qui change le texte de la rétroaction selon le texte donné
    /// </summary>
    public void ChangerTexte(string texte)
    {
        _champ.text = texte;
    }

    /// <summary>
    /// Méthode qui détruit la rétroaction, appelée en fin d'animation
    /// </summary>
    public void Detruire()
    {
        Destroy(gameObject);
    }
}
