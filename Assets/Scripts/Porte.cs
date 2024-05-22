using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.EventSystems;
using Unity.Mathematics;

/// <summary>
/// #Tp3
/// Auteur du code : Antoine Lachance, Léon Yu
/// Commentaires ajoutés par : Antoine Lachance, Léon Yu
/// /// Classe qui gère la porte
/// </summary>
public class Porte : MonoBehaviour
{
    [SerializeField] SONavigation _navigation; // Référence au script de navigation entre les scènes
    [SerializeField] Sprite[] _sprites; // Tableau des sprites de la porte (ouverte et fermée)
    [SerializeField] SOPerso _donneesPerso; // Données du personnage (ScriptableObject)
    [SerializeField] Light2D _lumierePorte; // Référence à la lumière de la porte, actif au début pour rendre le portail plus visible #synthese Leon
    [SerializeField] Light2D _lumierePortail; // Référence à la lumière du portail de la porte #tp4 Leon
    [SerializeField] AudioClip _sonPorte;

    SpriteRenderer _sr; // Référence au composant SpriteRenderer de la porte
    bool _aCle = false; // Booléen indiquant si la clé a été trouvée 
    [SerializeField] float _limiteLuminosite = 4f; // Limite de l'intensité de la lumière de la porte #Synthese Leon
    [SerializeField] float _forceAjustementLuminosite = 3f; // Force pour ajuster la luminosité de la porte après avoir obtenu la clé #Synthese Leon
    Coroutine _coroutine; // Référence à la coroutine pour ajuster la luminosité de la porte après avoir obtenu la clé #Synthese Leon
    float _lumiereIntensiteIni; // Intensité initiale de la lumière de la porte #Synthese Leon



    /// <summary>
    /// Start est appelée lors du premier frame où le script est activé, avant n'importe quel Update.
    /// </summary>
    void Start()
    {

        _lumiereIntensiteIni = _lumierePorte.intensity; // Obtention de l'intensité initiale de la lumière de la porte #Synthese Leon
        _lumierePorte.gameObject.SetActive(true); // Active la lumière de la porte #tp4 Leon
        _lumierePortail.gameObject.SetActive(false); // Désactive la lumière de la porte #tp4 Leon
        _aCle = false; // Réinitialisation de la variable aCle
        _sr = GetComponent<SpriteRenderer>(); // Obtention du composant SpriteRenderer attaché à cet objet
        _sr.sprite = _sprites[0]; // Définition du sprite initial de la porte (fermée)
    }

    /// <summary>
    /// OnTriggerEnter2D est appelée lorsque quelque chose entre en collision avec le collider de cet objet.
    /// </summary>
    /// <param name="other">Le Collider2D entrant en collision avec cet objet.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (_aCle && other.CompareTag("Player")) // Si la clé a été trouvée et le joueur entre en collision avec la porte
        {
            SceneBonus();
            StopCoroutine(_coroutine); // Arrête la coroutine
            _lumierePorte.gameObject.SetActive(false); // Désactive la lumière de la porte #tp4 Leon
            Perso perso = other.GetComponent<Perso>();
            perso.DesactiverInputs();
            _sr.sprite = _sprites[1]; // Changement du sprite de la porte (ouverte)
            _lumierePortail.gameObject.SetActive(true); // Active la lumière de la porte #tp4 Leon
            // Coroutine _coroutine = StartCoroutine(ChangerScene()); // Appel de la coroutine pour changer de scène après un délai
            GestAudio.instance.JouerEffetSonore(_sonPorte); // Joue le son de la porte
        }
    }

    /// <summary>
    /// Coroutine qui change de scène après 2 secondes et vide l'inventaire du joueur.
    /// </summary>
    IEnumerator ChangerScene()
    {
        yield return new WaitForSeconds(2f); // Attente de 2 secondes
        _donneesPerso.ViderInventaire(); // Vidage de l'inventaire du joueur
        _navigation.AllerSceneSuivante(); // Appel de la fonction pour aller à la scène suivante dans le script de navigation
    }

    /// <summary>
    /// #synthese Léon
    /// Méthode qui active le bonus du niveau et arrête la coroutine du niveau
    /// </summary>
    void SceneBonus()
    {

        Niveau.instance.ActiverBonus(); // Active le bonus du niveau

        Niveau.instance.ArreterCoroutine(); // Arrête la coroutine du niveau


        // Vide l'inventaire du joueur.
        _donneesPerso.ViderInventaire(); // Vidage de l'inventaire du joueur
    }

    /// <summary>
    /// #synthese Léon
    /// Méthode qui indique que le joueur a la clé et ajuste la lumière de la porte.
    /// </summary>
    public void PossederCle()
    {
        _aCle = true; // Indique que le joueur a la clé
        _coroutine = StartCoroutine(CouroutineAjusterLumiere()); // Appel de la coroutine pour ajuster la lumière après un délai

    }

    /// <summary>
    /// #synthese Léon
    /// Coroutine qui ajuste la luminosité de la porte.
    /// </summary>
    IEnumerator CouroutineAjusterLumiere()
    {
        while (true)
        {
            _lumierePorte.intensity = Mathf.PingPong(Time.time * _forceAjustementLuminosite, _limiteLuminosite) + _lumiereIntensiteIni; // Ajustement de l'intensité de la lumière de la porte
            yield return null;
        }
    }
}
