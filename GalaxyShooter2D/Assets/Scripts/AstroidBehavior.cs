using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidBehavior : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 3f;

    [SerializeField]
    private Animator _anim;

    private SpawnManager _spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        if (!_anim)
            Debug.LogError("Animator is Null");
        else
            Debug.Log("Animator Found");

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (!_anim)
            Debug.LogError("Manager is Null");
        else
            Debug.Log("Manager Found");

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, _rotationSpeed) * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Laser")
        {
            _anim.SetTrigger("Explode");
            Destroy(other.gameObject);
        }
    }

    public void Destroyed()
    {
        _spawnManager?.StartSpawning();
        Destroy(this.gameObject);
    }
}
