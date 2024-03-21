using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joyau : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] float _valeur = 0.1f; // Valeur du joyau
    [SerializeField] Retroaction _retroModele;
    // [SerializeField] SOPerso _donneesPerso;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Niveau.instance.LibererUnePos(transform.position);
            Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent);
            retro.ChangerTexte("+" + _valeur + " or");
            // _donneesPerso.argent += _valeur; //A ajouter quand on fusionne nos scripts
            // _donneesPerso.AjouterArgent(_valeur); //Alternatif meilleur
            Destroy(gameObject);
        }
    }
}
