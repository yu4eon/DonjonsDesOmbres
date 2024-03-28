using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // Ajout de la référence aux UnityEvents

/// <summary>
/// #tp3 Léon
/// Classe qui gère l'activateur d'un élément interactif.
/// </summary>
public class Activateur : MonoBehaviour
{
    [SerializeField] Sprite _sActif; // Sprite de l'activateur actif.
    [SerializeField] Sprite _sInactif; // Sprite de l'activateur inactif.

    static Activateur _instance; // Instance statique de la classe Activateur.
    static public Activateur instance => _instance; // Propriété publique qui permet l'accès à l'instance de la classe Activateur.

    UnityEvent _evenementActivateur = new UnityEvent(); // Événement Unity déclenché par l'activateur.
    public UnityEvent evenementActivateur => _evenementActivateur; // Propriété publique qui permet l'accès à l'événement de l'activateur.

    SpriteRenderer _sr; // Composant SpriteRenderer de l'activateur.

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject); // Détruit l'objet s'il y a déjà une instance de l'activateur.
            return;
        }
        _instance = this;

        _sr = GetComponent<SpriteRenderer>(); // Obtient le composant SpriteRenderer de l'activateur.
    }

    // Start is called before the first frame update
    void Start()
    {
        _sr.sprite = _sInactif; // Définit le sprite inactif au démarrage.
    }

    /// <summary>
    /// #tp3 Léon
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        _sr.sprite = _sActif; // Change le sprite de l'activateur à l'état actif.
        _evenementActivateur.Invoke(); // Déclenche l'événement de l'activateur.
    }
}
