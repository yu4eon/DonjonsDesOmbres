using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// #tp3
/// Auteur du code : Léon Yu
/// Commetaires ajoutés par : Léon Yu
/// classe qui gère les joyaux, qui sont des objets que le joueur peut ramasser pour obtenir de l'or
/// </summary>
public class Joyau : MonoBehaviour
{
    [SerializeField, Range(0, 20)] int _valeur = 1; // Valeur du joyau en argent
    [SerializeField] Retroaction _retroModele; // Modèle de rétroaction lorsque le joueur rammase le joyau
    [SerializeField] SOPerso _donneesPerso; // Données du personnage, (ScriptableObject)
    [SerializeField] AudioClip _sonJoyaux; // Son quand le joueur ramasse le joyau


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //Si le joueur entre en collision avec le joyau
        {
            Niveau.instance.LibererUnePos(transform.position); //Libérer la position du joyau
            Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent);
            retro.ChangerTexte("+" + _valeur + " or");
            _donneesPerso.AjouterArgent(_valeur); //Ajouter l'argent au joueur
            _donneesPerso.AjouterScore(_valeur * 10); //Ajouter le score au joueur
            Destroy(gameObject);
            GestAudio.instance.JouerEffetSonore(_sonJoyaux);
        }
    }
}
