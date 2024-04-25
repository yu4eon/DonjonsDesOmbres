using UnityEngine;

// Classe représentant une piste musicale dans le jeu.
public class PisteMusicale : MonoBehaviour
{
    // Type de la piste musicale.
    [SerializeField] TypePiste _type; 
    public TypePiste type => _type;

    // Indique si la piste est active par défaut.
    [SerializeField] bool _estActifParDefaut; 

    // Indique si la piste est actuellement active.
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

    // Composant AudioSource associé à la piste musicale.
    AudioSource _source;
    public AudioSource source => _source;

 
    void Awake() 
    {
        // Ajoute le composant AudioSource au GameObject.
        _source = gameObject.AddComponent<AudioSource>();
        // Initialise l'état actif à partir de la valeur par défaut.
        _estActif = _estActifParDefaut;
        // Définit la piste comme en boucle.
        _source.loop = true;
        // Démarre la lecture de la piste dès le démarrage.
        _source.playOnAwake = true;
    }


    void Start() 
    {
        // Ajuste le volume au démarrage.
        AjusterVolume();
    }

    // Fonction permettant d'ajuster le volume de la piste musicale en fonction de son état actif.
    public void AjusterVolume() 
    {
        // Si la piste est active, ajuste le volume en fonction du volume musical de SoundManager.
        if(estActif) _source.volume = SoundManager.instance.volumeMusicalRef;
        // Sinon, désactive le volume de la piste.
        else _source.volume = 0f;
    }
}
