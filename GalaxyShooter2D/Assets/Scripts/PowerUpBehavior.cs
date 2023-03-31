using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehavior : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2f;
    [SerializeField]
    private GameObject explosion;
    [SerializeField] // 0 = triple shot / 1 = speed boost / 2 = shields /3 = ammo refill / 4 = health up
    private enum PowerUp { TripleShot, SpeedBoost, Shields, AmmoRefill, HealthUp, Negative };

    [SerializeField]
    private PowerUp _playerPowerUp;



    [SerializeField]
    private AudioClip _audioClip;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * _speed * Time.deltaTime);

        if (transform.position.y < -6.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_audioClip, transform.position);
            PlayerBehaviour player = other.GetComponent<PlayerBehaviour>();
            if (player != null)
            {
                switch (_playerPowerUp)
                {
                    case PowerUp.TripleShot:
                        player.ActivateTripleShot();
                        break;
                    case PowerUp.SpeedBoost:
                        player.ActivateSpeedBoost();
                        break;
                    case PowerUp.Shields:
                        player.ActivateShields();
                        break;
                    case PowerUp.AmmoRefill:
                        player.RefillAmmo();
                        break;
                    case PowerUp.HealthUp:
                        player.HealthUp();
                        break;
                    case PowerUp.Negative:
                        player.PoweredDown();
                        break;
                }
            }
            Destroy(this.gameObject);

        }

        if (other.tag == "Laser")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

    }
}
