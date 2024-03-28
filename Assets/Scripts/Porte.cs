using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porte : MonoBehaviour
{
    [SerializeField] SONavigation _navigation;
    [SerializeField] Sprite[] _sprites;
    [SerializeField] SOPerso _donneesPerso;

    SpriteRenderer _sr;
    static public bool aCle = false;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        aCle = false;
        _sr = GetComponent<SpriteRenderer>();
        _sr.sprite = _sprites[0];
    }


    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (aCle && other.CompareTag("Player"))
        {
            _sr.sprite = _sprites[1];

            // Leon : J'ai changer le Invoke en Coroutine
            Coroutine _coroutine = StartCoroutine(ChangerScene());
            
        }
    }

    /// <summary>
    /// Coroutine qui change de scène après 2 secondes et vide l'inventaire du joueur
    /// </summary>
    IEnumerator ChangerScene()
    {
        yield return new WaitForSeconds(2f);
        _donneesPerso.ViderInventaire();
        _navigation.AllerSceneSuivante();
    }
}
