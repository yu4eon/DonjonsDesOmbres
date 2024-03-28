using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autels : MonoBehaviour
{
    [SerializeField] TypePouvoir element; // Type de pouvoir de l'autel.
    [SerializeField] SOPerso _donneesPerso; // ScriptableObject contenant les données du personnage.
    [SerializeField] SpriteRenderer _spriteEteint; // SpriteRenderer de l'autel lorsqu'il est éteint.
    SpriteRenderer sr; // Composant SpriteRenderer de l'autel.
    ParticleSystem ps; // Système de particules de l'autel.
    ParticleSystem.ShapeModule shape; // Module de forme des particules.
    ParticleSystem.EmissionModule emission; // Module d'émission des particules.
    bool estActif = false; // Indique si l'autel est actif ou non.
    Sprite _sBase; // Sprite de base de l'autel.

    /// <summary>
    /// #Tp3 Antoine
    /// Awake est appelé lorsque l'instance du script est chargée.
    /// </summary>
    void Awake()
    {
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
            _donneesPerso.AjouterPouvoir(element); // Ajoute le pouvoir de l'autel au personnage.
            sr.sprite = _spriteEteint.sprite; // Change le sprite de l'autel pour indiquer qu'il est éteint.
            shape.scale = new Vector3(4f, 5f, 1f); // Modifie l'échelle des particules.
            emission.rateOverTime = 100f; // Modifie le taux d'émission des particules.
            StartCoroutine(ArreterParticules()); // Démarre la coroutine pour arrêter les particules.
        }
    }

    IEnumerator ArreterParticules()
    {
        yield return new WaitForSeconds(1.5f); // Attend 1.5 secondes.
        ps.Stop(); // Arrête le système de particules.

    /// <summary>
    /// #Tp3 Antoine
    /// Active l'autel.
    /// </summary>
    void Activer()
    {
        estActif = true; // Active l'autel.
        sr.sprite = _sBase; // Change le sprite de l'autel à l'état actif.
        ps.Play(); // Démarre le système de particules.
    }
}
