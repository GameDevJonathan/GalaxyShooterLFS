using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player")?.GetComponent<PlayerBehaviour>();
        _anim = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);

        if (transform.position.y < -6)
        {
            topPos.x = Random.Range(-9f, 9f);
            transform.position = topPos;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerBehaviour player = other.GetComponent<PlayerBehaviour>();
            player?.OnDamage();
            DeathSequence();
            
        }

        if (other.tag == "Laser")
        {
            if (other.transform.parent != null)
            {
                _audioSource.Play();
                Destroy(other.transform.parent.gameObject);
            }
            Destroy(other.gameObject);
            _player?.AddScore(10);           
            DeathSequence();
            //Destroy(this.gameObject);
        }
    }

    private void DeathSequence()
    {
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
