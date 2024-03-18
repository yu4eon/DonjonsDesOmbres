using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

/// <summary>
/// Classe qui gère la tilemap principal, contenant toutes les salles du niveau
public class Niveau : MonoBehaviour
{
    [SerializeField] Tilemap _tilemapNiveau; // tilemap du niveau #tp3 Léon - J'ai changé le nom de la variable pour être plus explicite.
    [SerializeField] Salle[] _tSallesModeles; // Tableau de tous les prefabs de salles disponibles.
    [SerializeField] Vector2Int _taille = new Vector2Int(3, 3); // Taille du niveau en 2 dimensions, sur l'axe x et y.
    [SerializeField] TileBase _tuileModele; // Tuile utilisé pour les bordures
    [SerializeField] Joyau[] _tJoyauxModeles; // Tableau de tous les prefabs de joyaux disponibles. #tp3 Léon
    [SerializeField, Range(0, 20)] int _nbJoyauxParSalle = 5; // Nombre de joyaux par salle. #tp3 Léon
    
    List<Vector2Int> _lesPosLibres = new List<Vector2Int>(); // Liste des positions libres dans le niveau. #tp3 Léon 
    List<Vector2Int> _lesPosSurReperes = new List<Vector2Int>(); // Liste des positions sur les repères. #tp3 Léon

    // Propriété publique qui permet l'accès à la tilemap du niveau.
    public Tilemap tilemap => _tilemapNiveau;

    static Niveau _instance; // Instance statique de la classe. #tp3 Léon
    static public Niveau instance => _instance; // Propriété publique qui permet l'accès à l'instance de la classe. #tp3 Léon

     void Awake()
    {
        //Singleton #tp3 Léon
        if (_instance == null) //si l'instance est null
        {
            _instance = this; //l'instance est égale à cette instance
        }
        else
        {
            Destroy(gameObject); //sinon, détruit l'objet
        }

        // #tp3 Léon
        CreerNiveau();
        // #tp3 Léon
        TrouverPosLibres();
        PlacerLesJoyaux();
    }

    void PlacerLesJoyaux()
    {
        Transform contenant = new GameObject("Joyaux").transform; // Crée un GameObject pour contenir les joyaux.
        contenant.parent = transform; // Assigne le niveau comme parent du contenant.
        int nbJoyaux = _nbJoyauxParSalle * _taille.x * _taille.y; // Calcul du nombre total de joyaux à placer.
        for (int i = 0; i < nbJoyaux; i++) // Boucle pour placer les joyaux.
        {
            int indexJoyau = Random.Range(0, _tJoyauxModeles.Length); // Sélectionne un joyau aléatoire.
            Joyau joyauModele = _tJoyauxModeles[indexJoyau]; // Obtient le prefab du joyau.

            Vector2Int pos = ObtenirPosLibre(); // Obtient une position libre aléatoire.
            Vector3 pos3 = (Vector3)(Vector2)pos + _tilemapNiveau.transform.position + _tilemapNiveau.tileAnchor; // Convertit la position
            Instantiate(joyauModele, pos3, Quaternion.identity, contenant); // Crée le joyau à la position obtenue.
            if(_lesPosLibres.Count == 0) // Si il n'y a plus de position libre.
            {
                Debug.LogWarning("Plus de place pour les joyaux"); // Affiche un message d'avertissement.
                break; // Sort de la boucle.
            }
        }
    }

    Vector2Int ObtenirPosLibre()
    {
        int indexPosLibre = Random.Range(0, _lesPosLibres.Count);
        Vector2Int pos = _lesPosLibres[indexPosLibre];
        _lesPosLibres.RemoveAt(indexPosLibre);
        return pos;
    }

    void CreerNiveau()
    {

        // Calcul de la taille de la salle avec une bordure.
        Vector2Int tailleAvecUneBordure = Salle.taille - Vector2Int.one;

        // Boucle pour placer les salles dans le niveau selon la taille spécifiée dans l'axe x.
        for (int x = 0; x < _taille.x; x++)
        {
            // Boucle pour placer les salles dans le niveau selon la taille spécifiée dans l'axe y.
            for (int y = 0; y < _taille.y; y++)
            {
                Debug.Log("position actuel" +x + "," + y);
                // Position de la salle.
                Vector2 pos = new Vector2(tailleAvecUneBordure.x * x, tailleAvecUneBordure.y * y);

                Salle planche = Instantiate(_tSallesModeles[Random.Range(0, _tSallesModeles.Length)], pos, Quaternion.identity, transform);

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
                    _tilemapNiveau.SetTile(pos, _tuileModele);
                    // Debug.Log(x + " " + y);
                }
            }
        }
    }

    /// <summary>
    /// Trouve les positions dans la scène ou il n'y a aucune tuile. #tp3 Léon
    /// </summary>
    void TrouverPosLibres()
    {
        BoundsInt bornes = _tilemapNiveau.cellBounds;
        for(int x = bornes.xMin; x < bornes.xMax; x++) 
        {
            for(int y = bornes.yMin; y < bornes.yMax; y++)
            {
                Vector2Int posTuile = new Vector2Int(x, y);
                TileBase tuile = _tilemapNiveau.GetTile((Vector3Int)posTuile);
                if(tuile == null)
                {
                    _lesPosLibres.Add(posTuile);
                }
            }
        }
        foreach(Vector2Int pos in _lesPosSurReperes)
        {
            _lesPosLibres.Remove(pos);
        }
        Debug.Log(_lesPosLibres.Count + " espaces libres : "+ string.Join(", ", _lesPosLibres));
    }

    /// <summary>
    /// Méthode publique appelé par CarteTuiles qui ajoute une tuile à la tilemap du niveau.
    /// #tp3 Léon, j'ai enlevé le paramètre niveau, car il n'est plus nécessaire.
    /// </summary>
    /// <param name="tilemap">Tilemap d'ou vient la tuile</param>
    /// <param name="y">Position y de la tuile à ajouter</param>
    /// <param name="x">Position x de la tuile à ajouter</param>
    /// <param name="decalage">Décalage à appliquer à la tuile</param>
    public void AjouterTuile(Tilemap tilemap, int y, int x, Vector3Int decalage)
    {
        Vector3Int pos = new Vector3Int(x, y, 0);
        TileBase tile = tilemap.GetTile(pos);

        // Vérifie si la tuile existe.
        if (tile != null)
        {
            _tilemapNiveau.SetTile(pos + decalage, tile);
        }
    }
}
