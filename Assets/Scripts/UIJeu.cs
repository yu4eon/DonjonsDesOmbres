using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class UIJeu : MonoBehaviour
{
    [SerializeField] private SOPerso _donneesPerso;

    [SerializeField] private TextMeshProUGUI _champNiveau;
    [SerializeField] private TextMeshProUGUI _champTemps;
    [SerializeField] private TextMeshProUGUI _champScore;
    [SerializeField] private TextMeshProUGUI _champArgent;
    [SerializeField] private Image _barreVie;
    [SerializeField] private Image[] _tCrystalsPouvoir;
    Dictionary<TypePouvoir, Image> _dCrystalsPouvoir = new Dictionary<TypePouvoir, Image>();



    
    // Start is called before the first frame update
    void Start()
    {
        _champNiveau.text = "Lvl : " + _donneesPerso.niveau;
        _champScore.text = _donneesPerso.score +"";
        _champArgent.text = _donneesPerso.argent +" Gold";
        _donneesPerso.evenementMiseAJour.AddListener(MettreAJourInfo);



        // Initialisation du dictionnaire de cristaux de pouvoir
        //J'aurais pu faire un loop for mais avec les types de pouvoirs, c'est plus facile de le faire manuellement
        _dCrystalsPouvoir.Add(TypePouvoir.Poison, _tCrystalsPouvoir[0]);
        _dCrystalsPouvoir.Add(TypePouvoir.Ombre, _tCrystalsPouvoir[1]);
        _dCrystalsPouvoir.Add(TypePouvoir.Foudre, _tCrystalsPouvoir[2]);
        _dCrystalsPouvoir.Add(TypePouvoir.Glace, _tCrystalsPouvoir[3]);

    }

    void MettreAJourInfo()
    {
        _champScore.text = _donneesPerso.score + "";
        _champArgent.text = _donneesPerso.argent + " Gold";

        foreach (KeyValuePair<TypePouvoir, Image> entry in _dCrystalsPouvoir)
        {
            if (_donneesPerso.ContientPouvoir(entry.Key)) // Assuming SOPerso has a method to check this
            {
                entry.Value.enabled = true; // Enable the image
            }
            else
            {
                entry.Value.enabled = false; // Disable the image
            }
        }
    }
}
