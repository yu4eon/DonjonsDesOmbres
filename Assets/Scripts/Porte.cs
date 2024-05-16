using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

/// <summary>
/// #Tp3
/// Auteur du code : Antoine Lachance
/// Commentaires ajoutés par : Antoine Lachance
/// Classe qui gère la porte
/// </summary>
public class Porte : MonoBehaviour
{
    [SerializeField] SONavigation _navigation; // Référence au script de navigation entre les scènes
    [SerializeField] Sprite[] _sprites; // Tableau des sprites de la porte (ouverte et fermée)
    [SerializeField] SOPerso _donneesPerso; // Données du personnage (ScriptableObject)
    [SerializeField] GameObject _lumiere; // Référence à la lumière de la porte #tp4 Leon
    [SerializeField] AudioClip _sonPorte;
    [SerializeField] GameObject fond;
    [SerializeField] GameObject panneauBonus;
    [SerializeField] GameObject[] panneauJoueur;

    SpriteRenderer _sr; // Référence au composant SpriteRenderer de la porte
    static public bool aCle = false; // Booléen indiquant si la clé a été trouvée


    /// <summary>
    /// Start est appelée lors du premier frame où le script est activé, avant n'importe quel Update.
    /// </summary>
    void Start()
    {
        fond = GameObject.Find("Fond");
        panneauBonus = GameObject.Find("PointsBonus");
        panneauJoueur = GameObject.FindGameObjectsWithTag("PanneauJoueur");
        _lumiere.SetActive(false); // Désactive la lumière de la porte #tp4 Leon
        aCle = false; // Réinitialisation de la variable aCle
        _sr = GetComponent<SpriteRenderer>(); // Obtention du composant SpriteRenderer attaché à cet objet
        _sr.sprite = _sprites[0]; // Définition du sprite initial de la porte (fermée)

        fond.SetActive(false);
        panneauBonus.SetActive(false);
        foreach (var item in panneauJoueur)
        {
            item.SetActive(true);
        }
    }

    /// <summary>
    /// OnTriggerEnter2D est appelée lorsque quelque chose entre en collision avec le collider de cet objet.
    /// </summary>
    /// <param name="other">Le Collider2D entrant en collision avec cet objet.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (aCle && other.CompareTag("Player")) // Si la clé a été trouvée et le joueur entre en collision avec la porte
        {
            SceneBonus();
            Perso perso = other.GetComponent<Perso>();
            perso.DesactiverInputs();
            _sr.sprite = _sprites[1]; // Changement du sprite de la porte (ouverte)
            _lumiere.SetActive(true); // Active la lumière de la porte #tp4 Leon
            // Coroutine _coroutine = StartCoroutine(ChangerScene()); // Appel de la coroutine pour changer de scène après un délai
            SoundManager.instance.JouerEffetSonore(_sonPorte); // Joue le son de la porte
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

    void SceneBonus()
    {
        // Active l'objet 'fond' dans la scène.
        fond.SetActive(true);
        // Active l'objet 'panneauBonus' dans la scène.
        panneauBonus.SetActive(true);

        Niveau.instance.ArreterCoroutine(); // Arrête la coroutine du niveau

        panneauBonus.GetComponent<PanneauBonus>().CalculerPoints(); // Appelle la fonction 'CalculerPoints' du script 'PanneauBonus'

        // Désactive tous les objets enfants de 'panneauJoueur'.
        foreach (var item in panneauJoueur)
        {
            item.SetActive(false);
        }

        // Définit le texte du premier composant TextMeshProUGUI trouvé dans 'panneauBonus'.
        panneauBonus.GetComponentInChildren<TextMeshProUGUI>().text = "Bonus Collected!";

        // Vide l'inventaire du joueur.
        _donneesPerso.ViderInventaire(); // Vidage de l'inventaire du joueur
    }

    /// <summary>
    /// Callback sent to all game objects before the application is quit.
    /// </summary>
    void OnApplicationQuit()
    {
        fond.SetActive(false); // Désactive l'objet 'fond' dans la scène
        panneauBonus.SetActive(false); // Désactive l'objet 'panneauBonus' dans la scène
        foreach (var item in panneauJoueur) // Réactive tous les objets enfants de 'panneauJoueur'
        {
            item.SetActive(true);
        }
    }
}
