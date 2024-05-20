using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// Classe responsable de la gestion des sons dans le jeu.
public class SoundManager : MonoBehaviour
{
    [SerializeField] float _volumeMusicalRef = 0.8f; // Référence de volume pour la musique.
    [SerializeField] Vector2 _pitchSonMinMax = new Vector2(0.9f, 1.1f); // Plage de hauteur de ton des sons.

    // Propriété pour accéder à la référence de volume musical.
    public float volumeMusicalRef => _volumeMusicalRef;

    PisteMusicale[] _tPistes; // Tableau de pistes musicales.
    public PisteMusicale[] tPistes => _tPistes; // Propriété pour accéder aux pistes musicales.

    AudioSource _sourceEffetsSonores; // AudioSource pour les effets sonores.

    static SoundManager _instance; // Instance unique du SoundManager.
    public static SoundManager instance => _instance; // Propriété pour accéder à l'instance unique.

    // Fonction appelée lors de l'initialisation de l'objet.
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


    // Change le volume général de toutes les pistes musicales.
    public void ChangerVolumeGeneral(float volume)
    {
        _volumeMusicalRef = volume;
        foreach(PisteMusicale piste in _tPistes) piste.AjusterVolume();
    }

    // Change la hauteur de ton de toutes les pistes musicales.
    public void ChangerPitchMusique(float pitch)
    {
        foreach(PisteMusicale piste in _tPistes)
        {
            piste.source.pitch = pitch;
        }
    }

    // Change l'état de lecture d'une piste musicale spécifique.
    public void ChangerEtatLecturePiste(TypePiste type, bool estActive)
    {
        foreach(PisteMusicale piste in _tPistes)
        {
            if(piste.type == type)
            {
                piste.estActif = estActive;
                // StartCoroutine(FadeIn(piste, 2f));
            }
            // else StartCoroutine(FadeOut(piste, 2f));
        }
    }

    // Joue un effet sonore avec un pitch aléatoire dans la plage spécifiée.
    public void JouerEffetSonore(AudioClip clip)
    {
        float pitchAleatoire = Random.Range(_pitchSonMinMax.x, _pitchSonMinMax.y);
        _sourceEffetsSonores.pitch = pitchAleatoire;
        _sourceEffetsSonores.PlayOneShot(clip);
    }

    // Assure qu'il n'y a qu'une seule instance de SoundManager dans la scène.
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


    public IEnumerator FadeIn(PisteMusicale piste, float dureeFade)
    {
        float tempsEcoule = 0f;
        float volumeInitial = 0f;

        while(tempsEcoule < dureeFade)
        {
            piste.volume = Mathf.Lerp(volumeInitial, _volumeMusicalRef, tempsEcoule/dureeFade);
            tempsEcoule += Time.deltaTime;
            yield return null;
        }
        piste.volume = _volumeMusicalRef;
        piste.estActif = true;
    }
    public IEnumerator FadeOut(PisteMusicale piste, float dureeFade)
    {
        float tempsEcoule = 0f;
        float volumeInitial = piste.volume;

        while(tempsEcoule < dureeFade)
        {
            piste.volume = Mathf.Lerp(volumeInitial, 0f, tempsEcoule/dureeFade);
            tempsEcoule += Time.deltaTime;
            yield return null;
        }
        piste.volume = 0f;
        piste.estActif = false;
    }

}
