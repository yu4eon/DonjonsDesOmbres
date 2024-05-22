using System;
using System.Collections;
using System.Linq.Expressions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Auteur du code : Léon Yu, Antoine Lachance
/// Commetaires ajoutés par : Léon Yu, Antoine Lachance
/// Classe qui contrôle les déplacements du personnage.
/// </summary>
public class Perso : DetecteurSol
{
    [SerializeField] AudioClip[] _sonPerso; // Référence au son du saut
    [SerializeField] AudioClip[] _sonElements;
    [SerializeField] float _vitesse = 10f; // Vitesse à laquelle le personnage se déplace.
    float _vitesseInitial;
    [SerializeField] float _forceSaut = 120f; // L'amplitude du saut.
    [SerializeField] int _nbFramesMax = 10; // Nombre de frames maximum pendant lesquelles le joueur peut sauter.
    [SerializeField] static bool _possedeDoublesSauts = false; // Si le personnage possède le pouvoir de double saut.
    static public bool possedeDoublesSauts // Propriété pour accéder à la variable privée _possedeDoublesSauts. 
    {
        set
        {
            _possedeDoublesSauts = value;
        }
    }
    [SerializeField] SOPerso _donnees; // Référence aux données du personnage #synthese Leon
    [SerializeField] ParticleSystem _particuleCourse; // Particule de course lorsque le joueur bouge #tp3 Leon
    [SerializeField] ParticleSystem.MinMaxCurve _startSizeRapide; // Taille des particules lorsque le joueur est rapide #tp3 Leon
    // vv Apparament, tu ne peux pas acceder au module renderer a partir du particle system vv
    [SerializeField] ParticleSystemRenderer _renderModule; // Module de rendu des particules pour la particule de course #tp3 Leon
    [SerializeField] ParticleSystem[] _particulesPouvoirs; // Particules des pouvoirs #tp3 Leon
    [SerializeField] Retroaction _modeleRetro; // Modèle de rétroaction #synthese Leon
    [SerializeField] SONavigation _donneesNavigation; // Référence à la navigation du personnage #tp4 Leon
    UIJeu _uiJeu; // Référence à l'UI du jeu #synthese Leon
    ParticleSystem _particulePouvoirActuelle; // Particule du pouvoir actuel #tp3 Leon
    ParticleSystem.MinMaxCurve _startSizeInitial; // Taille des particules initiale #tp3 Leon
    ParticleSystem.MainModule _mainModule; // Module principal de la particule de course #tp3 Leon

    float _axeHorizontal; // Axe horizontal du personnage.
    int _nbFramesRestants = 0; // Nombre de frames restantes pendant lesquelles le joueur peut sauter.
    bool _veutSauter; // Si le joueur veut sauter.
    bool _peutDoubleSauter = false; // Si le joueur peut faire un double saut.
    bool _auDeuxiemeSaut; // Si le joueur est au deuxième saut.
    bool _estRapide; // Si le joueur est rapide #tp3 Leon
    bool _estInvincible; // Si le joueur est invincible #synthese Leon

    bool _peutDash = true; // Si le joueur peut faire un dash #synthese Antoine
    bool _estEntrainDeDasher;
    [SerializeField] float _dashForce = 24; // Force du dash #synthese Antoine
    int _direction = 1;
    float _tDash = 0.2f;
    float _dashDelai;
    bool _veutDasher = false;
    TrailRenderer _tr;
    // bool _estInvincible = false;

    [Header("Attaque")]
    TypePouvoir _pouvoirActuel; // Pouvoir actuel du joueur #synthese Leon
    bool _peutAttaquer = true; // Si le joueur peut attaquer #synthese Leon
    bool _estEnAttaqueLourd; // Si le joueur est en train d'attaquer #synthese Leon
    [SerializeField] float _delaiAttaque = 0.1f; // Delai avant d'instancier l'arme #synthese Leon
    ArmePerso _arme; // Référence à l'arme du personnage #synthese Leon

    int _LayerInvincibilite;
    int _LayerDefault;
    Coroutine _coroutineVitesse = null;

    Rigidbody2D _rb; // Rigidbody du personnage.
    SpriteRenderer _sr; // SpriteRenderer du personnage.
    Animator _animator; // Animator du personnage. #tp3 Leon
    PlayerInput _playerInput; // Input du joueur.

    /// <summary>
    /// Méthode qui est appelée lorsque le script est chargé.
    /// </summary>
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>(); // Obtient le Rigidbody du personnage.
        _sr = GetComponent<SpriteRenderer>(); // Obtient le SpriteRenderer du personnage.
        _tr = GetComponent<TrailRenderer>();
        _playerInput = GetComponent<PlayerInput>(); // Obtient l'Input du joueur.
        //#tp3 Leon
        _animator = GetComponent<Animator>(); // Obtient l'Animator du personnage. 
        _vitesseInitial = _vitesse; // Sauvegarde la vitesse initiale du personnage.
        _mainModule = _particuleCourse.main; // Obtient le module principal de la particule de course.
        _startSizeInitial = _mainModule.startSize; // Sauvegarde la taille initiale des particules.

        _LayerInvincibilite = LayerMask.NameToLayer("JoueurInvincible");
        _LayerDefault = LayerMask.NameToLayer("Joueur");
    }

    void Start()
    {
        _arme = GetComponentInChildren<ArmePerso>(); // Obtient l'arme du personnage
        _donnees.InitialiserVie(); // Initialise les données du personnage
        UIJeu.instance.MettreAJourInfo(); // Initialise les points de vie dans l'UI #synthese Leon
        Coroutine coroutine = StartCoroutine(CoroutineAjusterInvincibilite(2f));
        _donnees.AfficherInventaire(); // Affiche l'inventaire du joueur #synthese Leon
        // Debug.Log("Vie du personnage : " + _donnees.pv);
        // _arme.gameObject.SetActive(false); // Désactive l'arme du personnage
    }

    /// <summary>
    /// Méthode qui est appelée à chaque frame à un rythme fixe.
    /// </summary>
    override protected void FixedUpdate()
    {
        if (_estEntrainDeDasher)
        {
            return;
        }
        base.FixedUpdate(); // Appelle la méthode FixedUpdate de la classe mère.

        // #tp3 Leon Ajout d'un check sur les frames, pour si le joueur colisionne avec un plafond.
        if (_rb.velocity.y < 0 && _nbFramesRestants == 0) // Si le joueur est en train de tomber. 
        {
            _rb.gravityScale = 6.5f;
        }
        else
        {
            _rb.gravityScale = 5f;
        }

        _rb.velocity = new Vector2(_axeHorizontal * _vitesse, _rb.velocity.y); // Déplace le joueur en fonction de l'entrée horizontale.
        if (_estEnAttaqueLourd)
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
        }
        _animator.SetFloat("VelocityY", _rb.velocity.y); // Donne la vitesse verticale au paramètre de l'Animator. #tp4 Leon
        _animator.SetFloat("VelocityX", _rb.velocity.x); // Donne la vitesse horizontale au paramètre de l'Animator. #tp3 Leon


        // Si le joueur est en train de tomber ou sauter. J'ai du le mettre dans une autre condition car ça
        // ne fonctionnait pas si le joueur maintenait la touche de saut. #tp4 Leon
        if (_rb.velocity.y > 0.1 || _rb.velocity.y < -0.1)
        {
            _animator.SetBool("onGround", false); // Donne la valeur true au paramètre de l'Animator. 
        }
        else
        {
            _animator.SetBool("onGround", true); // Donne la valeur true au paramètre de l'Animator.
        }

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

        // #tp3 Leon
        // Si le joueur ne bouge pas ou n'est pas au sol, arrête les particules de course.
        if (_rb.velocity.x == 0 || !_estAuSol)
        {
            _particuleCourse.Stop();
        }
        // Si le joueur bouge et est au sol, commence les particules de course.
        else if (_rb.velocity.x != 0 && _estAuSol)
        {
            if (!_particuleCourse.isEmitting)
            {
                _particuleCourse.Play();
            }
        }

        if (_veutDasher)
        {
            StartCoroutine(Dash());
            // Debug.Log("Nooonnnnnn");
        }

    }

    /// <summary>
    /// Méthode temporaire à enlever pour la remise
    /// Permet de skipper le niveau, pour tester
    /// Pour activer, pèse sur Tab. Marche seulement lorsque le joueur est instancié dans la scène
    /// </summary>
    
    /// Mise en commentaire pour la remise
    // void OnSkipNiveau()
    // {
    //     _donneesNavigation.AllerSceneSuivante();
    // }

    /// <summary>
    /// Méthode qui est appelée lorsque le joueur appuie sur les touches de déplacement.
    /// </summary>
    /// <param name="value">La valeur retournée par le Input system.</param>
    void OnMove(InputValue value)
    {
        if (_estEnAttaqueLourd)
        {
            _axeHorizontal = 0;
            return;
        }
        _axeHorizontal = value.Get<Vector2>().x; // Obtient la valeur de l'axe horizontal de l'entrée.

        if (_axeHorizontal < 0) // Si le joueur se déplace vers la gauche.
        {
            _arme.ChangerDirection(true); // Change la direction de l'arme du personnage.
            _sr.flipX = true; // Tourne le personnage vers la gauche.
            _direction = -1;

            // #tp3 Leon
            // Tourne les particules de course vers la gauche.
            _renderModule.flip = new Vector3(1, 0);
            _renderModule.pivot = new Vector3(1, 0);
        }
        else if (_axeHorizontal > 0) // Si le joueur se déplace vers la droite.
        {
            _arme.ChangerDirection(false); // Change la direction de l'arme du personnage.
            _sr.flipX = false; // Tourne le personnage vers la droite.
            _direction = 1;
            // #tp3 Leon
            // Tourne les particules de course vers la droite.
            _renderModule.flip = new Vector3(0, 0);
            _renderModule.pivot = new Vector3(0, 0);
        }
    }

    void OnDash()
    {
        if (_peutDash == true)
        {
            _veutDasher = true;
        }
    }
    private IEnumerator Dash()
    {
        if (_peutDash)
        {
            _animator.SetTrigger("Dash");
            float graviteBase = _rb.gravityScale;
            _veutDasher = false;
            _peutDash = false;
            _estEntrainDeDasher = true;
            _tr.emitting = true;
            _rb.gravityScale = 0;
            _rb.velocity = new Vector2(_direction * _dashForce, 0f);
            // _estInvincible = true;
            Coroutine coroutine = StartCoroutine(CoroutineAjusterInvincibiliteDash());
            GestAudio.instance.JouerEffetSonore(_sonPerso[4]);
            yield return new WaitForSeconds(_tDash);
            _rb.gravityScale = graviteBase;
            // _estInvincible = false;
            _estEntrainDeDasher = false;
            _tr.emitting = false;
            _rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(2f);
            _peutDash = true;
        }
    }

    // Note pour les 4 fonctions suivantes : on n'a pas arrivé à faire que le input system envoie in int spécifique,
    // donc on a du faire une fonction pour chaque pouvoir. #tp3
    // Index de chaque pouvoir : 0 = Poison, 1 = Ombre, 2 = Foudre, 3 = Glace

    /// <summary>
    /// Méthode qui est appelée lorsque le joueur appuie sur le 3 ou dpad bas pour
    /// /// changer l'élement de pouvoir actuel en poison.
    /// </summary>
    void OnChangePoison()
    {
        InstantierParticules(0);
        // GestAudio.instance.JouerEffetSonore(_sonElements[0]);
    }

    /// <summary>
    /// Méthode qui est appelée lorsque le joueur appuie sur le 2 ou dpad droite pour
    /// changer l'élement de pouvoir actuel en ombre.
    /// </summary>
    void OnChangeOmbre()
    {
        InstantierParticules(1);
        // GestAudio.instance.JouerEffetSonore(_sonElements[1]);
    }

    /// <summary>
    /// Méthode qui est appelée lorsque le joueur appuie sur le 4 ou dpad gauche pour
    /// changer l'élement de pouvoir actuel en foudre.
    /// </summary>
    void OnChangeFoudre()
    {
        InstantierParticules(2);
        // GestAudio.instance.JouerEffetSonore(_sonElements[2]);
    }
    /// <summary>
    /// Méthode qui est appelée lorsque le joueur appuie sur le 1 ou dpad haut pour
    /// changer l'élement de pouvoir actuel en glace.
    /// </summary>
    void OnChangeGlace()
    {
        InstantierParticules(3);
        // GestAudio.instance.JouerEffetSonore(_sonElements[3]);
    }


    public void InstantierParticules(int index)
    {
        Vector3 tailleParticules = new Vector3(2, 2, 2);
        if (_donnees.pouvoirs.Contains((TypePouvoir)index))
        {
            if (_particulePouvoirActuelle != null) // Si le joueur a déjà un pouvoir actif.
            {
                Destroy(_particulePouvoirActuelle);
                _particulePouvoirActuelle = null;
                GestAudio.instance.JouerEffetSonore(_sonElements[index]);
            }
            _pouvoirActuel = (TypePouvoir)index; // Change le pouvoir actuel en celle du pouvoir index.
            _particulePouvoirActuelle = Instantiate(_particulesPouvoirs[index], transform.position, _particulesPouvoirs[index].transform.rotation, transform);
            _particulePouvoirActuelle.transform.localScale = tailleParticules; // Change la taille des particules pour qu'elles soit plus visible.
            _uiJeu.ActiverPouvoir(index); // Active les particules de pouvoir dans l'UI. #synthese Leon
        }
        else
        {
            Debug.LogWarning("Tu ne possède pas le pouvoir de " + ((TypePouvoir)index));
        }
    }

    /// <summary>
    /// Méthode qui est appelée lorsque le joueur appuie sur le bouton de saut.
    /// </summary>
    /// <param name="value">La valeur retournée par le Input system.</param>
    void OnJump(InputValue value)
    {
        if (_estEnAttaqueLourd)
        {
            return;
        }
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

    /// <summary>
    /// #tp3 Leon
    /// Méthode qui augmente la vitesse du personnage, appelé par effector de vitesse.
    /// </summary>
    public void AugmenterVitesse()
    {
        // Si le joueur est déjà rapide, on ne donne pas le boost.
        // Changé pour un return, puisque StopAllCoroutines() provoquait des erreurs. #synthese Leon
        if (_estRapide)
        {
            // return;
            // StopAllCoroutines();
            StopCoroutine(_coroutineVitesse);
            _vitesse = _vitesseInitial;
        }
        _vitesse = _vitesse * 1.5f;
        _mainModule.startSize = _startSizeRapide;
        _estRapide = true;
        _coroutineVitesse = StartCoroutine(ChangerVitesse());
    }

    /// <summary>
    /// #tp3 Leon
    /// Coroutine qui change la vitesse du personnage et la taille des particules après un certain temps,
    /// jusqu'à ce qu'il revienne à sa vitesse et taille initiale.
    /// </summary>
    IEnumerator ChangerVitesse()
    {
        yield return new WaitForSeconds(2);
        _vitesse = _vitesse / 1.2f;
        yield return new WaitForSeconds(2);
        _vitesse = _vitesseInitial;
        _mainModule.startSize = _startSizeInitial;
        _estRapide = false;

    }

    /// <summary>
    /// Sera appelé pour initialiser le pouvoir du personnage par Niveau
    /// </summary>
    /// <param name="pouvoir">Pouvoir donnee</param>
    public void Initialiser(TypePouvoir pouvoir, UIJeu uiJeu)
    {
        _uiJeu = uiJeu;
        // Debug.Log("Pouvoir initialisé : " + pouvoir);
        _pouvoirActuel = pouvoir;
        _donnees.AjouterPouvoir(pouvoir); // Ajoute le pouvoir au personnage
        // _arme.GetComponentInChildren<ArmePerso>(); // Initialise l'arme du personnage
        // Debug.Log("Pouvoir actuel : " + _pouvoirActuel);
    }

    void OnLightAttack()
    {
        if (_peutAttaquer)
        {
            _peutAttaquer = false;
            // Debug.Log("Attaque légère");
            _animator.SetTrigger("AttaqueLight");
            Coroutine coroutine = StartCoroutine(CoroutineAttaquer(true));
            CoroutineAttaquer(true);
        }
        else
        {
            Debug.LogWarning("Tu ne peux pas attaquer pour l'instant");
        }
    }

    void OnHeavyAttack()
    {
        if (_peutAttaquer)
        {
            _peutAttaquer = false;
            // Debug.Log("Attaque lourde");
            _animator.SetTrigger("AttaqueHeavy");
            Coroutine coroutine = StartCoroutine(CoroutineAttaquer(false));
        }
        else
        {
            Debug.LogWarning("Tu ne peux pas attaquer pour l'instant");
        }
    }

    IEnumerator CoroutineAttaquer(bool estLeger)
    {
        // Debug.Log(_pouvoirActuel);
        // Debug.Log(_arme);
        yield return new WaitForSeconds(_delaiAttaque);
        _arme.gameObject.SetActive(true);
        _estEnAttaqueLourd = !estLeger;
        _arme.InitialiserArme(_pouvoirActuel, estLeger); // Initialise l'arme du personnage
    }

    public void PermettreAttaque()
    {
        _estEnAttaqueLourd = false;
        _peutAttaquer = true;
    }
    public void TerminerAttaque()
    {
        _estEnAttaqueLourd = false;
    }

    void OnApplicationQuit()
    {
        _donnees.Initialiser(); // Initialise les données du personnage
    }
    void OnDestroy()
    {
        _donnees.evenementMiseAJour.RemoveAllListeners(); // Supprime tous les écouteurs d'événements de mise à jour des données du personnage
        _donnees.ViderInventaire();
    }

    public void JouerSon(int index)
    {
        GestAudio.instance.JouerEffetSonore(_sonPerso[index]); // Joue le son correspondant à l'index passé en paramè
    }

    /// <summary>
    /// Méthode qui permet de faire des dégats au personnage.
    /// </summary>
    /// <param name="degats">Nombre de dégat envoyé au personnage</param>
    public void SubirDegats(int degats)
    {
        int degatsFinaux = Mathf.Clamp((degats - (degats * _donnees.defense / 100)), 1, int.MaxValue);
        Retroaction retro = Instantiate(_modeleRetro, transform.position, Quaternion.identity, transform.parent);
        retro.ChangerTexte("-" + degatsFinaux, "#FF3535");
        _donnees.pv -= degatsFinaux;
        UIJeu.instance.MettreAJourInfo();
        Coroutine coroutine = StartCoroutine(CoroutineAjusterInvincibilite());
        Debug.Log("Points de vie restants : " + _donnees.pv);
        JouerSon(6);
        if (_donnees.pv <= 0)
        {
            // Debug.Log("Le joueur est mort");
            Mourir();
        }
        if (_donnees.pv <= _donnees.pvIni / 4)
        {
            GestAudio.instance.ChangerEtatLecturePiste(TypePiste.MusiqueEvenA, true);
        }
    }

    /// <summary>
    /// Coroutine qui permet de mettre le joueur en invincibilité pendant le dash.
    /// </summary>
    IEnumerator CoroutineAjusterInvincibiliteDash(float duree = 0.5f)
    {
        gameObject.layer = _LayerInvincibilite;
        _estInvincible = true;
        yield return new WaitForSeconds(duree);
        _estInvincible = false;
        gameObject.layer = _LayerDefault;

    }
    IEnumerator CoroutineAjusterInvincibilite(float duree = 1f)
    {
        gameObject.layer = _LayerInvincibilite;
        _estInvincible = true;
        Coroutine coroutine = StartCoroutine(CoroutineChangerCouleur());
        yield return new WaitForSeconds(duree);
        _estInvincible = false;
        _sr.enabled = true;
        gameObject.layer = _LayerDefault;

    }

    IEnumerator CoroutineChangerCouleur()
    {
        while (_estInvincible)
        {
            yield return new WaitForSeconds(0.1f);
            _sr.enabled = !_sr.enabled;
        }
        _sr.enabled = true;
    }

    /// <summary>
    /// Méthode qui permet de faire mourir le joueur.
    /// </summary>
    void Mourir()
    {
        GestAudio.instance.JouerEffetSonore(_sonPerso[5]);
        _donneesNavigation.AllerSceneTableauHonneur();
    }

    public void DesactiverInputs()
    {
        _playerInput.actions.Disable();
    }
}
