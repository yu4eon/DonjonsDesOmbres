using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Retroaction : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _champ;
    
    public void ChangerTexte(string texte)
    {
        _champ.text = texte;
    }

    public void Detruire()
    {
        Destroy(gameObject);
    }
}
