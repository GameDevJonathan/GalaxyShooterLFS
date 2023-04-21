using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingMissle : MonoBehaviour
{
    [Header("Missile Variables")]
    [SerializeField]
    private float _missleSpeed = 4f, _rotateSpeed = 300f, _rotateAmount, _rotationCorrection = 30f;
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    public Transform target;
    [SerializeField]
    private Vector2 _direction;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (!_rb)
        {
            Debug.LogError("Rigid Body Not Found");
        }        
    }

    void OnTriggerEnter2D(Collider2D other )
    {
        if (other.tag == "Enemy" && target == null)
        {
            target = other.GetComponent<Transform>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (target)
        {
            target = null;            
        }
    }




    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            _direction = (Vector2)target.position - _rb.position;
            _direction.Normalize();
            _rotateAmount = Vector3.Cross(_direction, transform.up).z;
            _rb.angularVelocity = -_rotateAmount * _rotateSpeed;

        }
        else
        {
            _rb.rotation = Mathf.Lerp(_rb.rotation, 0, _rotationCorrection * Time.deltaTime); 
        }
        _rb.velocity = transform.up * _missleSpeed * Time.deltaTime;
        
    }
}
