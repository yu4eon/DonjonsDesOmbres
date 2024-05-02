using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;


/// <summary>
/// Auteur du code : Léon Yu, Antoine Lachance
/// Commetaires ajoutés par : Léon Yu, Antoine Lachance 
/// Classe pour le conteneur des tuiles, détermine la taille de la salle et affiche des informations sur la salle.
public class Salle : MonoBehaviour
{
    //La taille d'une salle
    static Vector2Int _taille = new Vector2Int(32, 18);
    // Propriété pour accéder à la taille de la salle :
    static public Vector2Int taille => _taille;

    [SerializeField] public Transform _repereObjet; //Repère pour placer les objets
    [SerializeField] Transform[] _tReperesEnnemis; //Tableau des repères pour les ennemis
    public Transform[] tReperesEnnemis => _tReperesEnnemis; //Propriété pour accéder aux repères des ennemis
    [SerializeField] Transform[] _tEffectors; //Tableau des effectors dans la salle
    public Transform[] tEffectors => _tEffectors; //Propriété pour accéder aux effectors
    

    // #tp3 leon, Supprimer la methode Tester car elle etait inutile

    /// <summary>
    /// Méthode de Unity pour les Gizmos
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, (Vector2)_taille);
    }
    /// <summary>
    /// Méthode pour placer un modèle sur le repère. #tp3 Léon
    /// </summary>
    /// <param name="_modele">L'objet a placer</param>
    /// <returns>La position ou l'objet doit etre placé</returns>
    public Vector2Int PlacerSurRepere(GameObject _modele)
    {

        Vector3 pos = _repereObjet.position;
        Instantiate(_modele, pos, Quaternion.identity, transform.parent);
        return Vector2Int.FloorToInt(pos);
    }

    public void PlacerEnnemiSurRepere(GameObject _modele, int index, Transform contenant)
    {
        Vector3 pos = _tReperesEnnemis[index].position;
        Instantiate(_modele, pos, Quaternion.identity, contenant);
    }
}
