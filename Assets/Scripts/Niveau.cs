using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Tilemaps;
using Cinemachine;
using System.Linq;

/// <summary>
/// Auteur du code : Léon Yu. Antoine Lachance
/// Commenteur du code : Léon Yu et Antoine Lachance
/// Classe qui gère la tilemap principal, contenant toutes les salles du niveau
public class Niveau : MonoBehaviour
{
    [Header("Niveau")]
    [SerializeField] Tilemap _tilemapNiveau; // tilemap du niveau #tp3 Léon - J'ai changé le nom de la variable pour être plus explicite.
    [SerializeField] Salle[] _tSallesModeles; // Tableau de tous les prefabs de salles disponibles.
    [SerializeField] Vector2Int _taille = new Vector2Int(2, 2); // Taille du niveau en 2 dimensions, sur l'axe x et y.
    [SerializeField] TileBase _tuileModele; // Tuile utilisée pour les bordures
    [SerializeField] UIJeu _uiJeu; // Référence à l'interface du jeu. #synthese Léon 

    [Header("Items")]
    [SerializeField] Joyau[] _tJoyauxModeles; // Tableau de tous les prefabs de joyaux disponibles. #tp3 Léon
    [SerializeField] Autels[] _tAutelsModeles; // Tableau de tous les prefabs d'autels disponibles. #tp3 Antoine
    [SerializeField] Perso _perso; // Tp3 Antoine
    [SerializeField] GameObject _cle; // Tp3 Antoine
    [SerializeField] GameObject _activateur; // Tp3 Antoine
    [SerializeField] GameObject _porte; // Tp3 Antoine
    [SerializeField] GameObject[] _tEnnemis; // Tableau des ennemis #synthese Leon

    [Header("Données")]
    [SerializeField] SOPerso _donneesPerso; // #Tp4 leon
    [SerializeField] SONavigation _donneesNavigation; // #synthese leon

    [Header("Camera")]
    [SerializeField] GameObject cm_collider;
    [SerializeField] CinemachineVirtualCamera cvCamera;

    [Header("Paramètres de jeu")]
    [SerializeField, Range(0, 20)] int _nbJoyauxParSalle = 5; // Nombre de joyaux par salle. #tp3 Léon , Range(0, 20)
    [SerializeField, Range(40, 200)] int _limiteTemps = 120; // Limite de temps pour le niveau. #synthese Léon
    int _temps; // Temps écoulé dans le niveau. #synthese Léon
    public int temps => _temps; // Propriété publique pour accéder au temps écoulé. #synthese Léon

    List<Vector2Int> _lesPosLibres = new List<Vector2Int>(); // Liste des positions libres dans le niveau. #tp3 Léon 
    List<Vector2Int> _lesPosSurReperes = new List<Vector2Int>(); // Liste des positions sur les repères. #tp3 Léon
    List<Vector2Int> _lesPosEffectors = new List<Vector2Int>(); // Liste des positions des effectors. #tp3 Léon

    // Propriété publique qui permet l'accès à la tilemap du niveau.
    public Tilemap tilemap => _tilemapNiveau;
    List<Salle> _lesSalles = new List<Salle>(); // Liste des salles instancié du niveau. #tp3 Léon
    List<Salle> _lesSallesSurBordure = new List<Salle>(); // Liste des salles sur la bordure du niveau. #tp3 Léon
    static Niveau _instance; // Instance statique de la classe. #tp3 Léon
    static public Niveau instance => _instance; // Propriété publique qui permet l'accès à l'instance de la classe. #tp3 Léon

    List<Vector2Int> niveauSurBordure = new List<Vector2Int>();
    // Tableau des pouvoirs disponibles. #synthese Leon
    TypePouvoir[] _pouvoirs = { TypePouvoir.Poison, TypePouvoir.Ombre, TypePouvoir.Foudre, TypePouvoir.Glace };

    Perso _clonePerso;
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
        SoundManager.instance.ChangerEtatLecturePiste(TypePiste.MusiqueBase, true);
        SoundManager.instance.ChangerEtatLecturePiste(TypePiste.MusiqueEvenA, false);
        SoundManager.instance.ChangerEtatLecturePiste(TypePiste.MusiqueEvenB, false);
        DefinirTailleNiveau(); // #tp4 Léon
        CreerNiveau(); // #tp3 Léon
        TrouverPosLibres(); // #tp3 Léon
        PlacerItems(_perso, _porte, _cle, _activateur); //#tp3 Antoine
        PlacerAutels();//#tp3 Antoine
        PlacerLesJoyaux(); // #tp3 Léon
        PlacerEnnemis(); // #synthese Léon
        _temps = _limiteTemps; // #synthese Léon
        _uiJeu.MettreAJourTemps(_temps); // #synthese Léon
        Coroutine coroutine = StartCoroutine(CoroutineDecoulerTemps()); // #synthese Léon
        // GameObject persoClone = (GameObject)GameObject.Instantiate(_perso.gameObject, _lesPosLibres[Random.Range(0, _lesPosLibres.Count)], Quaternion.identity);
        cvCamera.m_Follow = _clonePerso.transform;
        cm_collider.transform.localScale = new Vector2(_taille.x * 32 - 1, _taille.y * 18 - 1);
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
                // Debug.LogWarning("Plus de place pour les joyaux"); // Affiche un message d'avertissement.
                break; // Sort de la boucle.
            }
        }
    }

    /// <summary>
    /// #tp3 Antoine    
    /// méthode pour placer les autels dans les salles du niveau.
    /// </summary>
    void PlacerAutels()
    {
        Transform contenant = new GameObject("Autels").transform; // Crée un GameObject pour contenir les autels.
        contenant.parent = transform; // Assigne le niveau comme parent du contenant.
        int nbAutels = 4; // Nombre d'autels à placer.

        // Sélectionne un pouvoir aléatoire pour donner au personnage. #synthese Leon
        TypePouvoir pouvoirAleatoire = _pouvoirs[Random.Range(0, _pouvoirs.Length)];
        Debug.Log("Pouvoir du personnage : " + pouvoirAleatoire); // Affiche le pouvoir du personnage.
        _clonePerso.Initialiser(pouvoirAleatoire, _uiJeu); // Donne le pouvoir au joueur #synthese Leon
        _clonePerso.InstantierParticules((int)pouvoirAleatoire); // Instancie les particules du pouvoir du personnage. #synthese Leon
        Debug.Log(_uiJeu.name);
        _donneesPerso.evenementMiseAJour.Invoke();
        // _uiJeu.MettreAJourInfo(); // Met à jour les informations du personnage
        // _uiJeu.ActiverParticulesPouvoir((int)pouvoirAleatoire); // Active les particules du pouvoir du personnage. #synthese Leon

        for (int i = 0; i < nbAutels; i++) // Boucle pour placer les autels.
        {
            int indexAutel = i;
            Autels autelModele = _tAutelsModeles[indexAutel]; // Obtient le prefab de l'autel.


            Vector2Int pos = ObtenirPosLibre(); // Obtient une position libre aléatoire.

            // Vérifie si la position du dessus est vide et que la position du dessous n'est pas vide.
            if (PositionDessusEstVide(pos) && PositionDessousEstOccupee(pos))
            {

                if (autelModele.pouvoir == pouvoirAleatoire) // Si le pouvoir de l'autel est le même que celui du personnage.
                {
                    Debug.LogWarning("Même pouvoir que le personnage, " + pouvoirAleatoire); // Affiche un message d'avertissement.
                    continue; // Passe à l'itération suivante.
                }
                Vector3 pos3 = (Vector3)(Vector2)pos + _tilemapNiveau.transform.position + _tilemapNiveau.tileAnchor; // Convertit la position
                pos3 += new Vector3(0, 1, 0); // Positionne l'autel au dessus de la position.
                RaycastHit2D hit = Physics2D.Raycast(pos3, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Effector"));
                RaycastHit2D hit2 = Physics2D.Raycast(pos3, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Autels"));
                if (hit.collider == null && hit2.collider == null)
                {
                    Instantiate(autelModele, pos3, Quaternion.identity, contenant); // Crée le joyau à la position obtenue.
                }
                else
                {
                    i--;
                }
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

    void PlacerEnnemis()
    {
        Transform contenant = new GameObject("Ennemis").transform; // Crée un GameObject pour contenir les ennemis.
        contenant.parent = transform; // Assigne le niveau comme parent du contenant.

        foreach (Salle salle in _lesSalles)
        {
            for (int i = 0; i < salle.tReperesEnnemis.Length; i++)
            {
                int indexEnnemi = Random.Range(0, _tEnnemis.Length); // Sélectionne un ennemi aléatoire.
                GameObject ennemiModele = _tEnnemis[indexEnnemi]; // Obtient le prefab de l'ennemi.
                salle.PlacerEnnemiSurRepere(ennemiModele, i, contenant); // Place l'ennemi sur le repère de la salle.
            }

        }
    }

    /// <summary>
    /// #tp3 Antoine
    /// Méthode pour vérifier si la position du dessus est vide.
    /// </summary>
    /// <param name="pos">Position à vérifier</param>
    /// <return>Si la position du dessus est vide</return>
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


    /// <summary>
    /// #tp3 Antoine
    /// Méthode pour vérifier si la position du dessous est occupée.
    /// </summary>
    /// <param name="pos">Position a verifier</param>
    /// <returns>Si la position est occuppé</returns>
    bool PositionDessousEstOccupee(Vector2Int pos)
    {
        // Debug.Log(pos);
        Vector2Int posDessous = new Vector2Int(pos.x, pos.y - 1);

        // Changement du fonctionnement pour utiliser le tilemap car les 
        // positions libres ne sont pas toujours fiables. #synthese Leon
        if (_tilemapNiveau.GetTile((Vector3Int)posDessous) != null)
        {
            return true;
        }
        return false;

        // if (Physics2D.OverlapCircle(posDessous, 0.5f).CompareTag("Effector"))
        // {
        //     return false;
        // }

        // return !_lesPosLibres.Contains(posDessous);
    }


    /// <summary>
    /// #tp3 Antoine
    /// Méthode pour placer les items dans les salles du niveau.
    /// </summary>
    /// <param name="perso">Personnage à placer</param>
    /// <param name="porte">Porte à placer</param>
    /// <param name="cle">Clé à placer</param>
    /// <param name="activateur">Activateur à placer</param>
    void PlacerItems(Perso perso, GameObject porte, GameObject cle, GameObject activateur) // #tp3 Antoine
    {
        Transform contenant = transform; // GameObject parent des items
        Niveau niveau = GetComponent<Niveau>(); // Récupérer le composant Niveau
        
        // Placer le personnage.
        Vector2Int posPerso = ObtenirPosLibre(); // Obtenir une position libre aléatoire.
        Vector3 pos3Perso = (Vector3)(Vector2)posPerso + _tilemapNiveau.transform.position + _tilemapNiveau.tileAnchor; // Convertir la position en Vector3.
        _clonePerso = Instantiate(perso, pos3Perso, Quaternion.identity, contenant); // Instancier le personnage.

        // Placer la porte.
        int sallePorteIndex = Random.Range(0, _lesSallesSurBordure.Count); // Prise aléatoire d'un chiffre entre 0 et le nombre de salles.
        Salle sallePorte = _lesSallesSurBordure[sallePorteIndex]; // Récupère la salle aléatoire
        PlacerObjets(porte, sallePorte); // Placer la porte sur le repère de sa salle

        // Placer la clé.
        _lesSallesSurBordure.Reverse();
        Salle salleCle = _lesSallesSurBordure[sallePorteIndex]; // Récupère la salle opposée à la salle de la
        PlacerObjets(cle, salleCle); // Placer la clé sur le repère de sa salle
        _lesSallesSurBordure.Remove(salleCle); // Retirer la salle de la liste
        _lesSallesSurBordure.Remove(sallePorte); // Retirer la salle de la liste

        // Placer l'activateur
        Salle salleActivateur = _lesSallesSurBordure[Random.Range(0, _lesSallesSurBordure.Count)]; // Choisir une salle aléatoire restante pour placer l
        PlacerObjets(activateur, salleActivateur); // Placer l'activateur sur le repère choisi
    }

    /// <summary>
    /// Méthode qui permet de placer les objets désirés dans les salles du niveau
    /// </summary>
    /// <param name="objet">Objet que l'on veut placer</param>
    /// <param name="salle">Salle à faire apparaître l'objet</param>
    void PlacerObjets(GameObject objet, Salle salle) // #synthese Antoine
    {
        Vector2Int decalage = Vector2Int.CeilToInt(_tilemapNiveau.transform.position); // Décalage pour la position du repère.
        Vector2Int posRep = salle.PlacerSurRepere(objet) - decalage; // Placer l'objet sur un repère aléatoire de la salle
        Vector2Int Rep = Vector2Int.FloorToInt((Vector2)salle._repereObjet.transform.position); // Position du repère
        _lesPosSurReperes.Add(Rep); // Ajouter la position du repère à la liste
    }





    /// <summary>
    /// #tp3 Léon
    /// Méthode pour obtenir une position libre aléatoire dans le niveau. 
    /// </summary>
    /// <returns>Position libre aléatoire</returns>
    Vector2Int ObtenirPosLibre()
    {
        int indexPosLibre = Random.Range(0, _lesPosLibres.Count);
        Vector2Int pos = _lesPosLibres[indexPosLibre];
        _lesPosLibres.RemoveAt(indexPosLibre);
        return pos;
    }

    /// <summary>
    /// #tp4 Léon
    /// Méthode pour définir la taille du niveau selon le niveau du joueur
    void DefinirTailleNiveau()
    {
        //Définir la taille du niveau selon le niveau du joueur
        switch (_donneesPerso.niveau)
        {
            case 1:
                _taille = new Vector2Int(2, 2);
                break;
            case 2:
                _taille = new Vector2Int(2, 3);
                break;
            default:
                _taille = new Vector2Int(_donneesPerso.niveau, 3);
                break;
        }
    }

    /// <summary>
    /// #tp3 Léon
    /// Méthode pour créer le niveau, incluant enlever les positions prises par des effectors dans la liste des positions libres
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

                if (x == 0 || y == 0 || x == _taille.x - 1 || y == _taille.y - 1)
                {
                    _lesSallesSurBordure.Add(salle);
                }

                // Nomme la salle selon sa position dans le niveau.
                salle.name = "Salle" + x + "_" + y;

                _lesSalles.Add(salle); // Ajoute la salle au tableau des salles.

                niveauSurBordure.Add(new Vector2Int(x, y));//Test


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
        Debug.Log(string.Join(", ", _lesSallesSurBordure)); //Test
        // Debug.Log(string.Join(", ", niveauSurBordure)); //Test

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

        }
        // Pour les positions prises par des repères, on les enlève de la liste des positions libres. 
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
        // Debug.Log(_lesPosLibres.Count + " espaces libres : " + string.Join(", ", _lesPosLibres));
    }

    /// <summary>
    /// #tp3 Léon
    /// Méthode publique appelée par Joyau qui libère une position dans la liste des positions libres.
    /// </summary>
    public void LibererUnePos(Vector3 posPrecise)
    {
        Vector2Int pos = Vector2Int.FloorToInt(posPrecise - _tilemapNiveau.transform.position);
        _lesPosLibres.Add(pos);
    }

    /// <summary>
    /// Méthode publique appelée par CarteTuiles qui ajoute une tuile à la tilemap du niveau.
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

    IEnumerator CoroutineDecoulerTemps()
    {
        while (_temps > 0)
        {
            // Debug.Log("Temps : " + _temps);
            yield return new WaitForSeconds(1);
            _temps--;
            _uiJeu.MettreAJourTemps(_temps);
        }
        _donneesNavigation.AllerSceneTableauHonneur();
        // Debug.Log("Temps écoulé");
        // SceneManager.LoadScene("SceneTitre");
    }

    public void ArreterCoroutine()
    {
        StopAllCoroutines();
    }
}
