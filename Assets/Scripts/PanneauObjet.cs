using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    // [SerializeField] CanvasGroup _canvasGroup;

    void Awake()
    {
        _champNom.text = _donnees.nom;
        _champPrix.text = _donnees.prixDeBase.ToString();
        _champDescription.text = _donnees.description;
        _image.sprite = _donnees.sprite;
    }
}

