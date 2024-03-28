using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>

/// Auteur du code : Léon Yu
/// Commetaires ajoutés par : Léon Yu
/// classe qui gère les joyaux, qui sont des objets que le joueur peut ramasser pour obtenir de l'or
public class Joyau : MonoBehaviour
{
    [SerializeField, Range(0, 20)] int _valeur = 1; // Valeur du joyau en argent
    [SerializeField] Retroaction _retroModele; // Modèle de rétroaction lorsque le joueur rammase le joyau
    [SerializeField] SOPerso _donneesPerso; // Données du personnage, (ScriptableObject)


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Niveau.instance.LibererUnePos(transform.position);
            Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent);
            retro.ChangerTexte("+" + _valeur + " or");
            _donneesPerso.AjouterArgent(_valeur); //Alternatif meilleur
            Destroy(gameObject);
        }
    }
}
