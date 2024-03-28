using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SOPerso", menuName = "SOPerso")]

/// <summary>
/// #tp3
/// Auteur du code : Antoine Lachance, Léon Yu
/// Commentaires ajoutés par : Antoine Lachance, Léon Yu
/// </summary>
public class SOPerso : ScriptableObject
{
    [Header("Valeurs actuelles")]
    [SerializeField, Range(1, 5)] int _niveau = 1;
    [SerializeField, Range(0, 500)] int _argent = 100;

    [Header("Valeurs initiales")]
    [SerializeField, Range(1, 5)] int _niveauIni = 1;
    [SerializeField, Range(0, 500)] int _argentIni = 100;

    [Header("Stats")]
    [SerializeField] int baseAttaque = 10;
    int attaqueBonus;
    [SerializeField] int baseDefense = 10;
    int defenseBonus;
    [SerializeField] int basePv = 10;
    int pvBonus;
    List<TypePouvoir> _pouvoirs = new List<TypePouvoir>();
    public List<TypePouvoir> pouvoirs => _pouvoirs;

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


    public void Initialiser()
    {
        _niveau = _niveauIni;
        _argent = _argentIni;
        ViderInventaire();
        // Reset tout les variable des SOObjet estAcheter à false
    }

    public void Acheter(SOObjet donneesObjet)
    {
        argent -= donneesObjet.prix;
        _lesObjets.Add(donneesObjet);
        AfficherInventaire();
        // Leon J'ai changé les ifs pour que ça check le type d'objet au lieu du nom de l'objet
        if (donneesObjet.typeObjet == TypeObjet.Attaque) attaqueBonus += 10; Debug.Log("Bonus attaque: " + attaqueBonus);
        if (donneesObjet.typeObjet == TypeObjet.DefensePV) //Si l'objet est de type DefensePV, ajoute 10 à la défense et aux points de vie
        {
            defenseBonus += 10;
            pvBonus += 10;
            Debug.Log("Bonus defense: " + defenseBonus);
            Debug.Log("Bonus pv: " + pvBonus);
        } 
        if (donneesObjet.typeObjet == TypeObjet.DoubleSaut) Perso.possedeDoublesSauts = true;
        if (donneesObjet.estAcheter == false) donneesObjet.estAcheter = true;
    }

    public void AjouterArgent(int value)
    {
        argent += value;
        Debug.Log("Argent ajouté: " + argent);
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

    /// <summary>
    /// Leon : 
    /// Méthode qui vide l'inventaire du joueur et remet les stats à leur valeur de base
    /// </summary>
    public void ViderInventaire()
    {
        foreach (SOObjet objet in _lesObjets)
        {
            objet.estAcheter = false;
        }
        _pouvoirs.Clear();
        _lesObjets.Clear();
        attaqueBonus = 0;
        defenseBonus = 0;
        pvBonus = 0;
        Perso.possedeDoublesSauts = false;
    }
    public void AjouterPouvoir(TypePouvoir nomPouvoir)
    {
        string pouvoirPerso = "";
        if (!_pouvoirs.Contains(nomPouvoir))
        {
            _pouvoirs.Add(nomPouvoir);
            foreach (TypePouvoir pouvoir in _pouvoirs)
            {
                if (pouvoirPerso != "")
                {
                    pouvoirPerso += ", ";
                }
                pouvoirPerso += pouvoir;
            }
            Debug.Log("Pouvoirs actuelles :" + pouvoirPerso);
        }
    }


    void OnValidate()
    {
        _evenementMiseAJour.Invoke();
    }
}
