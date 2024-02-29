using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Classe qui gère la tilemap principal, contenant toutes les salles du niveau
public class Niveau : MonoBehaviour
{
    [SerializeField] Tilemap _tilemap; // tilemap du niveau
    [SerializeField] Salle[] _tSallesModeles; // Tableau de tous les prefabs de salles disponibles.
    [SerializeField] Vector2Int _taille = new Vector2Int(3, 3); // Taille du niveau en 2 dimensions, sur l'axe x et y.
    [SerializeField] TileBase _tuileModele; // Tuile utilisé pour les bordures

    // Propriété publique qui permet l'accès à la tilemap du niveau.
    public Tilemap tilemap => _tilemap;

    void Awake()
    {
        // Calcul de la taille de la salle avec une bordure.
        Vector2Int tailleAvecUneBordure = Salle.taille - Vector2Int.one;

        // Boucle pour placer les salles dans le niveau selon la taille spécifiée dans l'axe x.
        for (int x = 0; x < _taille.x; x++)
        {
            // Boucle pour placer les salles dans le niveau selon la taille spécifiée dans l'axe y.
            for (int y = 0; y < _taille.y; y++)
            {
                // Position de la salle.
                Vector2 pos = new Vector2(tailleAvecUneBordure.x * x, tailleAvecUneBordure.y * y);
                
                Salle planche = Instantiate(_tSallesModeles[Random.Range(0,_tSallesModeles.Length)], pos, Quaternion.identity, transform);
                
                // Nomme la salle selon sa position dans le niveau.
                planche.name = "Salle" + x + "_" + y;
            }
        }

        // Calcul pour la taille du niveau avec une bordure, ainsi que les coordonnées minimales et maximales.
        Vector2Int tailleNiveau = _taille * tailleAvecUneBordure;
        Vector2Int min = Vector2Int.zero - Salle.taille / 2;
        Vector2Int max = min + tailleNiveau;

        // Boucle pour la création des bordures du niveau sur l'axe y.
        for (int y = min.y; y <= max.y; y++)
        {
        // Boucle pour la création des bordures du niveau sur l'axe x.
            for (int x = min.x; x <= max.x; x++)
            {
                // Si la position s'agit d'Une position de bordure.
                if (x == min.x || x == max.x || y == min.y || y == max.y)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    _tilemap.SetTile(pos, _tuileModele);
                    Debug.Log(x + " " + y);
                }
            }
        }
    }

    /// <summary>
    /// Méthode publique appelé par CarteTuiles qui ajoute une tuile à la tilemap du niveau.
    /// </summary>
    /// <param name="tilemap">Tilemap d'ou vient la tuile</param>
    /// <param name="niveau">Niveau dans laquel ajouter la tuile</param>
    /// <param name="y">Position y de la tuile à ajouter</param>
    /// <param name="x">Position x de la tuile à ajouter</param>
    /// <param name="decalage">Décalage à appliquer à la tuile</param>
    public void AjouterTuile(Tilemap tilemap, Niveau niveau, int y, int x, Vector3Int decalage)
    {
        Vector3Int pos = new Vector3Int(x, y, 0);
        TileBase tile = tilemap.GetTile(pos);

        // Vérifie si la tuile existe.
        if (tile != null)
        {
            niveau.tilemap.SetTile(pos + decalage, tile);
        }
    }
}
