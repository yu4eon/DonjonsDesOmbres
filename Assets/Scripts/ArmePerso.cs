using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmePerso : MonoBehaviour
{
    [SerializeField] SOArme[] _tDonneesArmes; // Tableau des données des armes
    [SerializeField] SOPerso _donneesPerso; // Données du joueur
    SOArme _armeEquipee; // Arme équipée par le joueur
    SpriteRenderer _spriteRenderer; // Sprite de l'arme
    Animator _animator; // Animator de l'arme
    Perso _perso; // Script du joueur
    Vector3 _positionInitiale; // Position initiale de l'arme


    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _perso = GetComponentInParent<Perso>();
    }

    void Start()
    {
        _positionInitiale = transform.localPosition;
        DesactiverArme();
    }
    public void InitialiserArme(TypePouvoir typePouvoir, bool _estLeger)
    {
        // Trouver l'arme correspondant au type de pouvoir
        SOArme _armeEquipee = System.Array.Find(_tDonneesArmes, arme => arme.typePouvoir == typePouvoir);
        Debug.Log("Arme trouvée : " + _armeEquipee.nom);
        Debug.Log("Degats : " + _armeEquipee.degats);
        // Changer le sprite de l'arme

        // Temporairement, en attendant les sprites de l'arme
        switch (typePouvoir)
        {
            case TypePouvoir.Poison:
                _spriteRenderer.color = Color.green;
                break;
            case TypePouvoir.Ombre:
                _spriteRenderer.color = Color.blue;
                break;
            case TypePouvoir.Foudre:
                _spriteRenderer.color = Color.yellow;
                break;
            default:
                _spriteRenderer.color = Color.white;
                break;
        }

        StartCoroutine(CoroutineDesactiverArme(_estLeger));

        // _spriteRenderer.sprite = _armeEquipee.sprite;

    }

    IEnumerator CoroutineDesactiverArme(bool _estLeger)
    {
        if (_estLeger)
        {
            yield return new WaitForSeconds(0.5f);
            DesactiverArme();
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            DesactiverArme();
        }
        
    }


    public void ChangerDirection(bool _estGauche)
    {
        if (_estGauche)
        {
            transform.localPosition = new Vector3(-_positionInitiale.x, _positionInitiale.y, _positionInitiale.z);
        }
        else
        {
            transform.localPosition = _positionInitiale;
        }
    }

    /// <summary>
    /// Sera appelé pour desactiver l'arme par animation event plus tard, pour l'instant, on l'appelleras par Coroutine
    /// </summary>
    public void DesactiverArme()
    {
        _perso.PermettreAttaque();
        gameObject.SetActive(false);
        // Désactiver l'arme
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Si le joueur entre en collision avec un ennemi
        if (other.GetComponent<Ennemi>() != null)
        {
            // Debug.Log(_armeEquipee.nom);
            Debug.Log("Collision avec ennemi");
            Ennemi ennemi = other.GetComponent<Ennemi>();
            int degatInfligee = _armeEquipee.degats * _donneesPerso.attaque;
            ennemi.SubirDegats(degatInfligee, _armeEquipee.typePouvoir);
        }

    }
}
