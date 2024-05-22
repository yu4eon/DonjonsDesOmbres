using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// #tp4
/// Auteur du code : Léon Yu
/// Commentaires ajoutés par : Léon Yu
/// Classe qui gère l'affichage des informations du personnage dans le jeu
/// </summary>
public class UIJeu : MonoBehaviour
{
    [SerializeField] SOPerso _donneesPerso; // Données du personnage, (ScriptableObject)
    [SerializeField] TextMeshProUGUI _champNiveau; // Champ de texte pour le niveau du personnage
    [SerializeField] TextMeshProUGUI _champTemps; // Champ de texte pour le temps de jeu
    [SerializeField] TextMeshProUGUI _champScore; // Champ de texte pour le score du personnage
    [SerializeField] TextMeshProUGUI _champArgent; // Champ de texte pour l'argent du personnage
    [SerializeField] RectTransform _barreVie; // Barre de vie du personnage
    [SerializeField] Image[] _tCrystalsPouvoir; // Tableau d'images pour les cristaux de pouvoir
    [SerializeField] ParticleSystem[] _tParticulesPouvoir; // Particules de pouvoir
    Dictionary<TypePouvoir, Image> _dCrystalsPouvoir = new Dictionary<TypePouvoir, Image>(); // Dictionnaire de cristaux de pouvoir
    RectTransform[] _tRectCrystalsPouvoir; // Tableau de RectTransform pour les cristaux de pouvoir
    Vector2 _tailleInactif = new Vector2(50, 50); // Taille des cristaux inactifs
    Vector2 _tailleIni; // Taille initiale des cristaux
    Color _couleurCrystalInactif = new Color(0.7f, 0.7f, 0.7f); // Couleur des cristaux inactifs
    static UIJeu _instance; // Instance statique de l'interface du jeu
    static public UIJeu instance => _instance; // Propriété publique pour accéder à l'instance de l'interface du jeu




    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        _tRectCrystalsPouvoir = new RectTransform[_tCrystalsPouvoir.Length];
        for (int i = 0; i < _tCrystalsPouvoir.Length; i++)
        {
            _tRectCrystalsPouvoir[i] = _tCrystalsPouvoir[i].GetComponent<RectTransform>();
        }
        _tailleIni = _tRectCrystalsPouvoir[0].sizeDelta;
    }
    // Start is called before the first frame update
    void Start()
    {
        // Initialisation des champs de texte
        
        _champNiveau.text = "Niv : " + _donneesPerso.niveau; 
        _champScore.text = _donneesPerso.score +"";
        _champArgent.text = _donneesPerso.argent +" Gold";
        _donneesPerso.evenementMiseAJour.AddListener(MettreAJourInfo); // Ajoute l'événement de mise à jour

        foreach (RectTransform rect in _tRectCrystalsPouvoir)
        {
            rect.sizeDelta = new Vector2(0, 0);
        }


        // Initialisation du dictionnaire de cristaux de pouvoir
        //J'aurais pu faire un loop for mais avec les types de pouvoirs, c'est plus facile de le faire manuellement
        _dCrystalsPouvoir.Add(TypePouvoir.Poison, _tCrystalsPouvoir[0]);
        _dCrystalsPouvoir.Add(TypePouvoir.Ombre, _tCrystalsPouvoir[1]);
        _dCrystalsPouvoir.Add(TypePouvoir.Foudre, _tCrystalsPouvoir[2]);
        _dCrystalsPouvoir.Add(TypePouvoir.Glace, _tCrystalsPouvoir[3]);

        foreach (KeyValuePair<TypePouvoir, Image> entry in _dCrystalsPouvoir)
        {
            entry.Value.color = _couleurCrystalInactif;
        }
        
        foreach(ParticleSystem particule in _tParticulesPouvoir)
        {
            particule.Stop();
        }
    }


    /// <summary>
    /// Méthode qui met à jour les informations du personnage
    /// </summary>
    public void MettreAJourInfo()
    {
        _champScore.text = _donneesPerso.score + "";
        _champArgent.text = _donneesPerso.argent + " Or";


        float fractionVie;
        fractionVie = (float)_donneesPerso.pv / (float)_donneesPerso.pvIni;
        // Debug.Log("Fraction de vie : " + fractionVie);
        _barreVie.localScale = new Vector3(fractionVie, _barreVie.localScale.y, 1);

        //Mettre à jour les cristaux de pouvoir
        foreach (KeyValuePair<TypePouvoir, Image> entry in _dCrystalsPouvoir)
        {
            //Si le personnage a le pouvoir, on l'active
            if (_donneesPerso.ContientPouvoir(entry.Key)) // Assuming SOPerso has a method to check this
            {
                entry.Value.enabled = true;
                
            }
            else
            {
                entry.Value.enabled = false;
            }
        }
    } 

    /// <summary>
    /// #synthese Léon
    /// Méthode qui met à jour le temps de jeu
    /// </summary>
    /// <param name="temps">Le temps de jeu en secondes</param>
    public void MettreAJourTemps(int temps)
    {
        
        int minutes = temps / 60; 
        int secondes = temps % 60;
        _champTemps.text = ((minutes < 10)? "0" +minutes: minutes) + ":" + ((secondes < 10)? "0" +secondes: secondes);
    }

    /// <summary>
    /// #synthese Léon
    /// Méthode qui active un cristal de pouvoir
    /// </summary>
    public void ActiverPouvoir(int index)
    {
        foreach (RectTransform rect in _tRectCrystalsPouvoir) // On désactive tous les cristaux
        {
            rect.sizeDelta = _tailleInactif; 
        }
        foreach (KeyValuePair<TypePouvoir, Image> entry in _dCrystalsPouvoir) // Pour chaque cristal, on les met inactifs
        {
            entry.Value.color = _couleurCrystalInactif;
        }


        // On active le cristal sélectionné
        _tRectCrystalsPouvoir[index].sizeDelta = _tailleIni;
        _tCrystalsPouvoir[index].color = Color.white;
    }


    /// <summary>
    /// #synthese Léon
    /// Méthode qui joue les particules de pouvoir
    /// Joue lorsqu'on obtient un pouvoir
    /// </summary>
    public void JouerParticulesPouvoir(int index)
    {
        _tParticulesPouvoir[index].Play();
    }

}
