using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableauHonneur : MonoBehaviour
{
    [SerializeField] SOSauvegarde _donneesSauvegarde; // Données de sauvegarde
    [SerializeField] SOPerso _donneesPerso; // Données du personnage
    List<JoueurScore> _lesJoueursScores = new List<JoueurScore>(); // Liste des joueurs et leurs scores
    // Start is called before the first frame update
    void Start()
    {
        _lesJoueursScores = _donneesSauvegarde.lesJoueursScores; // Obtient la liste des joueurs et leurs scores
        Debug.Log("Nombre de joueurs : " + _lesJoueursScores.Count); // Affiche le nombre de joueurs
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
