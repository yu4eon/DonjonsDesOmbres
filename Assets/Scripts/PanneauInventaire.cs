using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanneauInventaire : MonoBehaviour
{
    [SerializeField] PanneauVignette _panneauVignetteModele;


    void Start()
    {
        if(Boutique.instance != null)
        {
            Boutique.instance.donneesPerso.AfficherInventaire();
        }
    }
    public PanneauVignette Ajouter(SOObjet donnees)
    {
        PanneauVignette vignette = Instantiate(_panneauVignetteModele, transform);
        vignette.donneesObjet = donnees;
        return vignette;
    }
    
    public void Vider()
    {
        foreach (Transform enfant in transform)
        {
            Destroy(enfant.gameObject);
        }
    }
}
