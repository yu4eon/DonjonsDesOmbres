using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SOPerso", menuName = "SOPerso")]
public class SOPerso : ScriptableObject
{
    [Header("Valeurs actuelles")]
    [SerializeField, Range(1, 5)] int _niveau = 1;
    [SerializeField, Range(0, 500)] int _argent = 100;

    [Header("Valeurs initiales")]
    [SerializeField, Range(1, 5)] int _niveauIni = 1;
    [SerializeField, Range(0, 500)] int _argentIni = 100;

    public int niveau
    {
        get => _niveau;
        set
        {
            _niveau = Mathf.Clamp(value, 1, int.MaxValue);
            _evenementMiseAJour.Invoke();
        }
    }

    public int argent
    {
        get => _argent;
        set
        {
            _argent = Mathf.Clamp(value, 0, int.MaxValue);
            _evenementMiseAJour.Invoke();
        }

    }

    UnityEvent _evenementMiseAJour = new UnityEvent();
    public UnityEvent evenementMiseAJour => _evenementMiseAJour;

    List<SOObjet> _lesObjets = new List<SOObjet>();

    float _facteurPrixIni = 1;
    float _facteurPrix = 1;
    public float facteurPrix
    {
        get => _facteurPrix;
        set
        {
            _facteurPrix = Mathf.Clamp(value, 0, int.MaxValue);
            _evenementMiseAJour.Invoke();
        }
    }
    float _facteurPrixSiRabais = 0.9f;

    public void Initialiser()
    {
        _niveau = _niveauIni;
        _argent = _argentIni;
        _lesObjets.Clear();
        _facteurPrix = _facteurPrixIni;
    }

    public void Acheter(SOObjet donneesObjet)
    {
        argent -= donneesObjet.prix;
        if (donneesObjet.donneDroitRabais) facteurPrix = _facteurPrixSiRabais;
        _lesObjets.Add(donneesObjet);
        AfficherInventaire();

    }

    public void AjouterArgent(int value)
    {
        argent += value;
    }

    void AfficherInventaire()
    {
        string inventaire = "";
        foreach (SOObjet objet in _lesObjets)
        {
            if (inventaire != "")
            {
                inventaire += ", ";
            }
            inventaire += objet.nom;
        }
        Debug.Log("Inventaire du perso : " + inventaire);
    }


    void OnValidate()
    {
        _evenementMiseAJour.Invoke();
    }
}
