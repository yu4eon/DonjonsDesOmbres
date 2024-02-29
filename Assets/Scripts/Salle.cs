using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;


/// <summary>
/// Classe pour le conteneur des tuiles, détermine la taille de la salle et affiche des informations sur la salle.
public class Salle : MonoBehaviour
{
    //La taille d'une salle
    static Vector2Int _taille = new Vector2Int(30, 18);
    // Propriété pour accéder à la taille de la salle.
    static public Vector2Int taille => _taille;

    /// <summary>
    /// Utiliser purement pour afficher la taille de la salle
    /// </summary>
    public void Tester()
    {
        // Affiche des informations sur la salle dans la console.
        Debug.Log($"Planche {name} est à la position {transform.position} et a une taille de {taille} cases.");
    }

    /// <summary>
    /// Méthode de Unity pour les Gizmos
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, (Vector2)_taille);
    }
}
