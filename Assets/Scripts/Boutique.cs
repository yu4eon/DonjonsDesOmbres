using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Boutique : MonoBehaviour
{
    [SerializeField] SOPerso _donneesPerso;
    public SOPerso donneesPerso => _donneesPerso;

    [SerializeField] TextMeshProUGUI _champNiveau;
    [SerializeField] TextMeshProUGUI _champArgent;

    static Boutique _instance;
    static public Boutique instance => _instance;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        MettreAJourInfos();
        _donneesPerso.evenementMiseAJour.AddListener(MettreAJourInfos);
    }

    private void MettreAJourInfos()
    {
        _champNiveau.text = "Niveau " + _donneesPerso.niveau;
        _champArgent.text = _donneesPerso.argent + " $";
    }

    void OnApplicationQuit()
    {
        _donneesPerso.Initialiser();
    }

    
    void OnDestroy()
    {
        _donneesPerso.evenementMiseAJour.RemoveAllListeners();
        _donneesPerso.niveau++;
    }
}
