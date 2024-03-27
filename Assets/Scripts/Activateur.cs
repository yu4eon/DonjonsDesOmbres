using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // Ajout de la référence aux UnityEvents


public class Activateur : MonoBehaviour
{
    [SerializeField] Sprite _sActif;
    [SerializeField] Sprite _sInactif;

    static Activateur _instance;
    static public Activateur instance => _instance;

    UnityEvent _evenementActivateur = new UnityEvent();
    public UnityEvent evenementActivateur => _evenementActivateur;

    SpriteRenderer _sr;

    // Start is called before the first frame update
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        if (_sr.sprite != _sInactif)
        {
            _sr.sprite = _sInactif;
        }
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        _sr.sprite = _sActif;
        _evenementActivateur.Invoke();

        // if (Input.GetKeyDown(KeyCode.E)) // Exemple : déclenche l'événement quand la touche E est pressée
        // {
        //     _sr.sprite = _sActif;
        //     _evenementActivateur.Invoke(); // Appel de l'événement
        // }
        
    }

}
