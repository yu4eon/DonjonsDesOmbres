using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmePerso : MonoBehaviour
{
    [SerializeField] SOArme[] _tDonneesArmes; // Tableau des données des armes
    [SerializeField] SOPerso _donneesPerso; // Données du joueur
    // int _indexPouvoir; // Index du pouvoir
    SOArme _armeEquipee; // Arme équipée par le joueur
    SpriteRenderer _spriteRenderer; // Sprite de l'arme
    Animator _animator; // Animator de l'arme
    Perso _perso; // Script du joueur
    Vector3 _positionInitiale; // Position initiale de l'arme
    Collider2D _collider; // Collider de l'arme
    bool _estLeger; // Si l'attaque est léger


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
        DesactiverArme();
    }
    public void InitialiserArme(TypePouvoir typePouvoir, bool estLeger)
    {

        _estLeger = estLeger;
        _armeEquipee = _tDonneesArmes[(int)typePouvoir];


        // Jouer animation de l'arme
        string nomAnimation = _armeEquipee.nom;
        _animator.SetTrigger(nomAnimation);
        _animator.SetBool("estLeger", estLeger);
        Debug.Log("Arme activée");
        // _spriteRenderer.sprite = _armeEquipee.sprite;

    }



    public void ChangerDirection(bool _estGauche)
    {
        // Debug.Log(_armeEquipee.nom);
        // Debug.Log("Changer direction " + _estGauche);
        if (_estGauche)
        {
            // Debug.Log(transform.localPosition);
            transform.localPosition = new Vector3(-_positionInitiale.x, _positionInitiale.y, _positionInitiale.z);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            // Debug.Log(transform.localPosition);
            transform.localPosition = _positionInitiale;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void DesactiverCollider()
    {
        _collider.enabled = false;
    }
    public void ActiverCollider()
    {
        _collider.enabled = true;
    }
    /// <summary>
    /// Sera appelé pour desactiver l'arme par animation event plus tard, pour l'instant, on l'appelleras par Coroutine
    /// </summary>
    public void DesactiverArme()
    {
        // _perso = GetComponentInParent<Perso>();
        DesactiverCollider();
        _perso.PermettreAttaque();
        gameObject.SetActive(false);
        // Désactiver l'arme
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // _armeEquipee = _tDonneesArmes[_indexPouvoir];
        // Debug.Log(this.gameObject.name);
        // Si le joueur entre en collision avec un ennemi
        if (other.GetComponent<Ennemi>() != null)
        {
            // Debug.Log(_armeEquipee.nom);
            Debug.Log(_armeEquipee.nom);
            Debug.Log("Collision avec ennemi");
            Ennemi ennemi = other.GetComponent<Ennemi>();
            float degatInfligee = _armeEquipee.degats * _donneesPerso.attaque;
            if(!_estLeger) degatInfligee *= 2;
            ennemi.SubirDegats(Mathf.CeilToInt(degatInfligee), _armeEquipee.typePouvoir);
        }

    }
}
