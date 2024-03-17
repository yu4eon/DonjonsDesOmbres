using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Perso", menuName = "Perso")]
public class SOPerso : ScriptableObject
{
    [Header("Valeurs initialles")]
    [SerializeField, Range(1, 5)] int _niveauIni = 1;
    [SerializeField, Range(0, 500)] int _pointsIni = 0;

    [Header("Valeurs actuelles")]
    [SerializeField, Range(1, 5)] int _niveau = 1;
    [SerializeField, Range(0, 500)] int _argent = 0;

    public int argent
    {
        get => _argent;
        set
        {
            _argent = Mathf.Clamp(value, 0, int.MaxValue);
            _evenementMiseAJour.Invoke();
        }
    }
    public int niveau
    {
        get => _niveau;
        set
        {
            _niveau = Mathf.Clamp(value, 1, int.MaxValue);
            _evenementMiseAJour.Invoke();
        }
    }

    UnityEvent _evenementMiseAJour = new UnityEvent();
    public UnityEvent evenementMiseAJour => _evenementMiseAJour;

    List<SOObjet> _lesObjets = new List<SOObjet>();

    public void Initialiser()
    {
        _niveau = _niveauIni;
        _argent = _pointsIni;
        _lesObjets.Clear();
    }

    public void Acheter(SOObjet donneesObjet)
    {
        argent -= donneesObjet.prixDeBase;
        _lesObjets.Add(donneesObjet);
        AfficherInventaire();
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
            inventaire += objet.nom + "\n";
        }
        Debug.Log(inventaire);
    }

    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    void OnValidate()
    {
        _evenementMiseAJour.Invoke();
    }

}


