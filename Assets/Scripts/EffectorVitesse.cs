using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// #tp3
/// Auteur du code : Léon Yu
/// Commetaires ajoutés par : Léon Yu
/// classe qui gère l'effector de vitesse, qui est actif en tout temps
public class EffectorVitesse : MonoBehaviour
{

    /// <summary>
    /// Simplement, lorsque le joueur entre en collision avec l'effector, on augmente sa vitesse.
    /// </summary>
    /// <param name="other">Collision, en ce moment, réagi seulement avec le joueur</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") )
        {
            other.GetComponent<Perso>().AugmenterVitesse();
        }

    }	
    //Note : J'ai enlevé les parties ou on désactivent les effectors, car je crois qu'ils sont toujours actifs.
}
