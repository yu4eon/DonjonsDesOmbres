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
    [SerializeField] TypePouvoir _pouvoir; // Type de pouvoir de l'autel.
    public TypePouvoir pouvoir { get => _pouvoir;} // Propriété pour accéder à l'élément de l'autel. #synthese Leon
    [SerializeField] SOPerso _donneesPerso; // ScriptableObject contenant les données du personnage.
    [SerializeField] SpriteRenderer _spriteEteint; // SpriteRenderer de l'autel lorsqu'il est éteint.
    [SerializeField] Retroaction _modeleRetro; // Modèle de rétroaction pour l'autel.
    [SerializeField] AudioClip _sonAutel;
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
        if(collision.CompareTag("Player") && estActif)
        {
            SoundManager.instance.JouerEffetSonore(_sonAutel);
            Debug.Log("Element : " + _pouvoir); // Affiche l'élément de l'autel.
            _donneesPerso.AjouterPouvoir(_pouvoir); // Ajoute le pouvoir de l'autel au personnage.
            _donneesPerso.evenementMiseAJour.Invoke(); // Déclenche l'événement de mise à jour des données du personnage. #tp4 Leon
            Desactiver(); // Désactive l'autel.
            shape.scale = new Vector3(4f, 5f, 1f); // Modifie l'échelle des particules.
            emission.rateOverTime = 100f; // Modifie le taux d'émission des particules.
            Retroaction retro = Instantiate(_modeleRetro, transform.position, Quaternion.identity); // Instancie une rétroaction.
            string couleur;
            switch (_pouvoir) // Change la couleur de la rétroaction selon le pouvoir obtenu.
            {
                case TypePouvoir.Glace:
                    couleur = "#c1dee2";
                    break;
                case TypePouvoir.Foudre:
                    couleur = "#ff9900";
                    break;
                case TypePouvoir.Ombre:
                    couleur = "#9946ff";
                    break;
                case TypePouvoir.Poison:
                    couleur = "#65b872";
                    break;
                default:
                    couleur = "#FFFFFF";
                    break;
            }

            retro.ChangerTexte("Pouvoir obtenu : " + _pouvoir, couleur, 0.5f); // Change le texte de la rétroaction.
            UIJeu.instance.JouerParticulesPouvoir((int)_pouvoir); // Joue les particules du pouvoir.
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

    void Desactiver()
    {
        _lumiere.intensity = 0; // Définit l'intensité de la lumière de l'autel à 0 #tp4 Leon
        sr.sprite = _spriteEteint.sprite; // Change le sprite de l'autel pour indiquer qu'il est éteint.
        StartCoroutine(ArreterParticules()); // Démarre la coroutine pour arrêter les particules.
        estActif = false; // Désactive l'autel.
    }
}
