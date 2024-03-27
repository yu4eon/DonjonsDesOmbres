using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectorVitesse : MonoBehaviour
{
    // ParticleSystem ps;
    // bool estActif = false;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    // void Awake()
    // {
    //     ps = GetComponentInChildren<ParticleSystem>();
    //     ps.Stop();
    //     estActif = false;
    // }

    // void Start()
    // {
    //     Activateur.instance.evenementActivateur.AddListener(Activer);
    // }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") ) //&& estActif Pas mal sur que les effectors sont tout le temps actifs, donc j'ai enlevé cette condition.
        {
            other.GetComponent<Perso>().AugmenterVitesse();
        }
    }


    // void Activer()
    // {
    //     estActif = true;
    //     ps.Play();
    // }

    //Note : J'ai enlevé les parties ou on désactivent les effectors, car je crois qu'ils sont toujours actifs.
}
