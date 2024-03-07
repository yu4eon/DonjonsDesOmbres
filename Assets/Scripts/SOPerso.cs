using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Perso", menuName = "Perso")]
public class SOPerso : ScriptableObject
{
    [Header("Valeurs initialles")]
    [SerializeField, Range(1, 5)] int _niveauIni = 1;
    [SerializeField, Range(0, 500)] int _pointsIni = 0;

    [Header("Valeurs actuelles")]
    [SerializeField, Range(1, 5)] int _niveau = 1;
    [SerializeField, Range(0, 500)] int _points = 0;

    public int points
    {
        get => _points;
        set => _points = Mathf.Clamp(value, 0, int.MaxValue);
    }
    public int niveau
    {
        get => _niveau;
        set => _niveau = Mathf.Clamp(value, 1, int.MaxValue);
    }

    public void Initialiser()
    {
        _niveau = _niveauIni;
        _points = _pointsIni;
    }

}


