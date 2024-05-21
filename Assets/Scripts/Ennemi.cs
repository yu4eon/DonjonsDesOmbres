using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.Universal;

public class Ennemi : MonoBehaviour
{
    // [SerializeField] AudioClip[] _sonEnnemi; // Sons de l'ennemi
    // protected AudioClip[] sonEnnemi =>_sonEnnemi; // Variable qui contient le son de l'ennemi
    [SerializeField] AudioClip _sonDegat; // Son lorsqu'un ennemi prend des dégâts
    [SerializeField] AudioClip _sonMort; // Son lorsqu'un ennemi meurt
    [SerializeField] AudioClip _sonDeplacement; // Son de déplacement de l'ennemi
    [SerializeField] float _distanceSon = 20f; // Distance à laquelle le son de déplacement de l'ennemi est joué
    [SerializeField] TypePouvoir _typePouvoirEnnemi; // Type de pouvoir de l'ennemi
    [SerializeField] int _pointsDeVieIni = 100; // Points de vie initial de l'ennemi
    int _pointsDeVie; // Points de vie actuels de l'ennemi
    [SerializeField] int _valeurScore = 500; // Score donné par l'ennemi
    [SerializeField] int _valeurArgent = 15; // Argent donné par l'ennemi
    [SerializeField] int _degatsInfliges = 20; // Dégâts infligés par l'ennemi
    [SerializeField] Retroaction _retroModele; // Modèle de rétroaction lorsque l'ennemi prend des dégats
    [SerializeField] SOPerso _donneesPerso; // Données du joueur
    [SerializeField] Color _couleurEndommage = new Color(1, 0.6f, 0.6f); // Couleur de l'ennemi lorsqu'il est endommagé
    [SerializeField] GameObject _contenantBarreVie; // Contenant de la barre de vie de l'ennemi
    [SerializeField] GameObject _barreVie; // Barre de vie de l'ennemi
    private Color _couleurIni; // Couleur de base de l'ennemi
    Light2D _lumiere; // Lumière de l'ennemi
    float _delaiCouleur = 0.3f; // Délai pour reajuster la couleur de l'ennemi
    SpriteRenderer _spriteRenderer; // Sprite Renderer de l'ennemi
    Rigidbody2D _rb; // Rigidbody de l'ennemi
    bool _estInvulnerable = false; // Indique si l'ennemi est invulnérable
    bool _degatCritique = false; // Indique si l'ennemi a subi un dégât critique
    protected Perso _perso; // référence du joueur
    public Perso perso { get => _perso; set => _perso = value; }

    // Dictionnaire des faiblesses de chaque pouvoir (a changer selon les demandes de l'artiste)
    // Dictionary<TypePouvoir, TypePouvoir> _faiblesses = new Dictionary<TypePouvoir, TypePouvoir>
    // {
    //     { TypePouvoir.Poison, TypePouvoir.Glace },
    //     { TypePouvoir.Ombre, TypePouvoir.Foudre },
    //     { TypePouvoir.Foudre, TypePouvoir.Poison },
    //     { TypePouvoir.Glace, TypePouvoir.Ombre }
    // };

    Animator _animator; // Animator de l'ennemi

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _lumiere = GetComponentInChildren<Light2D>();
    }
    // Start is called before the first frame update
    virtual protected void Start()
    {
        ChoisirTypePouvoir();
        Initialiser();
        _couleurIni = _spriteRenderer.color;
    }

    virtual protected void FixedUpdate()
    {
        _animator.SetFloat("X", _rb.velocity.x);
    }

    void ChoisirTypePouvoir()
    {
        _typePouvoirEnnemi = (TypePouvoir)Random.Range(0, 4);
    }

    void Initialiser()
    {
        _pointsDeVieIni = Mathf.FloorToInt((float)_pointsDeVieIni + ((float)_donneesPerso.niveau/4 * (float)_pointsDeVieIni));
        Debug.Log("Points de vie de l'ennemi : " + _pointsDeVieIni);
        _pointsDeVie = _pointsDeVieIni;
        _contenantBarreVie.SetActive(false);
        _barreVie.SetActive(false);

        switch (_typePouvoirEnnemi)
        {
        case TypePouvoir.Poison:
            _lumiere.color = new Color(0.302f, 0.55f, 0.34f);
            break;
        case TypePouvoir.Ombre:
            _lumiere.color = new Color(0.2f, 0, 0.45f);
            break;
        case TypePouvoir.Foudre:
            _lumiere.color = new Color(1, 0.6f, 0);
            break;
        default:
            _lumiere.color = new Color(0.76f, 0.87f,0.89f);
            break;
        }


    }

    public void SubirDegats(int degats, TypePouvoir typePouvoir)
    {
        _contenantBarreVie.SetActive(true);
        _barreVie.SetActive(true);
        _degatCritique = false;
        if (_estInvulnerable) return; // Si l'ennemi est invulnérable, ne fait rien
        // Debug.Log("L'ennemi subit " + degats + " dégâts de type " + typePouvoir);
        // GestAudio.instance.JouerEffetSonore(_sonEnnemi[1]);
        GestAudio.instance.JouerEffetSonore(_sonDegat);
        if (typePouvoir == _typePouvoirEnnemi)
        {
            degats *= 2;
            _degatCritique = true;
        }
        _estInvulnerable = true; // L'ennemi est invulnérable
        _spriteRenderer.color = _couleurEndommage; // Change la couleur de l'ennemi
        StartCoroutine(CoroutineReajusterCouleur()); // Réajuste la couleur de l'ennemi
        Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent);
        if (_degatCritique)
        {
            retro.ChangerTexte("-" + degats + "!", "#FF3535", 1f, 2f);
        }
        else
        {
            retro.ChangerTexte("-" + degats, "#FFE32E");
        }
        _pointsDeVie = Mathf.Clamp(_pointsDeVie - degats, 0, _pointsDeVieIni); // Réduit les points de vie de l'ennemi
        float fractionVie = (float)_pointsDeVie / _pointsDeVieIni;
        Debug.Log(degats + " dégâts infligés");
        Debug.Log("Fraction de vie : " + fractionVie);
        _barreVie.transform.localScale = new Vector3(fractionVie, 1, 1);

        Debug.Log("Points de vie restants : " + _pointsDeVie);
        // Réduit les points de vie de l'ennemi
        if (_pointsDeVie <= 0)
        {
            Mourir();
            // GestAudio.instance.JouerEffetSonore(_sonEnnemi[2]);
        }
    }

    IEnumerator CoroutineReajusterCouleur()
    {
        yield return new WaitForSeconds(_delaiCouleur);
        _spriteRenderer.color = _couleurIni;
        _estInvulnerable = false;
    }

    void Mourir()
    {
        // Debug.Log("L'ennemi est mort");
        GestAudio.instance.JouerEffetSonore(_sonMort);
        _contenantBarreVie.SetActive(false);
        _barreVie.SetActive(false);
        _donneesPerso.AjouterArgent(_valeurArgent);
        _donneesPerso.AjouterScore(_valeurScore);
        // _animator.SetTrigger("Meurt");
        gameObject.SetActive(false);
    }

    public void JouerSonDeplacement()
    {
        // Joue le son de déplacement de l'ennemi si le joueur est à proximité
        if (Vector2.Distance(transform.position, perso.transform.position) < _distanceSon)
        {
            float fractionDistance = Vector2.Distance(transform.position, perso.transform.position) / _distanceSon;
            float volume = Mathf.Clamp(1 - fractionDistance, 0.1f, 1f);
            Debug.Log("Fraction distance : " + fractionDistance);
            GestAudio.instance.JouerEffetSonore(_sonDeplacement, volume);

        }

    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<Perso>() != null)
        {
            // Debug.Log("Collision avec le joueur");
            _animator.SetTrigger("ContactJoueur");
            Perso perso = other.gameObject.GetComponent<Perso>();
            perso.SubirDegats(_degatsInfliges);
        }
    }
}
