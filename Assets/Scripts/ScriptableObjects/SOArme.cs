using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Arme", menuName = "Arme Perso")]
public class SOArme : ScriptableObject
{
    [Header("LES DONNÉES")] // En-tête pour regrouper les données
    [SerializeField] string _nom; // Nom de l'arme, seulement pour l'éditeur pour pouvoir tester
    [SerializeField] float _degatsBase; // Dégâts de l'arme
    [SerializeField] TypePouvoir _typePouvoir; // Type de pouvoir de l'arme
    [SerializeField] AudioClip _sonAttaque; // Son d'attaque de l'arme
    
    public AudioClip sonAttaque { get => _sonAttaque; set => _sonAttaque = value; }
    public float degats { get => _degatsBase; set => _degatsBase = value; }
    public TypePouvoir typePouvoir { get => _typePouvoir; set => _typePouvoir = value; }
    public string nom { get => _nom; set => _nom = value; }
        
}
