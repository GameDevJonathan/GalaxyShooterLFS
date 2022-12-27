using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{

    [SerializeField]
    private float speed = 5f;

    void Start()
    {
        if(transform.parent != null)
        {
            Destroy(transform.parent.gameObject,1f);
        }
        Destroy(this.gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);       
    }    
}
