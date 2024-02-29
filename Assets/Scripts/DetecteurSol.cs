using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui gère la détection du sol pour les personnages
/// </summary>
public class DetecteurSol : MonoBehaviour
{

    [SerializeField] Vector2 _posCentreBoite = new Vector2(0, -1f); //Position du centre de la boite de collision
    [SerializeField] Vector2 _tailleBoite = new Vector2(0.5f, 0.1f); //Taille de la boite de collision
    [SerializeField] LayerMask _mask; //Les Layermasks qui sont considérés comme sol
    protected bool _estAuSol; //Si le personnage est au sol

    /// <summary>
    /// Méthode qui est appelée à chaque frame à un rhythme fixe
    /// </summary>
    virtual protected void FixedUpdate()
    {
        VerifierSol();
    }

    /// <summary>
    /// Méthode qui vérifie si le personnage est au sol par OverlapBox
    /// </summary>
    void VerifierSol()
    {
        Vector2 pointDepart = (Vector2)transform.position + _posCentreBoite;
        Collider2D collision = Physics2D.OverlapBox(pointDepart, _tailleBoite, 0, _mask);
        _estAuSol = collision != null;
        Debug.Log("Est au sol?" +_estAuSol);
    }

    /// <summary>
    /// Méthode qui dessine la boite de collision
    /// </summary>
    void OnDrawGizmos()
    {   
        if(!Application.isPlaying)
        {
            VerifierSol();
        }
        Gizmos.color = _estAuSol ? Color.green : Color.red;
        Vector2 pointDepart = (Vector2)transform.position + _posCentreBoite;
        Gizmos.DrawWireCube(pointDepart, _tailleBoite);
    }
}
