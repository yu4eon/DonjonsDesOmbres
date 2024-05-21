/// <summary>
/// Synthese Antoine
/// </summary>

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GestAudio : MonoBehaviour
{
    [SerializeField] float _volumeMusicalRef = 0.8f; // Référence de volume pour la musique.
    [SerializeField] Vector2 _pitchSonMinMax = new Vector2(0.9f, 1.1f); // Plage de hauteur de ton des sons.

    // Propriété pour accéder à la référence de volume musical.
    public float volumeMusicalRef => _volumeMusicalRef;

    PisteMusicale[] _tPistes; // Tableau de pistes musicales.
    public PisteMusicale[] tPistes => _tPistes; // Propriété pour accéder aux pistes musicales.

    AudioSource _sourceEffetsSonores; // AudioSource pour les effets sonores.

    static GestAudio _instance; // Instance unique du SoundManager.
    public static GestAudio instance => _instance; // Propriété pour accéder à l'instance unique.

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Assure qu'il n'y a qu'une seule instance de SoundManager dans la scène.
        if (DevenirInstanceSingleton() == false) return;

        // Ne détruit pas l'objet lors du chargement d'une nouvelle scène.
        DontDestroyOnLoad(gameObject);

        // Récupère les pistes musicales enfants.
        _tPistes = GetComponentsInChildren<PisteMusicale>();

        // Ajoute un composant AudioSource pour les effets sonores.
        _sourceEffetsSonores = gameObject.AddComponent<AudioSource>();
    }

    /// <summary>
    /// Fonction permetant de changer le volume général de toutes les pistes musicales.
    /// </summary>
    /// <param name="volume">Le nouveau volume général.</param>
    public void ChangerVolumeGeneral(float volume)
    {
        _volumeMusicalRef = volume;
        foreach(PisteMusicale piste in _tPistes) piste.AjusterVolume();
    }

    /// <summary>
    /// Fonction qui permet de changer la hauteur de ton de toutes les pistes musicales.
    /// </summary>
    /// <param name="pitch">Le nouveau pitch des pistes musicales.</param>
    public void ChangerPitchMusique(float pitch)
    {
        foreach(PisteMusicale piste in _tPistes)
        {
            piste.source.pitch = pitch;
        }
    }

    /// <summary>
    /// Fonction qui change l'état de lecture d'une piste musicale spécifique.
    /// </summary>
    /// <param name="type">Le type de la piste musicale à modifier.</param>
    /// <param name="estActive">L'état de lecture (actif ou non) de la piste musicale.</param>
    public void ChangerEtatLecturePiste(TypePiste type, bool estActive)
    {
        foreach(PisteMusicale piste in _tPistes)
        {
            if(piste.type == type)
            {
                piste.estActif = estActive;
                if(estActive) StartCoroutine(FadeIn(piste, 2f));
                else StartCoroutine(FadeOut(piste, 2f));
            }
        }
    }

    /// <summary>
    /// Fonction qui joue un effet sonore avec un pitch aléatoire dans la plage spécifiée.
    /// </summary>
    /// <param name="clip">Le clip audio de l'effet sonore à jouer.</param>
    /// <param name="volume">Le volume de l'effet sonore.</param>
    public void JouerEffetSonore(AudioClip clip, float volume = 1f)
    {
        float pitchAleatoire = Random.Range(_pitchSonMinMax.x, _pitchSonMinMax.y);
        _sourceEffetsSonores.pitch = pitchAleatoire;
        _sourceEffetsSonores.PlayOneShot(clip, volume);
    }

    /// <summary>
    /// Assure qu'il n'y a qu'une seule instance de SoundManager dans la scène.
    /// </summary>
    /// <returns>True si l'objet est devenu l'instance unique, false sinon.</returns>
    bool DevenirInstanceSingleton()
    {
        if (_instance != null)
        {
            Debug.LogError("Il y a déjà une instance de SoundManager dans la scène.");
            Destroy(gameObject);
            return false;
        }
        _instance = this;
        return true; // Succès !
    }

    /// <summary>
    /// Coroutine pour effectuer un fade-in(fondu en entrée) sur une piste musicale.
    /// </summary>
    /// <param name="piste">La piste musicale à laquelle appliquer le fondu.</param>
    /// <param name="dureeFade">La durée du fondu en secondes.</param>
    public IEnumerator FadeIn(PisteMusicale piste, float dureeFade)
    {
        float tempsEcoule = 0f;
        float volumeInitial = 0f;

        while(tempsEcoule < dureeFade)
        {
            piste.source.volume = Mathf.Lerp(volumeInitial, piste.volume, tempsEcoule/dureeFade);
            tempsEcoule += Time.deltaTime;
            yield return null;
        }
        piste.source.volume = _volumeMusicalRef;
    }

    /// <summary>
    /// Coroutine pour effectuer un fade-out(fondu en sortie) sur une piste musicale.
    /// </summary>
    /// <param name="piste">La piste musicale à laquelle appliquer le fondu.</param>
    /// <param name="dureeFade">La durée du fondu en secondes.</param>
    public IEnumerator FadeOut(PisteMusicale piste, float dureeFade)
    {
        float tempsEcoule = 0f;
        float volumeInitial = piste.source.volume;

        while(tempsEcoule < dureeFade)
        {
            piste.source.volume = Mathf.Lerp(volumeInitial, 0f, tempsEcoule/dureeFade);
            tempsEcoule += Time.deltaTime;
            yield return null;
        }
        piste.source.volume = 0f;
        piste.estActif = false;
    }
}
