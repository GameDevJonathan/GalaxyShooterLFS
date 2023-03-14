using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D),typeof(Animator),typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;
    Vector2 topPos = new Vector2(0, 6);

    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private BoxCollider2D _boxCollider;

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
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);

        if (transform.position.y < -6)
        {
            topPos.x = Random.Range(-9f, 9f);
            transform.position = topPos;
        }

    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.transform.name);
        if (other.tag == "Player")
        {

            PlayerBehaviour player = other.GetComponent<PlayerBehaviour>();
            DeathSequence();
            player?.OnDamage();
            
        }

        if (other.tag == "Laser")
        {
            GameObject.Find("Main Camera").TryGetComponent<CameraBehaviour>(out CameraBehaviour cam);
            if (other.transform.parent != null)
            {
                Destroy(other.transform.parent.gameObject);
            }
            cam.ScreenShake();
            Destroy(other.gameObject);
            DeathSequence();
            _player?.AddScore(10);           
            //Destroy(this.gameObject);
        }
    }

    public void BeamHit()
    {
        _player?.AddScore(10, 0);
        DeathSequence();
    }

    protected virtual void DeathSequence()
    {
        _spawnManager.KillCount();
        _audioSource.Play();
        _anim.Play("Explode");
        _boxCollider.enabled = false;
        moveSpeed = 0f;
    }

    public void Destroyed()
    {
        Destroy(this.gameObject);
    }
}
