using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Animator), typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource), typeof(LootDrop))]
public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;

    [SerializeField]
    private bool _canRam = false;
    
    [SerializeField]
    private float _ramSpeed = 1f;

    [SerializeField]
    private Animator _anim;
    
    [SerializeField]
    private BoxCollider2D _boxCollider;
   
    
    [SerializeField]
    private GameObject _shieldVisualizer;
    
    [SerializeField]
    protected bool _isShielded;
    
    [SerializeField]
    private float _radius = 5f;
    
    [SerializeField]
    protected LayerMask _playerMask;

    [SerializeField]
    private LootDrop _lootDrop;


    private PlayerBehaviour _player;
    private SpawnManager _spawnManager;

    private AudioSource _audioSource;


    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        
        _player = GameObject.Find("Player")?.GetComponent<PlayerBehaviour>();
        
        _spawnManager = GameObject.Find("SpawnManager")?.GetComponent<SpawnManager>();
        
        _anim = GetComponent<Animator>();
        
        _boxCollider = GetComponent<BoxCollider2D>();

        _canRam = Random.value > 0.25;

        _lootDrop = GetComponent<LootDrop>();
        
        
        if(this.gameObject.name == "Enemy" || this.gameObject.name == "Enemy_Smart")
        {
            _isShielded = Random.value > 0.5;
            
            switch (_isShielded)
            {
                case true:
                    _shieldVisualizer?.SetActive(true);
                    break;
                case false:
                    _shieldVisualizer?.SetActive(false);
                    break;
            }

        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);

        PlayerRam();
        if (transform.position.y < -6)
        {
            if (_spawnManager == null) return;
            transform.position = _spawnManager.RandomPoint();
        }

    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {

        GameObject.Find("Main Camera").TryGetComponent<CameraBehaviour>(out CameraBehaviour cam);
        if (other.tag == "Player")
        {
            DeathSequence();
            _player?.OnDamage();
            cam.ScreenShake();
        }

        if (other.tag == "Laser")
        {
            other.TryGetComponent(out LaserBehaviour laser);
            if (!laser._human) return;
            
            if (_isShielded)
            {
                _isShielded = false;
                Destroy(other.gameObject);
                _shieldVisualizer?.SetActive(false);
                return;
            }

            if (other.transform.parent != null)
            {
                Destroy(other.transform.parent.gameObject);
            }
            cam.ScreenShake();
            Destroy(other.gameObject);
            DeathSequence();
            _player?.AddScore(10);
        }
    }

    public void BeamHit()
    {
        _player?.AddScore(10, 0);
        DeathSequence();
    }

    protected virtual void DeathSequence()
    {
        _boxCollider.enabled = false;
        _spawnManager?.KillCount();
        _audioSource?.Play();
        _lootDrop?.SetDrop();
        _anim?.Play("Explode");
        _boxCollider.enabled = false;
        moveSpeed = 0f;
        StopAllCoroutines();
    }

    private void PlayerRam()
    {
        if (!_canRam) return;

        Collider2D finder =  Physics2D.OverlapCircle(transform.position, _radius,_playerMask);
        if (finder)
        {
            Debug.Log($"{finder.transform.name}");
            Debug.Log($"{finder.transform.position}");
            if (finder)
            {
               transform.position =  Vector2.MoveTowards(transform.position, finder.transform.position, _ramSpeed * Time.deltaTime);
            }
        }

        
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }



    public void Destroyed()
    {
        Destroy(this.gameObject);
    }
}
