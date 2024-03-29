using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    #region movement variables
    [Header("Movement Variables")]
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _speedDefault = 5f;
    [SerializeField]
    private float _boost = 1.5f;
    [SerializeField]
    private float _thrusterBoost = 1.25f;
    [SerializeField]
    private Vector3 _movement = new Vector3();
    [SerializeField]
    private bool _poweredDown = false;
    [Space]
    #endregion

    #region Movement Bounds
    [Header("Movement Bounds")]
    [SerializeField]
    private float _upperBound = 0f;
    [SerializeField]
    private float _lowerBound = -3.8f;
    [SerializeField]
    private float _leftBound = -11.3f;
    [SerializeField]
    private float _rightBound = 11.3f;
    #endregion

    #region Sprite Renderer
    [Header("Sprite Reneder")]
    [SerializeField]

    private SpriteRenderer _spriteRenderer, _afterImage;

    [SerializeField]
    private float _afterImageLifeTime, _timeBetweenAfterImages;

    [SerializeField]
    private float _afterImageCounter;
    public Color afterImageColor;
    #endregion

    #region Spawn Points and Ammo
    [Header("Spawn Points and Ammo")]
    [SerializeField]
    private Transform _offset;

    [SerializeField]
    private int _maxAmmo = 20;

    [SerializeField]
    private int _ammo;

    [SerializeField]
    private GameObject _laserPrefab;

    Coroutine _fireRate;

    [SerializeField]
    private float _fireSpeed = .5f;
    #endregion

    #region Hyper Beam
    [Header("Special Meter")]
    [SerializeField]
    private float _specialMeter = 0;
    private float _maxSpecialMeter = 100f;

    [Header("Hyper Beam")]
    [SerializeField]
    private bool _isBeamActive;
    [SerializeField]
    private Transform _hyperBeamSpawnPoint;
    [SerializeField]
    private Vector2 _beamBoxDirections = new Vector2(.5f, .5f);
    [SerializeField]
    private LayerMask _enemyMask;
    [SerializeField]
    private float _beamDistance = 100f, _beamDuration = 5f;
    [SerializeField]
    private BoxCollider2D[] _boxCollider;
    [SerializeField]
    private LineRenderer _lineRenderer;
    Coroutine HyperBeamCoroutine;
    #endregion

    #region Missage Barrage
    [Header("Missile Barrage")]
    [SerializeField]
    GameObject _rocketPrefab;
    [SerializeField]
    private Transform[] _missleFirePoint;
    Coroutine missleBarageCoroutine;
    #endregion

    #region Flags and Prefabs
    [Header("Flags and Prefabs")]
    [SerializeField]
    public bool _tripleShotActive = false;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private bool _speedBoostActive = false;

    [SerializeField]
    public bool _shieldsActive = false;

    [SerializeField]
    private SpriteRenderer _shieldSprite;

    [SerializeField]
    public int _shieldHp = 0;

    [SerializeField]
    private GameObject _shieldVisualizer, _leftThruster, _rightThruster;

    [SerializeField]
    private float _powerUpTime = 5f;
    #endregion

    #region Thrusters
    [Header("Thruster")]
    [SerializeField]
    private bool _boosting = false;
    [SerializeField]
    private float _maxThrusterAmount = 100f;
    [SerializeField]
    private float _thrusterAmount = 50f;
    [SerializeField]
    private float _thrusterDecayRate = 0.2f;
    [SerializeField]
    private float _thrusterRechargeRate = 0.3f;
    [SerializeField]
    private float _canRecharge = -1f;
    [SerializeField]
    private float _thrusterRechargeAmount;
    #endregion

    #region Score and Lives and Audio
    [Header("Score and Lives")]
    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private int _score;

    [SerializeField]
    private UIManager _uiManager;


    [Header("Audio")]
    [SerializeField]
    private AudioClip[] _audioClip;
    private AudioSource _audioSource;
    private SpawnManager _spawnManager;
    [SerializeField, Range(0, 1)]
    private float _timeScale = 1;
    #endregion

    [Header("Debug")]
    [SerializeField]
    private float _detectRadius = 5f;
    [SerializeField]
    private LayerMask _powerUpMask;
    [SerializeField]
    private float _pullSpeed = 1f;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("No Spawn Manager");
        }

        if (_uiManager == null)
        {
            Debug.LogError("No UI Manager");
        }
        else
        {
            _ammo = _maxAmmo;
            _uiManager.UpdateAmmo(_ammo, _maxAmmo);
            _uiManager.UpdateSpecialBar(_specialMeter);
        }

        if (!_shieldSprite)
        {
            Debug.LogError("Shield Sprite Not Found");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager._isGameOver) return;

        Time.timeScale = _timeScale;
        _thrusterAmount = Mathf.Clamp(_thrusterAmount, 0, _maxThrusterAmount);
        _specialMeter = Mathf.Clamp(_specialMeter, 0, 100);

        _afterImageCounter = Mathf.Clamp(_afterImageCounter, 0f, 100f);

        if (_afterImageCounter > 0)
        {
            _afterImageCounter -= Time.deltaTime;
        }
        //Debuging();

        CalculateMovement();

        if (_poweredDown)
        {
            _speed = _speedDefault;
            _tripleShotActive = false;
            _speedBoostActive = false;
            StopCoroutine("PowerUpTime");
            StopCoroutine("SpeedCoolDown");
        }

        //HyperBeam Active
        switch (_isBeamActive)
        {
            case true:
                LaserBeamActive();
                if (_beamDuration < 0)
                    _isBeamActive = false;
                break;
            case false:
                _beamDuration = 5f;
                _lineRenderer.enabled = false;
                break;
        }


        if (Input.GetKey(KeyCode.X) && !_isBeamActive)
        {
            if (_fireRate == null)
            {
                _fireRate = StartCoroutine(FireRate(_fireSpeed));
            }

        }

        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //}

        if (Input.GetKeyDown(KeyCode.C) && _specialMeter == 100 && !_poweredDown)
        {
            _specialMeter = 0;
            _isBeamActive = true;
            _uiManager.UpdateSpecialBar(_specialMeter);

            //LaserBeamDebug();

            #region camera shake
            GameObject.Find("Main Camera").TryGetComponent<CameraBehaviour>(out CameraBehaviour cam);
            cam.ScreenShake(0.3f, _beamDuration);
            #endregion            
        }

        #region laser code unused
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    EnableLaser();
        //}


        //if (Input.GetKey(KeyCode.X))
        //{
        //    UpdateLaser();
        //}

        //if (Input.GetKeyUp(KeyCode.X))
        //{
        //    DisableLaser();
        //}
        #endregion

        Thrusters();

        if(Input.GetKey(KeyCode.V)) DetectionSphere();

        switch (_shieldHp)
        {
            case 3:
                _shieldSprite.color = Color.white;
                break;
            case 2:
                _shieldSprite.color = Color.magenta;
                break;
            case 1:
                _shieldSprite.color = Color.red;
                break;
            default:
                _shieldSprite.color = Color.yellow;
                break;
        }
    }  

    void LaserBeamActive()
    {

        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, _hyperBeamSpawnPoint.position);
        _lineRenderer.SetPosition(1, _hyperBeamSpawnPoint.position + _hyperBeamSpawnPoint.up * _beamDistance);

        RaycastHit2D[] hit =
            Physics2D.BoxCastAll(gameObject.transform.position, _beamBoxDirections,
                                 gameObject.transform.rotation.z, gameObject.transform.up,
                                 _beamDistance, _enemyMask);

        foreach (RaycastHit2D collider in hit)
        {
            Debug.Log(collider.transform.name);
            EnemyBehaviour enemy = collider.transform.GetComponent<EnemyBehaviour>();
            if (enemy != null)
            {
                Debug.Log("got component");
                enemy?.BeamHit();
            }
            else
            {
                Debug.Log("couldn't get component");
            }
        }

        _beamDuration -= Time.deltaTime;
    }

    void DetectionSphere()
    {
        RaycastHit2D[] hit =
            Physics2D.CircleCastAll(gameObject.transform.position, _detectRadius, gameObject.transform.up, _detectRadius, _powerUpMask);
        foreach (RaycastHit2D collider in hit)
        {
            PowerUpBehavior powerUp = collider.transform.GetComponent<PowerUpBehavior>();
            if (powerUp != null)
            {
                Debug.Log("Got PowerUp");
                powerUp.transform.position = Vector2.MoveTowards(powerUp.transform.position, transform.position, _pullSpeed * Time.deltaTime);
            }
        }
    }


    #region missile barrage feature
    IEnumerator MissleBarrage()
    {
        //int[] firePoints = { 1, 2, 3, 2, 1, 3, 2, 3, 1 };
        // int prevIndex = 0;
        // int currIndex;
        // int barrageCount = 10;



        //while(barrageCount > 0)
        //{
        //     currIndex = Random.Range(0, _missleFirePoint.Length);
        //     Debug.Log($"Current Index: {currIndex}");
        //     Debug.Log($"Previous Index: {prevIndex}");

        //     if(currIndex != prevIndex)
        //     {
        //         GameObject missle;
        //         missle = Instantiate(_rocketPrefab, _missleFirePoint[currIndex].position, _missleFirePoint[currIndex].rotation);
        //         Debug.Log($"Hit 'if' statement current index = {currIndex}");
        //         prevIndex = currIndex;
        //         Debug.Log($"PrevIndex if Statement: {prevIndex}");
        //         barrageCount--;
        //         yield return new WaitForSeconds(0.2f);
        //     }           
        //     Debug.Log($"Barrage Count: {barrageCount}");
        //     Debug.Log("Inside loop: " + missleBarageCoroutine);
        //}        
        yield return new WaitForSeconds(0.2f);
        //missleBarageCoroutine = null;
    }
    #endregion

    void CalculateMovement()
    {
        if (_poweredDown)
        {
            _movement.x = -Input.GetAxisRaw("Horizontal");
            _movement.y = -Input.GetAxisRaw("Vertical");
        }
        else
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");
        }

        transform.Translate(_movement * _speed * Time.deltaTime);

        #region bounds
        if (transform.position.y >= _upperBound)
        {
            transform.position = new Vector3(transform.position.x, _upperBound, 0);
        }
        else if (transform.position.y <= _lowerBound)
        {
            transform.position = new Vector3(transform.position.x, _lowerBound, 0);
        }

        if (transform.position.x < _leftBound)
        {
            transform.position = new Vector3(_rightBound - 0.5f, transform.position.y, 0);
        }
        else if (transform.position.x > _rightBound)
        {
            transform.position = new Vector3(_leftBound + 0.5f, transform.position.y, 0);
        }
        #endregion
    }

    void FireLaser()
    {
        if (_offset != null)
        {
            switch (_tripleShotActive)
            {
                case false:
                    if (_ammo > 0)
                    {
                        _ammo--;
                        _uiManager?.UpdateAmmo(_ammo, _maxAmmo);
                        GameObject laser;
                        laser = Instantiate(_laserPrefab, _offset.position, Quaternion.identity);
                        _audioSource.PlayOneShot(_audioClip[0]);
                    }
                    else
                    {
                        //PlaySound
                        _audioSource.PlayOneShot(_audioClip[1]);
                    }
                    break;
                case true:
                    GameObject tripleShot;
                    tripleShot = Instantiate(_tripleShotPrefab, _offset.position, Quaternion.identity);
                    _audioSource.PlayOneShot(_audioClip[0]);
                    break;
            }
        }

    }

    void Thrusters()
    {
        if (_poweredDown) return;


        switch (_boosting)
        {
            case false:
                if ((_thrusterAmount < _maxThrusterAmount) && Time.time > _canRecharge)
                {
                    _canRecharge = Time.time + _thrusterRechargeRate;
                    _thrusterAmount += _thrusterRechargeAmount;
                    _uiManager?.UpdateThrusterBar(_thrusterAmount / _maxThrusterAmount);
                }
                break;

            case true:
                //if (_movement == Vector3.zero) return;
                _thrusterAmount -= _thrusterDecayRate * Time.deltaTime;
                _uiManager?.UpdateThrusterBar(_thrusterAmount / _maxThrusterAmount);
                if (_afterImageCounter <= 0)
                {
                    AfterImageEffect();
                }

                break;
        }

        _thrusterAmount = Mathf.Clamp(_thrusterAmount, 0, _maxThrusterAmount);



        if (!_speedBoostActive)
        {
            #region code block
            if (Input.GetKeyDown(KeyCode.Z))
            {
                _boosting = true;
                _speed *= _thrusterBoost;
            }

            if (Input.GetKeyUp(KeyCode.Z) || _thrusterAmount == 0)
            {
                _boosting = false;
                _speed = _speedDefault;
            }
            #endregion
        }
    }

    void Shields()
    {
        #region shield coode block
        if (_shieldsActive && _shieldHp > 0)
        {
            _shieldHp--;

        }

        if (_shieldHp == 0)
        {
            _shieldsActive = false;
            _shieldVisualizer.SetActive(false);
        }

        if (_shieldHp == -1)
        {
            _shieldVisualizer.SetActive(true);
        }
        #endregion
    }

    void Debuging()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (_lives > 0)
            {
                OnDamage();
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (!_shieldsActive && _shieldHp == 0)
            {
                _shieldHp = 3;
                ActivateShields();
            }
            else
            {
                Shields();
            }
        }
    }

    //method to add 10 to the score
    public void AddScore(int score, float powerAdd = 10f)
    {
        _score += score;
        _specialMeter += powerAdd;
        _uiManager?.UpdateScore(_score);
        _uiManager?.UpdateSpecialBar(_specialMeter/_maxSpecialMeter);
    }
    //communicate with the ui to update the score

    IEnumerator FireRate(float rateOfFire)
    {
        FireLaser();
        yield return new WaitForSeconds(rateOfFire);
        _fireRate = null;
    }

    public void OnDamage(bool healthUp = false)
    {
        if (!healthUp)
        {
            if (_shieldHp > 0)
            {
                Shields();
                return;
            }

            _lives--;
        }

        _uiManager?.UpdateLives(_lives);

        switch (_lives)
        {
            case 3:
                _leftThruster.SetActive(false);
                _rightThruster.SetActive(false);
                break;

            case 2:
                _leftThruster.SetActive(true);
                _rightThruster.SetActive(false);
                break;
            case 1:
                _rightThruster.SetActive(true);
                break;
        }

        if (_lives < 1)
        {
            _spawnManager?.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void RefillAmmo()
    {
        _ammo = _maxAmmo;
        _uiManager?.UpdateAmmo(_ammo, _maxAmmo);
    }

    public void HealthUp()
    {
        if (_lives < 3) _lives++;
        OnDamage(true);
        _uiManager.UpdateLives(_lives);
    }

    public void ActivateTripleShot()
    {
        if (_poweredDown) return;
        _tripleShotActive = true;
        StartCoroutine(PowerUpTime(_powerUpTime));
    }

    public void ActivateSpeedBoost()
    {
        if (_poweredDown) return;
        _speedBoostActive = true;
        _speed *= _boost;
        StartCoroutine(SpeedCoolDown(_powerUpTime));
    }

    public void ActivateShields()
    {
        if (_poweredDown) return;
        _shieldHp = 3;
        _shieldsActive = true;
        _shieldVisualizer.SetActive(true);
    }

    IEnumerator PowerUpTime(float time)
    {
        yield return new WaitForSeconds(time);
        _tripleShotActive = false;
    }

    IEnumerator SpeedCoolDown(float time)
    {
        yield return new WaitForSeconds(time);
        _speedBoostActive = false;
        _speed = _speedDefault;

    }

    public void AfterImageEffect()
    {

        #region code block
        SpriteRenderer image = Instantiate(_afterImage, transform.position, transform.rotation);
        image.sprite = _spriteRenderer.sprite;
        image.color = afterImageColor;
        _afterImageCounter = _timeBetweenAfterImages;
        Destroy(image.gameObject, _afterImageLifeTime);
        #endregion

    }

    public void PoweredDown()
    {
        _poweredDown = true;
        _shieldHp = -1;
        Shields();
        StartCoroutine(NegativeCoolDown(_powerUpTime));
    }

    IEnumerator NegativeCoolDown(float time)
    {
        yield return new WaitForSeconds(time);
        _poweredDown = false;
        _shieldHp = 0;
        _shieldVisualizer.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gameObject.transform.position, _detectRadius);

        Gizmos.matrix = gameObject.transform.localToWorldMatrix;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(Vector3.up * _beamDistance / 2,
            new Vector2(_beamBoxDirections.x, _beamDistance));

    }
}
