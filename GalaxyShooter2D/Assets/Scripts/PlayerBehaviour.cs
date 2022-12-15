using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float upperBound = 0f;
    [SerializeField]
    private float lowerBound = -3.8f;
    [SerializeField]
    private float LeftBound = -11.3f;
    [SerializeField]
    private float RightBound = 11.3f;

    [SerializeField]
    private Vector3 _offset = new Vector3(0, 1.05f, 0);
    
    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private int lives = 3;

    Coroutine _fireRate;
    
    
    [SerializeField]
    private float _fireSpeed = .5f;

    private SpawnManager spawnManager;
    
    
    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if(spawnManager == null)
        {
            Debug.LogError("No manager");
        }
    }

    // Update is called once per frame
    void Update()
    {
        calculateMovement();

        //if (Input.GetKey(KeyCode.Space))
        //{
        //    FireLaser();
        //}

        if (Input.GetKey(KeyCode.Space))
        {
            if (_fireRate == null)
            {
                _fireRate = StartCoroutine(fireRate(_fireSpeed));
            }
        }
    }



    void calculateMovement()
    {
        Vector3 movement = new Vector3();
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        transform.Translate(movement * speed * Time.deltaTime);

        if (transform.position.y >= upperBound)
        {
            transform.position = new Vector3(transform.position.x, upperBound, 0);
        }
        else if (transform.position.y <= lowerBound)
        {
            transform.position = new Vector3(transform.position.x, lowerBound, 0);
        }

        if (transform.position.x < LeftBound)
        {
            transform.position = new Vector3(RightBound - 0.5f, transform.position.y, 0);
        }
        else if (transform.position.x > RightBound)
        {
            transform.position = new Vector3(LeftBound + 0.5f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        GameObject laser;
        laser = Instantiate(_laserPrefab, transform.position + _offset, Quaternion.identity);

    }

    IEnumerator fireRate(float rateOfFire)
    {
        FireLaser();
        yield return new WaitForSeconds(rateOfFire);
        _fireRate = null;
    }

    public void OnDamage()
    {
        lives--;

        if(lives < 1)
        {
            spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    
}
