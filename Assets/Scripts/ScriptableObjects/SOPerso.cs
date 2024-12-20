using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

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
    [SerializeField] int _baseAttaque = 10; // Attaque de base du personnage
    int _attaqueBonus = 0; // Attaque bonus du personnage, obtenu par l'achat d'objets

    // Attaque du personnage en ajoutant l'attaque de base et l'attaque bonus  #synthese Leon
    int _attaque => _baseAttaque + _attaqueBonus;
    public int attaque => _attaque; // Propriété pour accéder à l'attaque du personnage

    [SerializeField] int baseDefense = 10;  // Défense de base du personnage
    int _defenseBonus; // Défense bonus du personnage, obtenu par l'achat d'objets
    // Défense du personnage en ajoutant la défense de base et la défense bonus  #synthese Leon
    int _defense => baseDefense + _defenseBonus;
    public int defense => _defense; // Propriété pour accéder à la défense du personnage
    [SerializeField] int _basePv = 100; // Points de vie de base du personnage
    int _pvBonus; // Points de vie bonus du personnage, obtenu par l'achat d'objets
    // Points de vie du personnage en ajoutant les points de vie de base et les points de vie bonus  #synthese Leon
    int _pvIni => _basePv + _pvBonus;
    public int pvIni => _pvIni; // Propriété pour accéder aux points de vie du personnage
    int _pv; // Points de vie actuels du personnage
    List<TypePouvoir> _pouvoirs = new List<TypePouvoir>(); // Liste des pouvoirs du personnage
    public List<TypePouvoir> pouvoirs => _pouvoirs; // Propriété pour accéder à la liste des pouvoirs du personnage

    public int pv // Propriété pour accéder et modifier les points de vie du personnage
    {
        get => _pv;
        set
        {
            _pv = Mathf.Clamp(value, 0, pvIni);
            _evenementMiseAJour.Invoke(); // Appelle l'événement de mise à jour
        }
    }

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

    Dictionary<SOObjet, int> _dObjets = new Dictionary<SOObjet, int>(); // Dictionnaire des objets du personnage #synthese Leon


    /// <summary>
    /// #tp4 Leon
    /// Méthode qui initialise les valeurs du personnage
    /// </summary>
    public void Initialiser()
    {
        _niveau = _niveauIni;
        _argent = _argentIni;
        _score = _scoreIni; //Initialise le score à sa valeur initiale #tp4 Leon
        ViderInventaire();
        InitialiserVie();
    }


    /// <summary>
    /// #synthese Leon
    /// Méthode qui initialise les points de vie du personnage
    /// afin de lui redonner les points de vie initiales
    /// </summary>
    public void InitialiserVie()
    {
        _pv = _pvIni;
    }

    /// <summary>
    /// #tp3 Antoine et Léon
    /// Méthode qui permet d'acheter un objet et d'ajuster les stats du personnage en conséquence
    /// </summary>
    /// <param name="donneesObjet"></param>
    public void Acheter(SOObjet donneesObjet)
    {
        argent -= donneesObjet.prix;
        if (_dObjets.ContainsKey(donneesObjet))
        {
            Mathf.Clamp(_dObjets[donneesObjet]++, 0, 99);
        }
        else
        {
            _dObjets.Add(donneesObjet, 1);
        }
        AfficherInventaire();
        // Leon : J'ai changé les ifs pour que ça check le type d'objet au lieu du nom de l'objet
        if (donneesObjet.typeObjet == TypeObjet.Attaque) _attaqueBonus += 3; Debug.Log("Bonus attaque: " + _attaqueBonus);
        if (donneesObjet.typeObjet == TypeObjet.DefensePV) //Si l'objet est de type DefensePV, ajoute 10 à la défense et aux points de vie
        {
            _defenseBonus += 10;
            _pvBonus += 30;
            Debug.Log("Bonus defense: " + _defenseBonus);
            Debug.Log("Bonus pv: " + _pvBonus);
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
        // Debug.Log("Argent ajouté: " + argent);
    }

    /// <summary>
    /// #tp4 Leon
    /// Méthode qui ajoute du score au personnage
    /// </summary>
    /// <param name="value">Score qu'on ajoute au joueur</param>
    public void AjouterScore(int value)
    {
        score += value;
        // Debug.Log("Score ajouté: " + score);
    }

    /// <summary>
    /// #tp3 Antoine
    /// Méthode qui affiche l'inventaire du joueur
    /// (#synthese Leon, Affiche l'inventaire du joueur dans la boutique et dans le niveau)
    /// </summary>
    public void AfficherInventaire()
    {
        PanneauVignette panneauVignette = null; //Crée une variable pour le panneau de la vignette précédente

        if (Boutique.instance != null) //Si la boutique existe
        {
            Boutique.instance.panneauInventaire.Vider(); //Vide le panneau d'inventaire
            foreach (KeyValuePair<SOObjet, int> objet in _dObjets) //Pour chaque objet dans la liste des objets
            {
                panneauVignette = Boutique.instance.panneauInventaire.Ajouter(objet.Key); //Ajoute l'objet au panneau d'inventaire
                panneauVignette.nb = objet.Value; //Indique le nombre d'objets dans le panneau de la vignette
            }
        }
        else if (Niveau.instance != null) //Si le niveau existe
        {
            foreach (KeyValuePair<SOObjet, int> objet in _dObjets) //Pour chaque objet dans la liste des objets
            {
                panneauVignette = Niveau.instance.panneauInventaire.Ajouter(objet.Key); //Ajoute l'objet au panneau d'inventaire
                panneauVignette.nb = objet.Value; //Indique le nombre d'objets dans le panneau de la vignette
            }
        }
    }

    /// <summary>
    /// #tp3 Leon 
    /// Méthode qui vide l'inventaire du joueur et remet les stats à leur valeur de base
    /// </summary>
    public void ViderInventaire()
    {
        _pouvoirs.Clear(); //Vide la liste des pouvoirs 
        _dObjets.Clear(); //Vide le dictionnaire des objets
        _attaqueBonus = 0; //Remet l'attaque à 0
        _defenseBonus = 0; //Remet la défense à 0
        _pvBonus = 0; //Remet les points de vie à 0
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


    /// <summary>
    /// #tp4 Leon
    /// méthode qui vérifie si le personnage possède un pouvoir
    /// </summary>
    /// <param name="nomPouvoir">Type de pouvoir à vérifier</param>
    /// <returns>Retourne vrai si le personnage possède le pouvoir, faux sinon</returns>
    public bool ContientPouvoir(TypePouvoir nomPouvoir)
    {
        return _pouvoirs.Contains(nomPouvoir);
    }

    void OnValidate()
    {
        _evenementMiseAJour.Invoke();
    }
}
