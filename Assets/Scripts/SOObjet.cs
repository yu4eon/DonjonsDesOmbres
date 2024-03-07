using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Objet", menuName = "Objet boutique")]
public class SOObjet : ScriptableObject
{
    [Header("LES DONNÉES")]
    [SerializeField] string _nom = "Trèfle";
    [SerializeField][Tooltip("Image de l'objet placé dans la boutique")] Sprite _sprite;
    [SerializeField][Range(0, 200)] int _prixDeBase = 30;
    [SerializeField][Range(1, 5)] int _niveauRequis = 1;
    [SerializeField][TextArea] string _description;
    [SerializeField][Tooltip("Cet objet donne-t-il droit au rabais?")] bool _donneDroitRabais = false;

    public string nom { get => _nom; set => _nom = value; }
    public Sprite sprite { get => _sprite; set => _sprite = value; }
    public int prixDeBase { get => _prixDeBase; set => _prixDeBase = Mathf.Clamp(value, 0, int.MaxValue); }
    public int niveauRequis { get => _niveauRequis; set => _niveauRequis = Mathf.Clamp(value, 0, int.MaxValue); }
    public string description { get => _description; set => _description = value; }
    public bool donneDroitRabais { get => _donneDroitRabais; set => _donneDroitRabais = value; }
}