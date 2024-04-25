using UnityEngine;                                               //Cours 11, 2024

public class PisteMusicale : MonoBehaviour
{
    [SerializeField] TypePiste _type; 
    public TypePiste type => _type;
    [SerializeField] bool _estActifParDefaut; 
    [SerializeField] bool _estActif; 
    public bool estActif
    {
        get => _estActif;
        set
        {
            _estActif = value;
            AjusterVolume();
        }
    }

    AudioSource _source;
    public AudioSource source => _source;

    void Awake() 
    {
        _source = gameObject.AddComponent<AudioSource>();
        _estActif = _estActifParDefaut;
        _source.loop = true;
        _source.playOnAwake = true;
    }

    void Start() 
    {
        AjusterVolume();
    }

    public void AjusterVolume() 
    {
        if(estActif) _source.volume = SoundManager.instance.volumeMusicalRef;
        else _source.volume = 0f;
    }
}