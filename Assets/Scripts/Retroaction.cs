using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

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
    [SerializeField] Animator _animator; // Animator de la rétroaction
    
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Méthode qui change le texte de la rétroaction selon le texte donné
    /// </summary>
    /// <param name="texte">Le texte à afficher</param>
    /// <param name="couleur">La couleur du texte</param>
    /// <param name="vitesse">La vitesse de l'animation</param>
    /// <param name="taille">La taille de la rétroaction</param>
    /// <returns></returns>
    public void ChangerTexte(string texte, string couleur = "#FFFFFF", float vitesse = 1f, float taille = 1f)
    {
        _animator.speed = vitesse;
        transform.localScale = transform.localScale * taille;
        _champ.text = texte;
        ColorUtility.TryParseHtmlString(couleur, out Color couleurTexte);
        // Debug.Log(couleurTexte);
        _champ.color = new Color(couleurTexte.r, couleurTexte.g, couleurTexte.b, 1);
        // _champ.color = hexToColor(couleur);
    }

    /// <summary>
    /// Méthode qui détruit la rétroaction, appelée en fin d'animation
    /// </summary>
    public void Detruire()
    {
        Destroy(gameObject);
    }
}
