using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class Ennemi : MonoBehaviour
{
    [SerializeField] TypePouvoir _typePouvoirEnnemi; // Type de pouvoir de l'ennemi
    [SerializeField] int _pointsDeVieIni = 100; // Points de vie initial de l'ennemi
    [SerializeField] int _scoreDonnee = 100; // Score donné par l'ennemi
    [SerializeField] int _argentDonnee = 20; // Argent donné par l'ennemi
    [SerializeField] Retroaction _retroModele; // Modèle de rétroaction lorsque l'ennemi prend des dégats
    [SerializeField] Color _couleurEndommage = new Color(1, 0.6f, 0.6f); // Couleur de l'ennemi lorsqu'il est endommagé
    int _pointsDeVie; // Points de vie actuels de l'ennemi
    float _delaiCouleur = 0.4f; // Délai pour reajuster la couleur de l'ennemi
    SpriteRenderer _spriteRenderer; // Sprite de l'ennemi
    bool _estInvulnerable = false; // Indique si l'ennemi est invulnérable

    // Dictionnaire des faiblesses de chaque pouvoir (a changer selon les demandes de l'artiste)
    Dictionary<TypePouvoir, TypePouvoir> _faiblesses = new Dictionary<TypePouvoir, TypePouvoir>
    {
        { TypePouvoir.Poison, TypePouvoir.Glace },
        { TypePouvoir.Ombre, TypePouvoir.Foudre },
        { TypePouvoir.Foudre, TypePouvoir.Poison },
        { TypePouvoir.Glace, TypePouvoir.Ombre }
    };

    Animator _animator; // Animator de l'ennemi

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _pointsDeVie = _pointsDeVieIni;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SubirDegats(int degats, TypePouvoir typePouvoir)
    {
        if(_estInvulnerable) return; // Si l'ennemi est invulnérable, ne fait rien
        Debug.Log("L'ennemi subit " + degats + " dégâts de type " + typePouvoir);
        if (_faiblesses[typePouvoir] == _typePouvoirEnnemi)
        {
            Debug.Log(_faiblesses[typePouvoir] + " " + _typePouvoirEnnemi);
            degats *= 2; // Double les dégâts si l'ennemi est faible contre le pouvoir
            Debug.Log("Double dégâts");
        }
        else
        {
            Debug.Log("Dégâts normaux");
        }
        _estInvulnerable = true; // L'ennemi est invulnérable
        _spriteRenderer.color = _couleurEndommage; // Change la couleur de l'ennemi
        StartCoroutine(CoroutineReajusterCouleur()); // Réajuste la couleur de l'ennemi
        Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent);
        retro.ChangerTexte("-" + degats, "#FF3535");
        _pointsDeVie -= degats; // Réduit les points de vie de l'ennemi
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
            // Perso perso = other.gameObject.GetComponent<Perso>();
        }
    }
}
