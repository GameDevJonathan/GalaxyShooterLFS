using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _speedDefault = 5f;
    [SerializeField]
    private float _boost = 1.5f;    
    
    
    [SerializeField]
    private float _upperBound = 0f;
    [SerializeField]
    private float _lowerBound = -3.8f;
    [SerializeField]
    private float _leftBound = -11.3f;
    [SerializeField]
    private float _rightBound = 11.3f;

    [SerializeField]
    private Transform _offset;

    [SerializeField]
    private GameObject _laserPrefab;
    
    [SerializeField]
    private GameObject _tripleShotPrefab;
    
    [SerializeField]
    private bool _tripleShotActive = false;

    [SerializeField]
    private bool _speedBoostActive;
    
    [SerializeField]
    private bool _shieldsActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private float _powerUpTime = 5f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private int _score;

    [SerializeField]
    private UIManager _uiManager;

    Coroutine _fireRate;


    [SerializeField]
    private float _fireSpeed = .5f;

    private SpawnManager _spawnManager;


    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("No Spawn Manager");
        }

        if(_uiManager == null)
        {
            Debug.LogError("No UI Manager");
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        CalculateMovement();

        //if (Input.GetKey(KeyCode.Space))
        //{
        //    FireLaser();
        //}

        if (Input.GetKey(KeyCode.Space))
        {
            if (_fireRate == null)
            {
                _fireRate = StartCoroutine(FireRate(_fireSpeed));
            }
        }
    }



    void CalculateMovement()
    {
        Vector3 movement = new Vector3();
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");        

        transform.Translate(movement * _speed * Time.deltaTime);

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
    }

    void FireLaser()
    {
        if (_offset != null)
        {
            switch (_tripleShotActive)
            {
                case false:
                    GameObject laser;
                    laser = Instantiate(_laserPrefab, _offset.position, Quaternion.identity);
                    break;
                case true:
                    GameObject tripleShot;
                    tripleShot = Instantiate(_tripleShotPrefab, _offset.position, Quaternion.identity);
                    break;

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
        if (_shieldsActive)
        {
            _shieldsActive = false;
            _shieldVisualizer.SetActive(false);
            return;

        }
        
        
        _lives--;

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
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
}
