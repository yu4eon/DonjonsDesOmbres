using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Table : MonoBehaviour
{
    [SerializeField] Tilemap _tilemap;
    [SerializeField] Planche[] _tSallesModeles;
    [SerializeField] Vector2Int _taille = new Vector2Int(3, 3);
    [SerializeField] TileBase _tuileModele;
    public Tilemap tilemap => _tilemap;

    void Awake()
    {
        Vector2Int tailleAvecUneBordure = Planche.tailleAvecBordures - Vector2Int.one;

        for (int x = 0; x < _taille.x; x++)
        {
            for (int y = 0; y < _taille.y; y++)
            {
                Vector2 pos = new Vector2(tailleAvecUneBordure.x * x, tailleAvecUneBordure.y * y);
                Planche planche = Instantiate(_tSallesModeles[Random.Range(0,_tSallesModeles.Length)], pos, Quaternion.identity, transform);
                planche.name = "Planche_" + x + "_" + y;
            }
        }

        Vector2Int tailleTable = _taille * tailleAvecUneBordure;
        Vector2Int min = Vector2Int.zero - Planche.tailleAvecBordures / 2;
        Vector2Int max = min + tailleTable;

        for (int y = min.y; y <= max.y; y++)
        {
            for (int x = min.x; x <= max.x; x++)
            {
                if (x == min.x || x == max.x || y == min.y || y == max.y)
                {
                    Vector3Int pos = new Vector3Int(x, y);
                    _tilemap.SetTile(pos, _tuileModele);
                }
            }
        }
    }
}