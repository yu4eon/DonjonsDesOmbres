using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class PanneauObjet : MonoBehaviour
{
    [Header("LES DONNÉES")]
    [SerializeField] SOObjet _donnees;
    public SOObjet donnees => _donnees;

    [Header("LES CONTENEURS")]
    [SerializeField] TextMeshProUGUI _champNom;
    [SerializeField] TextMeshProUGUI _champPrix;
    [SerializeField] TextMeshProUGUI _champDescription;
    [SerializeField] Image _image;
    [SerializeField] CanvasGroup _canvasGroup;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Start()
    {
        MettreAJourInfos();
        Boutique.instance.donneesPerso.evenementMiseAJour.AddListener(MettreAJourInfos);
    }

    void MettreAJourInfos()
    {
        _champNom.text = _donnees.nom;
        _champPrix.text = _donnees.prix + " $";
        _champDescription.text = _donnees.description;
        _image.sprite = _donnees.sprite;
        GererDispo();
    }

    void GererDispo()
    {
        bool aNiveauRequis = Boutique.instance.donneesPerso.niveau >= _donnees.niveauRequis;
        bool aAssezArgent = Boutique.instance.donneesPerso.argent >= _donnees.prix;
        if(aNiveauRequis && aAssezArgent)
        {
            _canvasGroup.interactable = true;
            _canvasGroup.alpha = 1;
        }
        else
        {
            _canvasGroup.interactable = false;
            _canvasGroup.alpha = .5f;
        }
    }

    public void Acheter()
    {
        Boutique.instance.donneesPerso.Acheter(_donnees);
    }
}