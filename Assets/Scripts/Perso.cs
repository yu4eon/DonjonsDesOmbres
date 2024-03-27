using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Classe qui contrôle les déplacements du personnage.
/// </summary>
public class Perso : DetecteurSol
{
    [SerializeField] float _vitesse = 10f; // Vitesse à laquelle le personnage se déplace.
    float _vitesseInitial;
    [SerializeField] float _forceSaut = 120f; // L'amplitude du saut.
    [SerializeField] int _nbFramesMax = 10; // Nombre de frames maximum pendant lesquelles le joueur peut sauter.
    [SerializeField] static bool _possedeDoublesSauts = false; // Si le personnage possède le pouvoir de double saut.
    [SerializeField] SOPerso _donnees;
    [SerializeField] ParticleSystem _particuleCourse;
    [SerializeField] ParticleSystem.MinMaxCurve _startSizeRapide;
    // vv Apparament, tu ne peux pas acceder au module renderer a partir du particle system vv
    [SerializeField] ParticleSystemRenderer _renderModule; 
    ParticleSystem.MinMaxCurve _startSizeInitial;
    ParticleSystem.MainModule _mainModule;
    static public bool possedeDoublesSauts
    {
        set
        {
            _possedeDoublesSauts = true;
        }
    }

    float _axeHorizontal; // Axe horizontal du personnage.
    int _nbFramesRestants = 0; // Nombre de frames restantes pendant lesquelles le joueur peut sauter.
    bool _veutSauter; // Si le joueur veut sauter.
    bool _peutDoubleSauter = false; // Si le joueur peut faire un double saut.
    bool _auDeuxiemeSaut; // Si le joueur est au deuxième saut.
    bool _estRapide;


    Rigidbody2D _rb; // Rigidbody du personnage.
    SpriteRenderer _sr; // SpriteRenderer du personnage.
    Animator _animator; 

    /// <summary>
    /// Méthode qui est appelée lorsque le script est chargé.
    /// </summary>
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>(); // Obtient le Rigidbody du personnage.
        _sr = GetComponent<SpriteRenderer>(); // Obtient le SpriteRenderer du personnage.
        _animator = GetComponent<Animator>();
        _vitesseInitial = _vitesse;
        _mainModule = _particuleCourse.main;
        _startSizeInitial = _mainModule.startSize;
    }

    /// <summary>
    /// Méthode qui est appelée à chaque frame à un rythme fixe.
    /// </summary>
    override protected void FixedUpdate()
    {
        base.FixedUpdate(); // Appelle la méthode FixedUpdate de la classe mère.

        // #tp3 Leon Ajout d'un check sur les frames, pour si le joueur colisionne avec un plafond.
        if (_rb.velocity.y < 0 && _nbFramesRestants == 0) // Si le joueur est en train de tomber. 
        {
            _rb.gravityScale = 3f;
        }
        else
        {
            _rb.gravityScale = 1.5f;
        }

        _rb.velocity = new Vector2(_axeHorizontal * _vitesse, _rb.velocity.y); // Déplace le joueur en fonction de l'entrée horizontale.

        _animator.SetFloat("VelocityX", _rb.velocity.x);

        // Debug.Log(_particuleCourse.isEmitting);
        // if(_rb.velocity.x ==0)
        // {
        //     _particuleCourse.Stop();
        // }
        // _particuleCourse.Play();

        if (_veutSauter) // Si le joueur veut sauter.
        {
            if (_peutDoubleSauter && _auDeuxiemeSaut) // Si le joueur peut faire un double saut.
            {
                _rb.velocity = new Vector2(_rb.velocity.x, 0); // Annule la vitesse verticale du joueur.
                _nbFramesRestants = _nbFramesMax; // Réinitialise le nombre de frames restantes.
                _peutDoubleSauter = false; // Déclare que le joueur a utilisé son double saut.
            }
            Sauter(); // Appelle la méthode pour faire sauter le joueur.
        }
        else if (_estAuSol) // Si le joueur est au sol.
        {
            _auDeuxiemeSaut = false; // Réinitialise l'indicateur de deuxième saut.
            _nbFramesRestants = _nbFramesMax; // Réinitialise le nombre de frames restantes pour sauter.
            if (_possedeDoublesSauts) _peutDoubleSauter = true; // Si le joueur ne possède pas le pouvoir de double saut, arrête la méthode ici.
            // _peutDoubleSauter = true; // Autorise le double saut.
        }
        else // Si le joueur n'est pas au sol et ne maintient pas le bouton de saut.
        {
            _auDeuxiemeSaut = true; // Indique que le joueur est au deuxième saut.
            _nbFramesRestants = 0; // Réinitialise le nombre de frames restantes.
            // _particuleCourse.Stop();
        }

        if (_rb.velocity.x == 0 || !_estAuSol)
        {
            _particuleCourse.Stop();
        }
        else if (_rb.velocity.x != 0 && _estAuSol)
        {
            if(!_particuleCourse.isEmitting)
            {
            _particuleCourse.Play();
            }
        }

    }

    /// <summary>
    /// Méthode qui est appelée lorsque le joueur appuie sur les touches de déplacement.
    /// </summary>
    /// <param name="value">La valeur retournée par le Input system.</param>
    void OnMove(InputValue value)
    {
        _axeHorizontal = value.Get<Vector2>().x; // Obtient la valeur de l'axe horizontal de l'entrée.
        Vector3 scaleParticule = _particuleCourse.transform.localScale;
        
        if (_axeHorizontal < 0) // Si le joueur se déplace vers la gauche.
        {
            _sr.flipX = true; // Tourne le personnage vers la gauche.

            _renderModule.flip = new Vector3(1,0);
            _renderModule.pivot = new Vector3(1,0);
        }
        else if (_axeHorizontal > 0) // Si le joueur se déplace vers la droite.
        {
            _sr.flipX = false; // Tourne le personnage vers la droite.
            _renderModule.flip = new Vector3(0,0);
            _renderModule.pivot = new Vector3(0,0);
        }
    }

    /// <summary>
    /// Méthode qui est appelée lorsque le joueur appuie sur le bouton de saut.
    /// </summary>
    /// <param name="value">La valeur retournée par le Input system.</param>
    void OnJump(InputValue value)
    {
        _veutSauter = value.isPressed; // Active ou désactive le saut en fonction de si le bouton est pressé ou non.
    }

    /// <summary>
    /// Méthode qui permet au joueur de sauter.
    /// </summary>
    void Sauter()
    {
        float fractionForce = (float)_nbFramesRestants / _nbFramesMax; // Calcule la fraction de la force de saut restante.
        Vector2 vecteurForce = Vector2.up * _forceSaut * fractionForce; // Calcule le vecteur de force de saut en fonction de la fraction restante.
        _rb.AddForce(vecteurForce); // Applique la force de saut au Rigidbody du personnage.

        if (_nbFramesRestants > 0) // S'il reste des frames pendant lesquelles le joueur peut sauter.
        {
            _nbFramesRestants--; // Décrémente le nombre de frames restantes.
        }
        else if (_auDeuxiemeSaut && _nbFramesRestants == 0) // Si le joueur est au deuxième saut et qu'il ne peut plus sauter.
        {
            _peutDoubleSauter = false; // Déclare que le joueur ne peut plus faire de double saut.
        }
    }

    public void AugmenterVitesse()
    {
        if(_estRapide)
        {
            StopAllCoroutines();
            _vitesse = _vitesseInitial;
        }
        _vitesse = _vitesse*1.5f;
        _mainModule.startSize = _startSizeRapide;
        _estRapide = true;
        Coroutine coroutineVitesse = StartCoroutine(ChangerVitesse());
    }

    IEnumerator ChangerVitesse()
    {
        yield return new WaitForSeconds(2);
        _vitesse = _vitesse/1.2f;
        yield return new WaitForSeconds(2);
        _vitesse = _vitesseInitial;
        _mainModule.startSize = _startSizeInitial;
        _estRapide = false;
        
    }
    
    /// <summary>
    /// Callback sent to all game objects before the application is quit.
    /// </summary>
    void OnApplicationQuit()
    {
        _donnees.Initialiser();
    }
}
