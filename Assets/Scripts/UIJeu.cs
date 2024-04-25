using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIJeu : MonoBehaviour
{
    [SerializeField] private SOPerso _donneesPerso;

    [SerializeField] private TextMeshProUGUI _champNiveau;
    [SerializeField] private TextMeshProUGUI _champTemps;
    [SerializeField] private TextMeshProUGUI _champScore;
    [SerializeField] private TextMeshProUGUI _champArgent;


    
    // Start is called before the first frame update
    void Start()
    {
        _champNiveau.text = "Lvl : " + _donneesPerso.niveau;
        _champScore.text = _donneesPerso.score +"";
        _champArgent.text = _donneesPerso.argent +" Gold";
    }

    public void MettreAJourScore()
    {
        _champScore.text = _donneesPerso.score + "";
    }

    public void MettreAJourArgent()
    {
        _champArgent.text = _donneesPerso.argent + " Gold";
    }
}
