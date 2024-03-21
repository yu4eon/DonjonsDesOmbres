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
    bool _estEnPlay = true;
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

    void MettreAJourInfos()
    {
        _champArgent.text = _donneesPerso.argent + " $";
        _champNiveau.text = "Niveau " + _donneesPerso.niveau;
    }

    void OnApplicationQuit()
    {
        _estEnPlay = false;
        _donneesPerso.Initialiser();
        Debug.Log("Quit");
    }

    void OnDestroy()
    {
        _donneesPerso.evenementMiseAJour.RemoveAllListeners();
        if(_estEnPlay) _donneesPerso.niveau++;
        Debug.Log("Destroy");
    }
}