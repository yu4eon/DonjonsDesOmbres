using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


/// <summary>
/// Classe qui gère l'apparition des tuiles sur la tilemap principal
/// </summary>
public class CarteTuiles : MonoBehaviour
{
    [SerializeField]
    [Range(0, 100)]
    int _chanceApparition = 100; // Chance d'apparition des tuiles
    Tilemap _tilemap; // Tilemap du gameobject

    void Awake()
    {
        _tilemap = GetComponent<Tilemap>(); 

        BoundsInt bounds = _tilemap.cellBounds; 
        Niveau niveau = GetComponentInParent<Niveau>(); 
        Vector3Int decalage = Vector3Int.FloorToInt(transform.position); 

        // Détermine si les tuiles doit apparaître par un tirage aléatoire
        if (Random.Range(0, 100) < _chanceApparition)
        {
            // Parcourt toutes les tuiles de la tilemap sur l'axe x
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                // Parcourt toutes les tuiles de la tilemap sur l'axe y
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    // Ajoute la tuile à la tilemap principal
                    niveau.AjouterTuile(_tilemap, niveau, y, x, decalage);
                }
            }
            gameObject.SetActive(false);
        }
        // Si les tuiles n'apparaissent pas
        else 
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Méthode qui est appelée lorsque le script est chargé.
    /// </summary>
    void OnValidate()
    {
        // P.S : j'ai ajouté cette ligne puisque sinon Unity ne met pas à jour la couleur de la tilemap dans l'éditeur :
        _tilemap = GetComponent<Tilemap>();  
        _tilemap.color = new Color(1, 1, 1, _chanceApparition / 100f); 
    }
}
