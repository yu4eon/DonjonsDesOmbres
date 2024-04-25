using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// #tp3
/// Auteur du code : Antoine Lachance
/// Commetaires ajoutés par : Antoine Lachance
/// </summary>
public class Autels : MonoBehaviour
{
    [SerializeField] TypePouvoir element; // Type de pouvoir de l'autel.
    [SerializeField] SOPerso _donneesPerso; // ScriptableObject contenant les données du personnage.
    [SerializeField] SpriteRenderer _spriteEteint; // SpriteRenderer de l'autel lorsqu'il est éteint.
    Light2D _lumiere; // Référence à la lumière de l'autel #tp4 Leon
    SpriteRenderer sr; // Composant SpriteRenderer de l'autel.
    ParticleSystem ps; // Système de particules de l'autel.
    ParticleSystem.ShapeModule shape; // Module de forme des particules.
    ParticleSystem.EmissionModule emission; // Module d'émission des particules.
    bool estActif = false; // Indique si l'autel est actif ou non.
    int _intensiteLumiereActif = 5; // Intensité de la lumière de l'autel #tp4 Leon
    Sprite _sBase; // Sprite de base de l'autel.

    /// <summary>
    /// #Tp3 Antoine
    /// Awake est appelé lorsque l'instance du script est chargée.
    /// </summary>
    void Awake()
    {
        _lumiere = GetComponentInChildren<Light2D>(); // Obtient la lumière de l'autel.
        sr = GetComponent<SpriteRenderer>(); // Obtient le composant SpriteRenderer de l'autel.
        ps = GetComponentInChildren<ParticleSystem>(); // Obtient le système de particules de l'autel.
        shape = ps.shape; // Obtient le module de forme des particules.
        emission = ps.emission; // Obtient le module d'émission des particules.
        estActif = false; // Initialise l'état actif de l'autel à faux.
        ps.Stop(); // Arrête le système de particules.

        _sBase = sr.sprite; // Sauvegarde le sprite de base de l'autel.
        sr.sprite = _spriteEteint.sprite; // Définit le sprite de l'autel comme étant éteint.
    }
    
    void Start()
    {
        _lumiere.intensity = 0; // Définit l'intensité de la lumière de l'autel à 0 #tp4 Leon
        Activateur.instance.evenementActivateur.AddListener(Activer); // Ajoute l'écouteur pour activer l'autel.
    }
    
    /// <summary>
    /// #Tp3 Antoine
    /// Gère la collision avec le joueur pour activer l'autel.
    /// </summary>
    /// <param name="collision">Collider2D de la collision.</param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && ps.isPlaying)
        {
            Debug.Log("Element : " + element); // Affiche l'élément de l'autel.
            _lumiere.intensity = 0; // Définit l'intensité de la lumière de l'autel à 0 #tp4 Leon
            _donneesPerso.AjouterPouvoir(element); // Ajoute le pouvoir de l'autel au personnage.
            _donneesPerso.evenementMiseAJour.Invoke(); // Déclenche l'événement de mise à jour des données du personnage. #tp4 Leon
            sr.sprite = _spriteEteint.sprite; // Change le sprite de l'autel pour indiquer qu'il est éteint.
            shape.scale = new Vector3(4f, 5f, 1f); // Modifie l'échelle des particules.
            emission.rateOverTime = 100f; // Modifie le taux d'émission des particules.
            StartCoroutine(ArreterParticules()); // Démarre la coroutine pour arrêter les particules.
        }
    }
    /// <summary>
    /// #Tp3 Antoine
    /// Coroutine qui arrête le système de particules.
    /// </summary>
    IEnumerator ArreterParticules()
    {
        yield return new WaitForSeconds(1.5f); // Attend 1.5 secondes.
        ps.Stop(); // Arrête le système de particules.
    }
    /// <summary>
    /// #Tp3 Antoine
    /// Active l'autel.
    /// </summary>
    void Activer()
    {
        _lumiere.intensity = _intensiteLumiereActif; // Définit l'intensité de la lumière de l'autel à 3 #tp4 Leon
        estActif = true; // Active l'autel.
        sr.sprite = _sBase; // Change le sprite de l'autel à l'état actif.
        ps.Play(); // Démarre le système de particules.
    }
}
