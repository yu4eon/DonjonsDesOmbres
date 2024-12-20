using UnityEngine;
using TMPro;

/// <summary>
/// #tp3
/// Auteur du code : Antoine Lachance
/// commentaires ajoutés par : Antoine Lachance
/// Classe pour la boutique et affiche les informations du personnage.
/// </summary>
public class Boutique : MonoBehaviour
{
    [SerializeField] SOPerso _donneesPerso; // ScriptableObject contenant les données du personnage.
    public SOPerso donneesPerso => _donneesPerso; // Propriété publique pour accéder aux données du personnage.
    [SerializeField] TextMeshProUGUI _champNiveau; // Champ de texte pour afficher le niveau.
    [SerializeField] TextMeshProUGUI _champArgent; // Champ de texte pour afficher l'argent du personnage.
    [SerializeField] PanneauInventaire _panneauInventaire; // Panneau d'inventaire pour afficher les objets.
    public PanneauInventaire panneauInventaire => _panneauInventaire; // Propriété publique pour accéder au panneau d'inventaire.
    [SerializeField] AudioClip _sonCages; // Son d'achat de la boutique.

    static Boutique _instance; // Instance statique de la boutique.
    static public Boutique instance => _instance; // Propriété publique pour accéder à l'instance de la boutique.
    bool _estEnPlay = true; // Indique si l'application est en cours d'exécution.

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        MettreAJourInfos(); // Met à jour les informations affichées dans la boutique.
        _donneesPerso.evenementMiseAJour.AddListener(MettreAJourInfos); // Ajoute l'écouteur pour mettre à jour les infos.

    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        // Changement de l'état des pistes musicales. #synthese Antoine
        GestAudio.instance.ChangerEtatLecturePiste(TypePiste.MusiqueBase, false);
        GestAudio.instance.ChangerEtatLecturePiste(TypePiste.MusiqueEvenA, false);
        GestAudio.instance.ChangerEtatLecturePiste(TypePiste.MusiqueEvenB, false);
        GestAudio.instance.ChangerEtatLecturePiste(TypePiste.MusiqueMenu, true);
    }

    /// <summary>
    /// #Tp3 Antoine
    /// Met à jour les informations affichées dans la boutique.
    /// </summary>
    void MettreAJourInfos()
    {
        _champArgent.text = _donneesPerso.argent + " $"; // Met à jour le champ d'argent.
        _champNiveau.text = "Niveau " + _donneesPerso.niveau; // Met à jour le champ de niveau.
    }

    void OnApplicationQuit()
    {
        _estEnPlay = false; // Indique que l'application n'est plus en cours d'exécution.
        _donneesPerso.Initialiser(); // Réinitialise les données du personnage.
        Debug.Log("Quit"); // Affiche un message de débogage.
    }

    void OnDestroy()
    {
        _donneesPerso.evenementMiseAJour.RemoveAllListeners(); // Supprime tous les écouteurs d'événements de mise à jour.
        if (_estEnPlay) _donneesPerso.niveau++; // Incrémente le niveau du personnage si l'application est en cours d'exécution.
        Debug.Log("Destroy"); // Affiche un message de débogage.
    }
}
