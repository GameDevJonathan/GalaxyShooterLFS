using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;
    Vector2 topPos = new Vector2(0, 6);

    private PlayerBehaviour _player;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerBehaviour>();
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
            player.OnDamage();
            Destroy(this.gameObject);
        }

        if (other.tag == "Laser")
        {
            if (other.transform.parent != null)
            {
                Destroy(other.transform.parent.gameObject);
            }
            Destroy(other.gameObject);
            
            _player?.AddScore(10);
            Destroy(this.gameObject);
        }
    }
}
