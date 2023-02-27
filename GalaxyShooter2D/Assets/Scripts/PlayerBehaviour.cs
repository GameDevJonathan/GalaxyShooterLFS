using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _speedDefault = 5f;
    [SerializeField]
    private float _boost = 1.5f;
    [SerializeField]
    private float _thrusterBoost = 1.25f;
    [Space]

    [Header("Movement Bounds")]
    [SerializeField]
    private float _upperBound = 0f;
    [SerializeField]
    private float _lowerBound = -3.8f;
    [SerializeField]
    private float _leftBound = -11.3f;
    [SerializeField]
    private float _rightBound = 11.3f;

    [Header("Spawn Points and Ammo")]
    [SerializeField]
    private Transform _offset;

    [SerializeField]
    private int _maxAmmo = 15;

    [SerializeField]
    private int _ammo;

    [SerializeField]
    private GameObject _laserPrefab;

    Coroutine _fireRate;

    [SerializeField]
    private float _fireSpeed = .5f;


    [Header("Special Meter")]
    [SerializeField]
    private float _specialMeter = 100f;

    [Header("Hyper Beam")]
    [SerializeField]
    private Transform _hyperBeamSpawnPoint;
    [SerializeField]
    private Vector3 _beamOffset;
    [SerializeField]
    private float _beamDistance = 100f, _beamDuration = 4f;
    [SerializeField]
    private BoxCollider2D _boxCollider;
    [SerializeField]
    private float _boxWidth = 1f;
    [SerializeField]
    private float _boxSideWitdh = 1f;
    [SerializeField]
    private LayerMask enemyLayer;
    [SerializeField]
    private LineRenderer _lineRenderer;
    Coroutine HyperBeamCoroutine;

    [Header("Missile Barrage")]
    [SerializeField]
    GameObject _rocketPrefab;
    [SerializeField]
    private Transform[] _missleFirePoint;
    Coroutine missleBarageCoroutine;


    [Header("Flags and Prefabs")]
    [SerializeField]
    private bool _tripleShotActive = false;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private bool _speedBoostActive = false;

    [SerializeField]
    private bool _shieldsActive = false;

    [SerializeField]
    private SpriteRenderer _shieldSprite;

    [SerializeField]
    private int _shieldHp = 0;

    [SerializeField]
    private GameObject _shieldVisualizer, _leftThruster, _rightThruster;

    [SerializeField]
    private float _powerUpTime = 5f;


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


    // Start is called before the first frame update
    void Start()
    {

        _audioSource = GetComponent<AudioSource>();

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("No Spawn Manager");
        }

        if (_uiManager == null)
        {
            Debug.LogError("No UI Manager");
        }

        if (!_shieldSprite)
        {
            Debug.LogError("Shield Sprite Not Found");
        }

        _ammo = _maxAmmo;
        _uiManager.UpdateAmmo(_ammo, _maxAmmo);
    }

    // Update is called once per frame
    void Update()
    {
        Debuging();
        _specialMeter = Mathf.Clamp(_specialMeter, 0, 100);

       
        CalculateMovement();


        if (Input.GetKey(KeyCode.Space))
        {
            if (_fireRate == null)
            {
                _fireRate = StartCoroutine(FireRate(_fireSpeed));
            }

        }

        if (Input.GetKeyDown(KeyCode.X) /*&& _specialMeter == 100*/)
        {

            //LaserBeamDebug();

            if (HyperBeamCoroutine == null)
            {
                HyperBeamCoroutine = StartCoroutine(LaserBeam());
            }

            //LaserBeamDebug();
            //if (missleBarageCoroutine == null)
            //{
            //    _specialMeter = 0;
            //    missleBarageCoroutine = StartCoroutine(MissleBarrage());
            //}
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
        }
    }

    IEnumerator LaserBeam()
    {
        _lineRenderer.enabled = true;
        _boxCollider.enabled = true;

        //RaycastHit2D hitInfo = Physics2D.Raycast(_hyperBeamSpawnPoint.position, _hyperBeamSpawnPoint.up * _beamDistance);
        //Debug.DrawRay(_hyperBeamSpawnPoint.position, _hyperBeamSpawnPoint.up * _beamDistance, Color.white);

        //RaycastHit2D[] hitInfo = Physics2D.BoxCastAll(_boxCollider.bounds.center, _boxCollider.bounds.size + new Vector3(0,_boxSideWitdh), 0f, Vector2.up, _beamDistance, enemyLayer);
        //Debug.DrawRay(_boxCollider.bounds.center + new Vector3(_boxCollider.bounds.extents.x + _boxSideWitdh, 0), Vector2.up * (_boxCollider.bounds.extents.y + _beamDistance), Color.red);
        //Debug.DrawRay(_boxCollider.bounds.center - new Vector3(_boxCollider.bounds.extents.x + _boxSideWitdh, 0), Vector2.up * (_boxCollider.bounds.extents.y + _beamDistance), Color.red);
        //Debug.DrawRay(_boxCollider.bounds.center + new Vector3(_boxCollider.bounds.extents.x + _boxSideWitdh, _boxCollider.bounds.extents.y + _beamDistance), Vector2.left * (_boxCollider.bounds.extents.y + _boxWidth), Color.blue);


        //foreach(RaycastHit2D enemy in hitInfo)
        //{
        //    if (enemy.transform.tag == "Enemy")
        //    {
        //        Debug.Log(enemy.transform.name);
        //    }
        //}


        //if (hitInfo && hitInfo.transform.tag == "Enemy")
        //{
        //    EnemyBehaviour enemy = hitInfo.transform.GetComponent<EnemyBehaviour>();
        //    enemy?.BeamHit();
        //}

        _lineRenderer.SetPosition(0, _hyperBeamSpawnPoint.position);
        _lineRenderer.SetPosition(1, _hyperBeamSpawnPoint.position + _hyperBeamSpawnPoint.up * _beamDistance);
        yield return new WaitForSeconds(_beamDuration);
        _lineRenderer.enabled = false;
        _boxCollider.enabled = false;
        HyperBeamCoroutine = null;
    }
    //void LaserBeamDebug()
    //{
    ////    //_lineRenderer.enabled = true;

    //    RaycastHit2D hitInfo = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0f, Vector2.up, _beamDistance, enemyLayer);
    //    Debug.DrawRay(_boxCollider.bounds.center + new Vector3(_boxCollider.bounds.extents.x + _boxSideWitdh, 0), Vector2.up* (_boxCollider.bounds.extents.y + _beamDistance), Color.red);
    //    Debug.DrawRay(_boxCollider.bounds.center - new Vector3(_boxCollider.bounds.extents.x + _boxSideWitdh, 0), Vector2.up* (_boxCollider.bounds.extents.y + _beamDistance), Color.red);
    //    Debug.DrawRay(_boxCollider.bounds.center + new Vector3(_boxCollider.bounds.extents.x + _boxSideWitdh, _boxCollider.bounds.extents.y + _beamDistance), Vector2.left* (_boxCollider.bounds.extents.y +_boxWidth), Color.blue);

    ////    if (hitInfo && hitInfo.transform.name == "Enemy")
    ////    {
    ////        Debug.Log("hit enemy");
    ////        EnemyBehaviour enemy = hitInfo.transform.GetComponent<EnemyBehaviour>();
    ////        enemy?.BeamHit();
    ////    }

    ////    //_lineRenderer.SetPosition(0, _hyperBeamSpawnPoint.position);
    ////    //_lineRenderer.SetPosition(1, _hyperBeamSpawnPoint.position + _hyperBeamSpawnPoint.up * _beamDistance);
    ////    //yield return new WaitForSeconds(_beamDuration);
    ////    //_lineRenderer.enabled = false;
    ////    //HyperBeamCoroutine = null;
    //}
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
        if (HyperBeamCoroutine != null)
            return;

        Vector3 movement = new Vector3();
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        transform.Translate(movement * _speed * Time.deltaTime);

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
                        _uiManager.UpdateAmmo(_ammo);
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
        if (!_speedBoostActive)
        {
            #region code block
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _speed *= _thrusterBoost;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
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
            _uiManager.UpdateShields(_shieldHp);
        }

        if (_shieldHp == 0)
        {
            _shieldsActive = false;
            _shieldVisualizer.SetActive(false);
            _uiManager.UpdateShields(_shieldHp);
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
                _uiManager.UpdateShields(_shieldHp);
                ActivateShields();
            }
            else
            {
                Shields();
            }
        }
    }

    //method to add 10 to the score
    public void AddScore(int score)
    {
        _score += score;
        _uiManager.UpdateScore(_score);
    }
    //communicate with the ui to update the score

    IEnumerator FireRate(float rateOfFire)
    {
        FireLaser();
        yield return new WaitForSeconds(rateOfFire);
        _fireRate = null;
    }

    public void OnDamage()
    {
        if (_shieldHp > 0)
        {
            Shields();
            return;
        }

        _lives--;

        _uiManager.UpdateLives(_lives);

        switch (_lives)
        {
            case 2:
                _leftThruster.SetActive(true);
                break;
            case 1:
                _rightThruster.SetActive(true);
                break;
        }

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void RefillAmmo()
    {
        _ammo = _maxAmmo;
        _uiManager.UpdateAmmo(_ammo);
    }

    public void HealthUp()
    {
        _lives++;
        _uiManager.UpdateLives(_lives);
    }

    public void ActivateTripleShot()
    {
        _tripleShotActive = true;
        StartCoroutine(PowerUpTime(_powerUpTime));
    }

    public void ActivateSpeedBoost()
    {
        _speedBoostActive = true;
        _speed *= _boost;
        StartCoroutine(SpeedCoolDown(_powerUpTime));
    }

    public void ActivateShields()
    {
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

    #region laser enable code
    private void EnableLaser()
    {

    }

    private void UpdateLaser()
    {

    }
    #endregion

    private void OnDrawGizmos()
    {
        
        //Debug.DrawRay(_boxCollider.bounds.center + new Vector3(_boxCollider.bounds.extents.x + _boxSideWitdh,0), Vector2.up * (_boxCollider.bounds.extents.y + _beamDistance), Color.green);
        //Debug.DrawRay(_boxCollider.bounds.center - new Vector3(_boxCollider.bounds.extents.x + _boxSideWitdh,0), Vector2.up * (_boxCollider.bounds.extents.y + _beamDistance), Color.green);
        //Debug.DrawRay(_boxCollider.bounds.center + new Vector3(_boxCollider.bounds.extents.x + _boxSideWitdh,_boxCollider.bounds.extents.y + _beamDistance), Vector2.left  * (_boxCollider.bounds.extents.y +_boxWidth), Color.green);

    }
}
