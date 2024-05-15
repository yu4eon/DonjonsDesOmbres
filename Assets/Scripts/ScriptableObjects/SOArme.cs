using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Arme", menuName = "Arme Perso")]
public class SOArme : ScriptableObject
{
    [Header("LES DONNÉES")] // En-tête pour regrouper les données
    [SerializeField] string _nom; // Nom de l'arme, seulement pour l'éditeur pour pouvoir tester
    [SerializeField, Tooltip("Image de l'arme")] Sprite _sprite; // Sprite de l'arme
    [SerializeField] float _degatsBase; // Dégâts de l'arme
    [SerializeField] TypePouvoir _typePouvoir; // Type de pouvoir de l'arme

    public Sprite sprite { get => _sprite; set => _sprite = value; }
    public float degats { get => _degatsBase; set => _degatsBase = value; }
    public TypePouvoir typePouvoir { get => _typePouvoir; set => _typePouvoir = value; }
    public string nom { get => _nom; set => _nom = value; }
        
}
