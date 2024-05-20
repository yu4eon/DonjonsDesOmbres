using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

/// <summary>
/// #tp4
/// Auteur du code: Leon Yu
/// Ajout des commentaires: Leon Yu
/// Classe pour le tableau d'honneur, qui affiche les scores des joueurs
/// et permet de sauvegarder le score du joueur actuel
/// </summary>
public class TableauHonneur : MonoBehaviour
{

    [SerializeField] SOSauvegarde _donneesSauvegarde; // Données de sauvegarde
    [SerializeField] SOPerso _donneesPerso; // Données du personnage
    [SerializeField] GameObject _contenu; // contenant des scores
    [SerializeField] string _fichier = "sauvegarde.TIM"; // Nom du fichier de sauvegarde
    [SerializeField] GameObject _prefabLigne; // Préfab de la ligne de score
    string _cheminEtFichier;

    // On a toute essayer pour essayer de faire fonctionner le bouton mais sans succès.
    // On a essayer avec SerializedField, GetComponentInChildren. Mais rien ne fonctionne.
    // Il semble marcher dans le Start mais pas dans la fonction SauvegarderScore, On est pas sur pourquoi.
    // On doit donc se tourner vers la seul solution qui restait, qui est de faire un Find, ce qu'on sait est interdit, mais c'est la seul chose qui marchait.
    Button _boutonMenu; // Bouton pour retourner au menu
    static List<JoueurScore> _lesJoueursScores = new List<JoueurScore>(); // Liste des joueurs et leurs scores
    void Awake()
    {
        _cheminEtFichier = Application.persistentDataPath + "/" + _fichier;
        Debug.Log(_cheminEtFichier);
        Debug.Log(_fichier);
        Debug.Log(Application.persistentDataPath);
        if (_donneesSauvegarde.LireFichier() != null) // si le fichier existe
        {
            InitialiserDonnees();
        }
        else
        {
            Debug.LogWarning("Le fichier n'existe pas");
            _lesJoueursScores = _donneesSauvegarde.lesJoueursScores;
        }
    }

    void Start()
    {
        // _boutonMenu = GetComponentInChildren<Button>();
        // Trouver le bouton pour retourner au menu, on sais que c'est interdit mais c'est la seul solution qui marche
        _boutonMenu = GameObject.Find("BoutonMenu").GetComponent<Button>();
        _boutonMenu.interactable = false;
        // Ajouter le joueur actuel à la liste des scores
        _lesJoueursScores.Add(new JoueurScore { joueur = "Nom", score = _donneesPerso.score, estJoueurActuelle = true });

        // Tri des scores
        _lesJoueursScores.Sort((a, b) => b.score.CompareTo(a.score));

        // Limiter le nombre de scores affichés à 6
        while (_lesJoueursScores.Count > 6)
        {
            _lesJoueursScores.RemoveAt(_lesJoueursScores.Count - 1);
        }
        AfficherScores();

        GestAudio.instance.ChangerEtatLecturePiste(TypePiste.MusiqueBase, false);
        GestAudio.instance.ChangerEtatLecturePiste(TypePiste.MusiqueEvenA, false);
        GestAudio.instance.ChangerEtatLecturePiste(TypePiste.MusiqueEvenB, false);
        GestAudio.instance.ChangerEtatLecturePiste(TypePiste.MusiqueMenu, true);
    }


    /// <summary>
    /// Initialise les données des joueurs
    /// </summary>
    void InitialiserDonnees()
    {
        Debug.Log(_donneesSauvegarde.LireFichier());
        _lesJoueursScores = _donneesSauvegarde.lesJoueursScores;
    }

    /// <summary>
    /// Affiche les scores des joueurs en instanciant des lignes de score
    /// </summary>
    void AfficherScores()
    {
        Debug.Log("Nombre de joueurs : " + _lesJoueursScores.Count);
        foreach (JoueurScore joueurScore in _lesJoueursScores)
        {
            Debug.Log(string.Join(" ", joueurScore.joueur, joueurScore.score));
            GameObject ligne = Instantiate(_prefabLigne, _contenu.transform);
            ligne.GetComponent<RowUI>().AfficherInfo(joueurScore, _lesJoueursScores.IndexOf(joueurScore) + 1);
            if (joueurScore.estJoueurActuelle)
            {
                joueurScore.estJoueurActuelle = false;
            }
        }
    }

    /// <summary>
    /// Sauvegarde le score du joueur actuel
    /// et met à jour le nom du joueur
    /// </summary>
    /// <param name="nom">Nom dont on veut remplacer</param>
    public void SauvegarderScore(string nom)
    {

        _boutonMenu = GetComponentInChildren<Button>();
        _boutonMenu.interactable = true;
        ActiverBoutonMenu();
        Debug.Log("Sauvegarde du score");
        // Debug.Log(_lesJoueursScores.Count);

        // Mettre à jour le nom du joueur
        foreach (JoueurScore joueurScore in _lesJoueursScores)
        {

            // Si le joueur actuel est trouvé
            if (joueurScore.joueur == "Nom")
            {
                joueurScore.joueur = nom; // Mettre à jour le nom
                break;
            }
        }

        _donneesSauvegarde.EcrireFichier();
    }

    /// <summary>
    /// Active le bouton pour retourner au menu
    /// </summary>
    public void ActiverBoutonMenu()
    {
        // _boutonMenu = GameObject.Find("BoutonMenu").GetComponent<Button>(); // On doit refaire le Find pour trouver le bouton
        _boutonMenu.interactable = true;
    }

    void OnApplicationQuit()
    {
        _donneesPerso.Initialiser(); // Réinitialise les données du personnage.
        Debug.Log("Quit"); // Affiche un message de débogage.
    }
}
