using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal; // Ajout de la référence aux UnityEvents

/// <summary>
/// #tp3 Léon
/// Classe qui gère l'activateur d'un élément interactif.
/// </summary>
public class Activateur : MonoBehaviour
{
    [SerializeField] Sprite _sActif; // Sprite de l'activateur actif.
    [SerializeField] Sprite _sInactif; // Sprite de l'activateur inactif.
    [SerializeField] Retroaction _modeleRetro; // Modèle de rétroaction #synthese Leon 
    Light2D _lumiere; // Lumière de l'activateur #tp4 Leon

    static Activateur _instance; // Instance statique de la classe Activateur.
    static public Activateur instance => _instance; // Propriété publique qui permet l'accès à l'instance de la classe Activateur.

    UnityEvent _evenementActivateur = new UnityEvent(); // Événement Unity déclenché par l'activateur.
    public UnityEvent evenementActivateur => _evenementActivateur; // Propriété publique qui permet l'accès à l'événement de l'activateur.
    SpriteRenderer _sr; // Composant SpriteRenderer de l'activateur.
    int _intensiteLumiereActif = 3; // Intensité de la lumière de l'activateur #tp4 Leon
    int _intensiteLumiereInactif = 0; // Intensité de la lumière de l'activateur #tp4 Leon
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject); // Détruit l'objet s'il y a déjà une instance de l'activateur.
            return;
        }
        _instance = this;
        _lumiere = GetComponentInChildren<Light2D>(); // Obtient la lumière de l'activateur.
        _sr = GetComponent<SpriteRenderer>(); // Obtient le composant SpriteRenderer de l'activateur.
    }

    // Start is called before the first frame update
    void Start()
    {
        _lumiere.intensity = _intensiteLumiereInactif; // Définit l'intensité de la lumière de l'activateur à 1 #tp4 Leon
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
        if (_sr.sprite == _sInactif)
        {
            _sr.sprite = _sActif; // Change le sprite de l'activateur à l'état actif.
            _lumiere.intensity = _intensiteLumiereActif; // Augmente l'intensité de la lumière de l'activateur à 3 #tp4 Leon

            Retroaction retro = Instantiate(_modeleRetro, transform.position, Quaternion.identity); // Instancie une rétroaction.
            retro.ChangerTexte("Autels activés"); // Change le texte de la rétroaction.

            _evenementActivateur.Invoke(); // Déclenche l'événement de l'activateur.
        }
    }
}
