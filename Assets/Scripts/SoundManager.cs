using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] float _volumeMusicalRef = 1f;
    [SerializeField] Vector2 _pitchSonMinMax = new Vector2(0.9f, 1.1f);
    
    public float volumeMusicalRef => _volumeMusicalRef;
    PisteMusicale[] _tPistes;
    public PisteMusicale[] tPistes => _tPistes;
    AudioSource _sourceEffetsSonores;
    static SoundManager _instance;
    public static SoundManager instance => _instance;

    void Awake()
    {
        if (DevenirInstanceSingleton()==false) return;
        DontDestroyOnLoad(gameObject);
        _tPistes = GetComponentsInChildren<PisteMusicale>();
        _sourceEffetsSonores = gameObject.AddComponent<AudioSource>();

    }
    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangerEtatLecturePiste(TypePiste.MusiqueBase, true);
        }
    }

    public void ChangerVolumeGeneral(float volume)
    {
        _volumeMusicalRef = volume;
        foreach(PisteMusicale piste in _tPistes) piste.AjusterVolume();
    }

    public void ChangerPitchMusique(float pitch)
    {
        foreach(PisteMusicale piste in _tPistes)
        {
            piste.source.pitch = pitch;
        }
    }

    public void ChangerEtatLecturePiste(TypePiste type, bool estActife)
    {
        
        foreach(PisteMusicale piste in _tPistes)
        {
            if(piste.type == type) piste.estActif = estActife;
        }

    }

    public void JouerEffetSonore(AudioClip clip)
    {
        float pitchAlea = Random.Range(_pitchSonMinMax.x, _pitchSonMinMax.y);
        _sourceEffetsSonores.pitch = pitchAlea;
        _sourceEffetsSonores.PlayOneShot(clip);
    }

    bool DevenirInstanceSingleton()
    {
        if (_instance != null)
        {
            Debug.LogError("Il y a déjà une instance de GestAudio dans la scène");
            Destroy(gameObject);
            return false;
        }
        _instance = this;
        return true; //succès!
    }
}
