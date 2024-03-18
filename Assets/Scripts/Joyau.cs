using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joyau : MonoBehaviour
{
    [SerializeField, Range(0, 20)] float _valeur = 5; // Valeur du joyau
    // [SerializeField] SOPerso _donneesPerso;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("Player"))
    //     {
    //         // Ajouter animation de texte plus tard.
    //         _donneesPerso.argent += _valeur;
    //         Destroy(gameObject);
    //     }
    // }
}
