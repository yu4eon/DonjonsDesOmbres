using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joyau : MonoBehaviour
{
    [SerializeField, Range(0, 20)] int _valeur = 1; // Valeur du joyau
    [SerializeField] Retroaction _retroModele;
    [SerializeField] SOPerso _donneesPerso;


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
