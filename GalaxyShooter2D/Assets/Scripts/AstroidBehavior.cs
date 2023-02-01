using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidBehavior : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 3f;

    [SerializeField]
    private Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        if (!_anim)
            Debug.LogError("Animator is Null");
        else
            Debug.Log("Animator Found");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, _rotationSpeed) * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.TryGetComponent<Transform>(out Transform _laser);

        if(_laser?.gameObject.tag == "Laser")
        {
            _anim.SetTrigger("Explode");
            Destroy(_laser.gameObject);
        }
    }

    public void Destroyed()
    {
        Destroy(this.gameObject);
    }
}
