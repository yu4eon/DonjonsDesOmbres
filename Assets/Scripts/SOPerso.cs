using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SOPerso", menuName = "SOPerso")]

/// <summary>
/// #tp3
/// Auteur du code : Antoine Lachance, Léon Yu
/// Commentaires ajoutés par : Antoine Lachance, Léon Yu
/// </summary>
public class SOPerso : ScriptableObject
{
    [Header("Valeurs actuelles")]
    [SerializeField, Range(1, 5)] int _niveau = 1; // Niveau du personnage
    [SerializeField, Range(0, 500)] int _argent = 100; // Argent du personnage
    [SerializeField, Range(0, 100000)] int _score = 0; // Score du personnage #tp4 Leon

    [Header("Valeurs initiales")]
    [SerializeField, Range(1, 5)] int _niveauIni = 1; // Niveau initial du personnage
    [SerializeField, Range(0, 500)] int _argentIni = 100; // Argent initial du personnage
    [SerializeField, Range(0, 100000)] int _scoreIni = 0; // Score du personnage #tp4 Leon

    [Header("Stats")]
    [SerializeField] int baseAttaque = 10; // Attaque de base du personnage
    int attaqueBonus;
    [SerializeField] int baseDefense = 10;  // Défense de base du personnage
    int defenseBonus;
    [SerializeField] int basePv = 10; // Points de vie de base du personnage
    int pvBonus; // Points de vie bonus du personnage
    List<TypePouvoir> _pouvoirs = new List<TypePouvoir>(); // Liste des pouvoirs du personnage
    public List<TypePouvoir> pouvoirs => _pouvoirs; // Propriété pour accéder à la liste des pouvoirs du personnage

    public int niveau // Propriété pour accéder et modifier le niveau du personnage
    {
        get => _niveau;
        set
        {
            _niveau = Mathf.Clamp(value, 1, int.MaxValue); 
            _evenementMiseAJour.Invoke(); // Appelle l'événement de mise à jour
        }
    }

    public int argent // Propriété pour accéder et modifier l'argent du personnage
    {
        get => _argent;
        set
        {
            _argent = Mathf.Clamp(value, 0, int.MaxValue);
            _evenementMiseAJour.Invoke(); // Appelle l'événement de mise à jour
        }

    }
    public int score // Propriété pour accéder et modifier le score du personnage #tp4 Leon
    {
        get => _score;
        set
        {
            _score = Mathf.Clamp(value, 0, int.MaxValue);
            _evenementMiseAJour.Invoke(); // Appelle l'événement de mise à jour
        }
    }

    UnityEvent _evenementMiseAJour = new UnityEvent(); // Événement de mise à jour
    public UnityEvent evenementMiseAJour => _evenementMiseAJour; // Propriété pour accéder à l'événement de mise à jour

    List<SOObjet> _lesObjets = new List<SOObjet>(); // Liste des objets du personnage


    public void Initialiser()
    {
        _niveau = _niveauIni;
        _argent = _argentIni;
        _score = _scoreIni; //Initialise le score à sa valeur initiale #tp4 Leon
        ViderInventaire();
        // Reset tout les variable des SOObjet estAcheter à false
    }

    /// <summary>
    /// #tp3 Antoine et Léon
    /// Méthode qui permet d'acheter un objet et d'ajuster les stats du personnage en conséquence
    /// </summary>
    /// <param name="donneesObjet"></param>
    public void Acheter(SOObjet donneesObjet)
    {
        argent -= donneesObjet.prix;
        _lesObjets.Add(donneesObjet);
        AfficherInventaire();
        // Leon : J'ai changé les ifs pour que ça check le type d'objet au lieu du nom de l'objet
        if (donneesObjet.typeObjet == TypeObjet.Attaque) attaqueBonus += 10; Debug.Log("Bonus attaque: " + attaqueBonus);
        if (donneesObjet.typeObjet == TypeObjet.DefensePV) //Si l'objet est de type DefensePV, ajoute 10 à la défense et aux points de vie
        {
            defenseBonus += 10;
            pvBonus += 10;
            Debug.Log("Bonus defense: " + defenseBonus);
            Debug.Log("Bonus pv: " + pvBonus);
        } 
        if (donneesObjet.typeObjet == TypeObjet.DoubleSaut) Perso.possedeDoublesSauts = true;
        if (donneesObjet.estAcheter == false) donneesObjet.estAcheter = true;
    }

    /// <summary>
    /// #tp3 Antoine
    /// Méthode qui ajoute de l'argent au personnage
    /// </summary>
    /// <param name="value">Argent qu'on ajoute au joueur</param>
    public void AjouterArgent(int value)
    {
        argent += value;
        Debug.Log("Argent ajouté: " + argent);
        AjouterScore(value * 10); //Ajoute le montant de l'argent au score multiplié par 10
    }

    /// <summary>
    /// #tp4 Leon
    /// Méthode qui ajoute du score au personnage
    /// </summary>
    /// <param name="value">Score qu'on ajoute au joueur</param>
    public void AjouterScore(int value) 
    {
        score += value;
        Debug.Log("Score ajouté: " + score);
    }

    /// <summary>
    /// #tp3 Antoine
    /// Méthode qui affiche l'inventaire du joueur
    /// </summary>
    void AfficherInventaire()
    {
        string inventaire = "";
        foreach (SOObjet objet in _lesObjets) //Pour chaque objet dans la liste des objets
        {
            if (inventaire != "") //Si l'inventaire n'est pas vide
            {
                inventaire += ", "; //Ajoute une virgule
            }
            inventaire += objet.nom;
        }
        Debug.Log("Inventaire du perso : " + inventaire);
    }

    /// <summary>
    /// #tp3 Leon 
    /// Méthode qui vide l'inventaire du joueur et remet les stats à leur valeur de base
    /// </summary>
    public void ViderInventaire()
    {
        foreach (SOObjet objet in _lesObjets) //Pour chaque objet dans la liste des objets
        {
            objet.estAcheter = false; //Remet l'objet à non acheté
        }
        _pouvoirs.Clear(); //Vide la liste des pouvoirs 
        _lesObjets.Clear(); //Vide la liste des objets
        attaqueBonus = 0; //Remet l'attaque à 0
        defenseBonus = 0; //Remet la défense à 0
        pvBonus = 0; //Remet les points de vie à 0
        Perso.possedeDoublesSauts = false; //Remet le double saut à false
    }
    /// <summary>
    /// tp3 Leon
    /// Méthode qui ajoute un pouvoir au personnage
    /// </summary>
    /// <param name="nomPouvoir">Type de pouvoir à ajouter</param>
    public void AjouterPouvoir(TypePouvoir nomPouvoir)
    {
        string pouvoirPerso = "";
        if (!_pouvoirs.Contains(nomPouvoir)) //Si le pouvoir n'est pas déjà dans la liste des pouvoirs
        {
            _pouvoirs.Add(nomPouvoir); //Ajoute le pouvoir à la liste des pouvoirs
            foreach (TypePouvoir pouvoir in _pouvoirs) //Pour chaque pouvoir dans la liste des pouvoirs
            {
                if (pouvoirPerso != "") //Si la liste des pouvoirs n'est pas vide
                {
                    pouvoirPerso += ", "; //Ajoute une virgule
                }
                pouvoirPerso += pouvoir; //Ajoute le pouvoir à la liste des pouvoirs
            }
            Debug.Log("Pouvoirs actuelles :" + pouvoirPerso);
        }

    }

    public bool ContientPouvoir(TypePouvoir nomPouvoir)
    {
        return _pouvoirs.Contains(nomPouvoir);
    }

    void OnValidate()
    {
        _evenementMiseAJour.Invoke();
    }
}
