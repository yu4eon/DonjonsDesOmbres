using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Auteur du code : Léon Yu, Antoine Lachance
/// Commentaires ajoutés par : Léon Yu, Antoine Lachance
/// Classe qui contrôle les armes du personnage.
/// </summary>
public class ArmePerso : MonoBehaviour
{
    [SerializeField] SOArme[] _tDonneesArmes; // Tableau des données des armes.
    [SerializeField] SOPerso _donneesPerso; // Données du personnage.
    [SerializeField] Vector3 _tailleJavelin = new Vector3(0.8f, 0.8f, 0.8f); // Taille de la javelin.
    [SerializeField] Vector3 _tailleMarteau = new Vector3(1.2f, 1.2f, 1.2f); // Taille du marteau.
    [SerializeField] float _delaiAlpha = 0.05f; // Délai pour changer l'alpha de l'arme.
    SOArme _armeEquipee; // Arme équipée par le joueur.
    SpriteRenderer _spriteRenderer; // Sprite de l'arme.
    Animator _animator; // Animator de l'arme.
    Perso _perso; // Script du personnage.
    Vector3 _positionInitiale; // Position initiale de l'arme.
    Collider2D _collider; // Collider de l'arme.
    bool _estLeger; // Si l'attaque est légère.
    bool _estGauche; // Si l'attaque est à gauche.

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
    }

    void Start()
    {
        _collider.enabled = false;
        _positionInitiale = transform.localPosition;
        _perso = GetComponentInParent<Perso>();
        DesactiverArme(); // Désactive l'arme au démarrage.
    }

    /// <summary>
    /// Initialise l'arme en fonction du type de pouvoir et de l'attaque légère ou lourde.
    /// </summary>
    /// <param name="typePouvoir">Le type de pouvoir de l'arme.</param>
    /// <param name="estLeger">Indique si l'attaque est légère.</param>
    public void InitialiserArme(TypePouvoir typePouvoir, bool estLeger)
    {
        _estLeger = estLeger;
        _armeEquipee = _tDonneesArmes[(int)typePouvoir];
        GestAudio.instance.JouerEffetSonore(_armeEquipee.sonAttaque);
        if (_armeEquipee == null) Debug.LogWarning("Arme non trouvée");
        Coroutine coroutine = StartCoroutine(CoroutineChangerAlpha());

        // Ajuste la taille de l'arme en fonction de son type.
        switch (_armeEquipee.nom)
        {
            case "Javelin":
                transform.localScale = _tailleJavelin;
                if (_estGauche)
                {
                    transform.localScale = new Vector3(-_tailleJavelin.x, _tailleJavelin.y, _tailleJavelin.z);
                }
                break;
            case "Marteau":
                transform.localScale = _tailleMarteau;
                if (_estGauche)
                {
                    transform.localScale = new Vector3(-_tailleMarteau.x, _tailleMarteau.y, _tailleMarteau.z);
                }
                break;
            default:
                transform.localScale = new Vector3(1, 1, 1);
                if (_estGauche)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                break;
        }

        // Joue l'animation de l'arme.
        string nomAnimation = _armeEquipee.nom;
        _animator.SetTrigger(nomAnimation);
        _animator.SetBool("estLeger", estLeger);
        Debug.Log("Arme activée");
    }

    /// <summary>
    /// Change la direction de l'arme en fonction de la direction du personnage.
    /// </summary>
    /// <param name="estGauche">Indique si l'arme doit être orientée à gauche.</param>
    public void ChangerDirection(bool estGauche)
    {
        Vector3 tailleActuelle = transform.localScale;
        if (estGauche)
        {
            _estGauche = true;
            transform.localPosition = new Vector3(-_positionInitiale.x, _positionInitiale.y, _positionInitiale.z);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            _estGauche = false;
            transform.localPosition = _positionInitiale;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    /// <summary>
    /// Désactive le collider de l'arme.
    /// </summary>
    public void DesactiverCollider()
    {
        _collider.enabled = false;
    }

    /// <summary>
    /// Active le collider de l'arme.
    /// </summary>
    public void ActiverCollider()
    {
        _collider.enabled = true;
    }

    /// <summary>
    /// Désactive l'arme.
    /// </summary>
    public void DesactiverArme()
    {
        DesactiverCollider();
        _perso.PermettreAttaque(); // Permet au personnage d'attaquer à nouveau.
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Coroutine pour changer progressivement l'alpha de l'arme.
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineChangerAlpha()
    {
        float alpha = 0;
        while (alpha < 1)
        {
            alpha += 0.2f;
            _spriteRenderer.color = new Color(1, 1, 1, alpha);
            yield return new WaitForSeconds(_delaiAlpha);
        }
    }

    /// <summary>
    /// Gère la collision de l'arme avec d'autres objets.
    /// </summary>
    /// <param name="other">Le collider de l'objet avec lequel l'arme entre en collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Ennemi>() != null) // Si l'arme entre en collision avec un ennemi.
        {
            Ennemi ennemi = other.GetComponent<Ennemi>();
            float degatInfligee = _armeEquipee.degats * _donneesPerso.attaque;
            if (!_estLeger) degatInfligee *= 2; // Double les dégâts si l'attaque n'est pas légère.
            ennemi.SubirDegats(Mathf.CeilToInt(degatInfligee), _armeEquipee.typePouvoir); // Inflige les dégâts à l'ennemi.
        }
    }
}
