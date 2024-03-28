using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autels : MonoBehaviour
{
    // [SerializeField] string element;
    [SerializeField] TypePouvoir element;
    [SerializeField] SOPerso _donneesPerso;
    [SerializeField] SpriteRenderer _spriteEteint;
    SpriteRenderer sr;
    ParticleSystem ps;
    bool estActif = false;
    Sprite _sBase;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        ps = GetComponentInChildren<ParticleSystem>();

        estActif = false;
        ps.Stop();

        _sBase = sr.sprite;
        sr.sprite = _spriteEteint.sprite;
    }
    
    void Start()
    {
        Activateur.instance.evenementActivateur.AddListener(Activer);
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && ps.isPlaying)
        {
            Debug.Log("Element : " + element);
            if (Input.GetKeyDown(KeyCode.E))
            {
                _donneesPerso.AjouterPouvoir(element);
                sr.sprite = _spriteEteint.sprite;
                ps.Stop();
            }
        }
    }

    void Activer()
    {
        estActif = true;
        sr.sprite = _sBase;
        ps.Play();
    }
}
