using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanneauVignette : MonoBehaviour
{
    [SerializeField] Image _image; // Image de la vignette
    [SerializeField] TextMeshProUGUI _champ;
    SOObjet _donneesObjet;
    public SOObjet donneesObjet
    {
        get { return _donneesObjet; }
        set
        {
            _donneesObjet = value;
            _image.sprite = _donneesObjet.sprite;
        }
    }	
    int _nb = 1;
    public int nb
    {
        get { return _nb; }
        set
        {
            _nb = value;
            _champ.text = _nb.ToString();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        nb = nb;
    }
}
