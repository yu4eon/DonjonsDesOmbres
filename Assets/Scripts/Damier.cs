using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Damier : MonoBehaviour
{
    [SerializeField] TypeCase _typeCase;
    void Awake()
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;
        bool doitRester = (_typeCase == TypeCase.Impaires);
        Table table = GetComponentInParent<Table>();
        Vector3Int decalage = Vector3Int.FloorToInt(transform.position);

        for (int y = bounds.yMin; y < bounds.yMax; y++)
        {
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                if(_typeCase == TypeCase.Toutes) doitRester = true;
                ModifieTuile(tilemap, doitRester, table, y, x, decalage);
                doitRester = !doitRester;
            }
            doitRester = !doitRester;

        }
        gameObject.SetActive(false);
    }

    private static void ModifieTuile(Tilemap tilemap, bool doitRester, Table table, int y, int x, Vector3Int decalage)
    {
        Vector3Int pos = new Vector3Int(x, y, 0);
        TileBase tile = tilemap.GetTile(pos);
        if (!doitRester)
        {
            tilemap.SetTile(pos + decalage, null);
        }
        else if (tile != null)
        {
            table.tilemap.SetTile(pos + decalage, tile);
        }
    }
}
