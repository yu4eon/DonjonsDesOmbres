using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

// using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Ennemi : MonoBehaviour
{
    [SerializeField] AudioClip[] _sonEnnemi; // Sons de l'ennemi
    [SerializeField] TypePouvoir _typePouvoirEnnemi; // Type de pouvoir de l'ennemi
    [SerializeField] int _pointsDeVieIni = 100; // Points de vie initial de l'ennemi
    int _pointsDeVie; // Points de vie actuels de l'ennemi
    [SerializeField] int _valeurScore = 500; // Score donné par l'ennemi
    [SerializeField] int _degatsInfliges = 20; // Dégâts infligés par l'ennemi
    [SerializeField] Retroaction _retroModele; // Modèle de rétroaction lorsque l'ennemi prend des dégats
    [SerializeField] SOPerso _donneesPerso; // Données du joueur
    [SerializeField] Color _couleurEndommage = new Color(1, 0.6f, 0.6f); // Couleur de l'ennemi lorsqu'il est endommagé
    [SerializeField] GameObject _contenantBarreVie; // Contenant de la barre de vie de l'ennemi
    [SerializeField] GameObject _barreVie; // Barre de vie de l'ennemi
    Light2D _lumiere; // Lumière de l'ennemi
    float _delaiCouleur = 0.3f; // Délai pour reajuster la couleur de l'ennemi
    SpriteRenderer _spriteRenderer; // Sprite de l'ennemi
    bool _estInvulnerable = false; // Indique si l'ennemi est invulnérable
    bool _degatCritique = false; // Indique si l'ennemi a subi un dégât critique

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
        _animator = GetComponent<Animator>();
        _lumiere = GetComponentInChildren<Light2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        

        Initialiser();

        

        
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
        if(_estInvulnerable) return; // Si l'ennemi est invulnérable, ne fait rien
        Debug.Log("L'ennemi subit " + degats + " dégâts de type " + typePouvoir);
        if(typePouvoir == _typePouvoirEnnemi)
        {
            degats *= 2;
            _degatCritique = true;
        }
        // if (_faiblesses[typePouvoir] == _typePouvoirEnnemi)
        // {
        //     Debug.Log(_faiblesses[typePouvoir] + " " + _typePouvoirEnnemi);
        //     degats *= 2; // Double les dégâts si l'ennemi est faible contre le pouvoir
        //     Debug.Log("Double dégâts");
        //     _degatCritique = true;
        // }
        // else
        // {
        //     Debug.Log("Dégâts normaux");
        // }
        _estInvulnerable = true; // L'ennemi est invulnérable
        _spriteRenderer.color = _couleurEndommage; // Change la couleur de l'ennemi
        StartCoroutine(CoroutineReajusterCouleur()); // Réajuste la couleur de l'ennemi
        Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent);
        if(_degatCritique)
        {
            retro.ChangerTexte("-" + degats + "!", "#FF3535", 1f, 2f);
        }
        else
        {
            retro.ChangerTexte("-" + degats, "#FFE32E");
        }
        Mathf.Clamp(_pointsDeVie -= degats, 0, _pointsDeVieIni); // Réduit les points de vie de l'ennemi
        float fractionVie = (float)_pointsDeVie / _pointsDeVieIni;
        Debug.Log("Fraction de vie : " + fractionVie);
        _barreVie.transform.localScale = new Vector3(fractionVie, 1, 1);

        Debug.Log("Points de vie restants : " + _pointsDeVie);
        // Réduit les points de vie de l'ennemi
        if(_pointsDeVie <= 0)
        {
            Mourir();
        }
    }

    IEnumerator CoroutineReajusterCouleur()
    {
        yield return new WaitForSeconds(_delaiCouleur);
        _spriteRenderer.color = Color.white;
        _estInvulnerable = false;
    }

    void Mourir()
    {
        Debug.Log("L'ennemi est mort");
        _contenantBarreVie.SetActive(false);
        _barreVie.SetActive(false);
        _donneesPerso.AjouterScore(_valeurScore);
        // _animator.SetTrigger("Meurt");
        gameObject.SetActive(false);
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
            Debug.Log("Collision avec le joueur");
            Perso perso = other.gameObject.GetComponent<Perso>();
            perso.SubirDegats(_degatsInfliges);
        }
    }
}
