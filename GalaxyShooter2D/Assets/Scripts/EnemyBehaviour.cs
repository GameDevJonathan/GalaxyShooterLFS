using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;    
    Vector2 topPos = new Vector2(0, 6);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);   

        if(transform.position.y < -6)
        {
            topPos.x = Random.Range(-9f, 9f);
            transform.position = topPos;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Name: " + other.transform.name);
        
    }
}
