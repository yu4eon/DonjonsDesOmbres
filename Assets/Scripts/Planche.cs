using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Planche : MonoBehaviour
{
    static Vector2Int _tailleAvecBordures = new Vector2Int(22, 10);
    static public Vector2Int tailleAvecBordures => _tailleAvecBordures;

    public void Tester()
    {
        Debug.Log($"Planche {name} est à la position {transform.position} et a une taille de {tailleAvecBordures} cases.");
    }

    
}
