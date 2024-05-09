using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemi : MonoBehaviour
{
    [SerializeField] TypePouvoir _typePouvoirEnnemi; // Type de pouvoir de l'ennemi
    [SerializeField] int _pointsDeVieIni = 100; // Points de vie initial de l'ennemi
    int _pointsDeVie; // Points de vie actuels de l'ennemi

    // Dictionnaire des faiblesses de chaque pouvoir (a changer selon les demandes de l'artiste)
    Dictionary<TypePouvoir, TypePouvoir> _faiblesses = new Dictionary<TypePouvoir, TypePouvoir>
    {
        { TypePouvoir.Poison, TypePouvoir.Glace },
        { TypePouvoir.Ombre, TypePouvoir.Foudre },
        { TypePouvoir.Foudre, TypePouvoir.Poison },
        { TypePouvoir.Glace, TypePouvoir.Ombre }
    };

    Animator _animator; // Animator de l'ennemi
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SubirDegats(int degats, TypePouvoir typePouvoir)
    {
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

        _pointsDeVie -= degats; // Réduit les points de vie de l'ennemi
        // Réduit les points de vie de l'ennemi
        if(_pointsDeVie <= 0)
        {
            Mourir();
        }
    }

    void Mourir()
    {
        Debug.Log("L'ennemi est mort");
        // _animator.SetTrigger("Meurt");
        gameObject.SetActive(false);
    }
}
