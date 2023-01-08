using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehavior : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2f;
    [SerializeField] // 0 = triple shot / 1 = speed boost / 2 = shields 
    private int _powerUpID; 

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
            PlayerBehaviour player = other.GetComponent<PlayerBehaviour>();
            if(player != null)
            {
                switch (_powerUpID)
                {
                    case 0:
                        player.ActivateTripleShot();
                        break;
                    case 1:
                        player.ActivateSpeedBoost();
                        break;
                }
            }
            Destroy(this.gameObject);
            
        }

    }
}
