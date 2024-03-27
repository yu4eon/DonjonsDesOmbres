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

    void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        _sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _sr.sprite = _sInactif;
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        _sr.sprite = _sActif;
        _evenementActivateur.Invoke();
    }

}
