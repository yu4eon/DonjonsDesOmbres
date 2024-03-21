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
    [SerializeField][Range(1, 5)] int _niveauRequis = 1;
    [SerializeField, TextArea]/*TextArea*/ string _description;
    [SerializeField][Tooltip("Cet objet donne-t-il droit au rabais?")] bool _donneDroitRabais = false;
    [SerializeField][Tooltip("Cet objet donne-t-il un bonus d'attaque")] bool _bonusAttack = false;
    [SerializeField][Tooltip("Cet objet donne-t-il un bonus de point de vie")] bool _bonusPv = false;

    public string nom { get => _nom; set => _nom = value; }
    public Sprite sprite { get => _sprite; set => _sprite = value; }
    public int prix
    {
        get
        {
            float facteur = 1f;
            if(Boutique.instance != null) facteur = Boutique.instance.donneesPerso.facteurPrix;
            int prix = Mathf.RoundToInt(_prixDeBase * facteur);
            return prix;
        }
    }
    public int niveauRequis { get => _niveauRequis; set => _niveauRequis = Mathf.Clamp(value, 0, int.MaxValue); }
    public string description { get => _description; set => _description = value; }
    public bool donneDroitRabais { get => _donneDroitRabais; set => _donneDroitRabais = value; }
    public bool donneBonusAttack { get => _bonusAttack; set => _bonusAttack = value; }
}