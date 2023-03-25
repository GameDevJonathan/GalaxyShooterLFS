using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Animator), typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;

    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private BoxCollider2D _boxCollider;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private bool _isShielded = true;

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

        _isShielded = Random.value > 0.5;

        switch (_isShielded)
        {
            case true:
                _shieldVisualizer.SetActive(true);
                break;
            case false:
                _shieldVisualizer.SetActive(false);
                break;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);

        if (transform.position.y < -6)
        {
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
            if (_isShielded)
            {
                _isShielded = false;
                Destroy(other.gameObject);
                _shieldVisualizer.SetActive(false);
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
        _spawnManager?.KillCount();
        _audioSource?.Play();
        _anim?.Play("Explode");
        _boxCollider.enabled = false;
        moveSpeed = 0f;
    }

    public void Destroyed()
    {
        Destroy(this.gameObject);
    }
}
