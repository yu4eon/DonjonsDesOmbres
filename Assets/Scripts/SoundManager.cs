using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] float _volumeMusicalRef = 1f;
    [SerializeField] Vector2 _pitchSonMinMax = new Vector2(0.9f, 1.1f);
    static AudioSource _sourceEffetsSonores;

    static SoundManager _instance;
    public static SoundManager Instance => _instance;

    void Awake()
    {
        if (!BecomeSingletonInstance()) return;
        DontDestroyOnLoad(gameObject);
        _sourceEffetsSonores = gameObject.AddComponent<AudioSource>();
    }

    public static void PlaySound(AudioClip clip)
    {
        if (Instance == null)
        {
            Debug.LogError("AudioManager instance is not initialized!");
            return;
        }
        // _sourceEffetsSonores.pitch = pitch;
        _sourceEffetsSonores.PlayOneShot(clip, Instance._volumeMusicalRef);
    }

    public static void ChangeGeneralVolume(float volume)
    {
        if (Instance == null)
        {
            Debug.LogError("AudioManager instance is not initialized!");
            return;
        }
        Instance._volumeMusicalRef = volume;
    }

    bool BecomeSingletonInstance()
    {
        if (_instance != null)
        {
            Debug.LogError("An instance of AudioManager already exists in the scene");
            Destroy(gameObject);
            return false;
        }
        _instance = this;
        return true; // Success!
    }
}
