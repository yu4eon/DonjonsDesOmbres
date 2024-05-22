using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// #synthese Léon
/// Classe pour la gestion du fond, qui peut fade
/// du noir au blanc
/// </summary>
public class FadeFromBlack : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------
    private Image _img; //Le composant image
    private float _alpha =1f; // Le niveau de transparence initial

    [SerializeField]
    [Range(0.1f, 5f)]
    private float _fadeSpeed; //Vitesse du fade

    // --- Initialisation -------------------------------------------------------

    private void Awake()
    {
        _img = GetComponent<Image>();
    }

    private void Start()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutBlack());
        ChangeAlpha(_alpha);
    }

    // --- Méthodes Privées -------------------------------------------------------

    /// <summary>
    /// Change la transparence de l'image
    /// </summary>
    /// <param name="valeur"> Le niveau de transparence </param>
    private void ChangeAlpha(float valeur)
    {
        _img.color = new Color(_img.color.r, _img.color.g, _img.color.b, valeur);
    }


    /// <summary>
    /// met le fond de noir à blanc
    /// </summary>
    IEnumerator FadeOutBlack()
    {
        while (_alpha >= 0f) //Tant que le niveau de transparence est supérieur à 0
        {
            _alpha -= Time.deltaTime * _fadeSpeed;

            if (_alpha <= 0f) //Si le niveau de transparence est inférieur ou égal à 0
            {
                ChangeAlpha(0f);
                
            }
            else
            {
                ChangeAlpha(_alpha); 
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }
        StopAllCoroutines();
    }
}
