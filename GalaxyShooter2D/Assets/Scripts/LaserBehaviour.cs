using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    /// <summary>
    /// This is a Unity Game object script. Imma break
    /// it down best I can so you can understand it.
    /// </summary>    

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    public bool _human = true;
    [SerializeField]
    public bool _up = true;

    //Start function is call at the very first frame when the game starts
    void Start()
    {

        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject, 1f);
        }
        Destroy(this.gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        switch (_up)
        {
            case true:
                transform.Translate(Vector2.up * speed * Time.deltaTime);
                break;
            case false:
                transform.Translate(Vector2.down * speed * Time.deltaTime);
                break;

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !_human)
        {            
            other.TryGetComponent(out PlayerBehaviour player);
            player.OnDamage();
            Destroy(this.gameObject);
        }
        
    }
}
