using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "Objet", menuName = "Objet boutique")]
public class SOObjet : ScriptableObject
{
    [Header("LES DONNÉES")]
    [SerializeField] string _nom = "Trèfle";
    [SerializeField, Tooltip("Îcone de l'objet pour la boutique")] Sprite _sprite;
    [SerializeField][Range(0, 200)] int _prixDeBase = 30;
    [SerializeField, TextArea]/*TextArea*/ string _description;
    [SerializeField][Tooltip("Cet objet a-t'il déjà été acheté?")] bool _estAcheter = false;
    [SerializeField] TypeObjet _typeObjet; // Type de l'objet #tp3 Leon 

    public TypeObjet typeObjet { get => _typeObjet; set => _typeObjet = value; }
    public string nom { get => _nom; set => _nom = value; }
    public Sprite sprite { get => _sprite; set => _sprite = value; }
    public int prix => _prixDeBase;

    public string description { get => _description; set => _description = value; }
    public bool estAcheter { get => _estAcheter; set => _estAcheter = value; }
}