using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Auteur du code : Léon Yu. Antoine Lachance
/// Commenteur du code : Léon Yu et Antoine Lachance
/// Classe qui gère la tilemap principal, contenant toutes les salles du niveau
public class Niveau : MonoBehaviour
{
    [SerializeField] Tilemap _tilemapNiveau; // tilemap du niveau #tp3 Léon - J'ai changé le nom de la variable pour être plus explicite.
    [SerializeField] Salle[] _tSallesModeles; // Tableau de tous les prefabs de salles disponibles.
    [SerializeField] Vector2Int _taille = new Vector2Int(3, 3); // Taille du niveau en 2 dimensions, sur l'axe x et y.
    [SerializeField] TileBase _tuileModele; // Tuile utilisé pour les bordures
    [SerializeField] Joyau[] _tJoyauxModeles; // Tableau de tous les prefabs de joyaux disponibles. #tp3 Léon
    [SerializeField] Autels[] _tAutelsModeles; // Tableau de tous les prefabs d'autels disponibles. #tp3 Antoine
    [SerializeField] Perso _perso;
    [SerializeField] GameObject _cle;
    [SerializeField] GameObject _activateur;
    [SerializeField] GameObject _porte;
    // [SerializeField] SOActivateur _activateur;
    [SerializeField] int _nbJoyauxParSalle = 5; // Nombre de joyaux par salle. #tp3 Léon , Range(0, 20)
    List<Vector2Int> _lesPosLibres = new List<Vector2Int>(); // Liste des positions libres dans le niveau. #tp3 Léon 
    List<Vector2Int> _lesPosSurReperes = new List<Vector2Int>(); // Liste des positions sur les repères. #tp3 Léon
    List<Vector2Int> _lesPosEffectors = new List<Vector2Int>(); // Liste des positions des effectors. #tp3 Léon

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

        
    }

    void Start()
    {
        
        CreerNiveau(); // #tp3 Léon
        TrouverPosLibres(); // #tp3 Léon
        PlacerItems(_perso, _porte, _cle, _activateur); //#tp3 Antoine
        PlacerAutels();//#tp3 Antoine
        PlacerLesJoyaux(); // #tp3 Léon
    }

    /// <summary>
    /// #tp3 Léon
    /// Méthode pour placer les joyaux dans les salles du niveau. 
    /// </summary>
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
            if (_lesPosLibres.Count == 0) // Si il n'y a plus de position libre.
            {
                Debug.LogWarning("Plus de place pour les joyaux"); // Affiche un message d'avertissement.
                break; // Sort de la boucle.
            }
        }
    }

    void PlacerAutels()
    {
        Transform contenant = new GameObject("Autels").transform; // Crée un GameObject pour contenir les autels.
        contenant.parent = transform; // Assigne le niveau comme parent du contenant.
        int nbAutels = 4; // Nombre d'autels à placer.
        for (int i = 0; i < nbAutels; i++) // Boucle pour placer les autels.
        {
            int indexAutel = i;
            Autels autelModele = _tAutelsModeles[indexAutel]; // Obtient le prefab de l'autel.

            Vector2Int pos = ObtenirPosLibre(); // Obtient une position libre aléatoire.

            // Vérifie si la position du dessus est vide et que la position du dessous n'est pas vide.
            if (PositionDessusEstVide(pos) && PositionDessousEstOccupee(pos))
            {
                Vector3 pos3 = (Vector3)(Vector2)pos + _tilemapNiveau.transform.position + _tilemapNiveau.tileAnchor; // Convertit la position
                pos3 += new Vector3(0, 1, 0); // Positionne l'autel au dessus de la position.
                Instantiate(autelModele, pos3, Quaternion.identity, contenant); // Crée le joyau à la position obtenue.
            }
            else
            {
                // Debug.LogWarning("Impossible de placer l'autel à la position : " + pos + ". La position du dessus doit être vide et celle du dessous doit être occupée.");
                i--; // Réduit le compteur pour réessayer de placer un autel.
            }

            if (_lesPosLibres.Count == 0) // Si il n'y a plus de position libre.
            {
                // Debug.LogWarning("Plus de place pour les autels"); // Affiche un message d'avertissement.
                break; // Sort de la boucle.
            }
        }
    }
    // Méthode pour vérifier si la position du dessus est vide.
    bool PositionDessusEstVide(Vector2Int pos)
    {
        Vector2Int posDessus = new Vector2Int(pos.x, pos.y + 1);
        if (_lesPosLibres.Contains(posDessus))
        {
            Vector2Int posDeuxDessus = new Vector2Int(pos.x, pos.y + 2);
            if (_lesPosLibres.Contains(posDeuxDessus))
            {
                return _lesPosLibres.Contains(posDessus);
            }
        }
        return false;
    }

    // Méthode pour vérifier si la position du dessous est occupée.
    bool PositionDessousEstOccupee(Vector2Int pos)
    {
        Vector2Int posDessous = new Vector2Int(pos.x, pos.y - 1);
        return !_lesPosLibres.Contains(posDessous);
    }

    void PlacerItems(Perso perso, GameObject porte, GameObject cle, GameObject activateur)
    {
        // Transform contenant = new GameObject("Items").transform; // Crée un GameObject pour contenir le perso, la porte et la clé.
        // contenant.parent = transform; // Assigne le niveau comme parent du contenant.
        Transform contenant = transform;

        // Récupérer le composant Niveau
        Niveau niveau = GetComponent<Niveau>();
        // Choisir un index aléatoire
        int index = Random.Range(0, niveau.transform.childCount);
        // Récupérer la salle aléatoire
        Salle salleAleatoire = niveau.transform.GetChild(index).GetComponent<Salle>();
        // Placer le personnage.
        Vector2Int posPerso = ObtenirPosLibre();
        Vector3 pos3Perso = (Vector3)(Vector2)posPerso + _tilemapNiveau.transform.position + _tilemapNiveau.tileAnchor;
        Instantiate(perso, pos3Perso, Quaternion.identity, contenant);


        // Placer la porte.
        List<string> extremitees = new List<string>
        {
            "Salle0_2",
            "Salle0_0",
            "Salle2_2",
            "Salle2_0",
        };
        int nb = Random.Range(0, extremitees.Count);
        Salle salle = GameObject.Find(extremitees[nb]).GetComponentInChildren<Salle>();
        Vector2Int decalage = Vector2Int.CeilToInt(_tilemapNiveau.transform.position);
        Vector2Int posRep = salle.PlacerSurRepere(porte) - decalage;


        // Placer la clé.
        extremitees.Reverse();
        Salle salle2 = GameObject.Find(extremitees[nb]).GetComponentInChildren<Salle>();
        Vector2Int posRep2 = salle2.PlacerSurRepere(cle) - decalage;
        extremitees.Reverse();
        salleAleatoire.PlacerSurRepere(activateur);
    }


    
    /// <summary>
    /// Méthode pour obtenir une position libre aléatoire dans le niveau. #tp3 Léon
    /// </summary>
    Vector2Int ObtenirPosLibre()
    {
        int indexPosLibre = Random.Range(0, _lesPosLibres.Count);
        Vector2Int pos = _lesPosLibres[indexPosLibre];
        _lesPosLibres.RemoveAt(indexPosLibre);
        return pos;
    }

    /// <summary>
    /// Méthode pour créer le niveau, incluant enlever les positions prises par des effectors dans la liste des positions libres. #tp3 Léon
    /// </summary>
    void CreerNiveau()
    {
        // Calcul de la taille de la salle avec une bordure.
        Vector2Int tailleAvecUneBordure = Salle.taille - Vector2Int.one;

        Vector2Int placementSpecial = new Vector2Int(Random.Range(0, _taille.x), Random.Range(0, _taille.y));

        // Boucle pour placer les salles dans le niveau selon la taille spécifiée dans l'axe x.
        for (int x = 0; x < _taille.x; x++)
        {
            // Boucle pour placer les salles dans le niveau selon la taille spécifiée dans l'axe y.
            for (int y = 0; y < _taille.y; y++)
            {
                // Debug.Log("position actuel" +x + "," + y);

                Vector2Int placementSalle = new Vector2Int(x, y); // #tp3, Léon Yu, Position selon la grille de la salle

                // Position de la salle.
                Vector2 pos = new Vector2(tailleAvecUneBordure.x * x, tailleAvecUneBordure.y * y);

                Salle salle = Instantiate(_tSallesModeles[Random.Range(0, _tSallesModeles.Length)], pos, Quaternion.identity, transform);

                // Nomme la salle selon sa position dans le niveau.
                salle.name = "Salle" + x + "_" + y;

                // Pour chaque effector de vitesse dans la salle. #tp3 Léon
                foreach (Transform posEffector in salle.tEffectors)
                {
                    // Ajoute les positions des effectors dans la liste des positions sur les repères.
                    //Ici, le i est -1, 0 et 1 pour les 3 tuiles que les effectors de vitesse prennent.
                    for (int i = -1; i < 2; i++)
                    {
                        Vector2Int decalage = Vector2Int.CeilToInt(_tilemapNiveau.transform.position); // Décalage pour la position.
                        //Les positions sur le repère sur son axe de x
                        Vector2Int posRep = new Vector2Int(Mathf.FloorToInt(posEffector.position.x - i), Mathf.FloorToInt(posEffector.position.y)) - decalage;
                        _lesPosEffectors.Add(posRep); // Ajoute la position sur la liste des positions pour effectors
                    }
                }
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
                // Si la position s'agit d'Une position de bordure :
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
    /// #tp3 Léon
    /// Trouve les positions dans la scène ou il n'y a aucune tuile. 
    /// </summary>
    void TrouverPosLibres()
    {
        BoundsInt bornes = _tilemapNiveau.cellBounds;
        // pour les tuiles sur l'axe des x
        for (int x = bornes.xMin; x < bornes.xMax; x++)
        {
            // pour les tuiles sur l'axe des y
            for (int y = bornes.yMin; y < bornes.yMax; y++)
            {
                Vector2Int posTuile = new Vector2Int(x, y);
                TileBase tuile = _tilemapNiveau.GetTile((Vector3Int)posTuile);
                // Si la tuile est vide, ajoute la position à la liste des positions libres :
                if (tuile == null) 
                {
                    _lesPosLibres.Add(posTuile);
                }
            }

        // Pour les positions prises par des repères, on les enlève de la liste des positions libres. 
        } 
        foreach (Vector2Int pos in _lesPosSurReperes)
        {
            _lesPosLibres.Remove(pos);
        }
        //Pour les positions prises par des effectors, on les enlève de la liste des positions libres.
        foreach (Vector2Int pos in _lesPosEffectors)
        {
            _lesPosLibres.Remove(pos);
        }
        // un simple debug pour voir combien d'espaces libres il y a dans le niveau
        Debug.Log(_lesPosLibres.Count + " espaces libres : " + string.Join(", ", _lesPosLibres));
    }

    /// <summary>
    /// #tp3 Léon
    /// Méthode publique appelé par Joyau qui libère une position dans la liste des positions libres.
    /// </summary>
    public void LibererUnePos(Vector3 posPrecise)
    {
        Vector2Int pos = Vector2Int.FloorToInt(posPrecise - _tilemapNiveau.transform.position);
        _lesPosLibres.Add(pos);
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
