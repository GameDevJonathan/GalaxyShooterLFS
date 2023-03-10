using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour2 : EnemyBehaviour
{
    [SerializeField]
    private float _hSpeed = 4;
    [SerializeField]
    private bool _movingRight = false;
    [SerializeField]
    private float _min, _max, _randomMove;
    
    protected override void Update()
    {
        base.Update();

        if (_movingRight)
            transform.Translate(Vector2.right * _hSpeed * Time.deltaTime);
        else
            transform.Translate(Vector2.left * _hSpeed * Time.deltaTime);
        RandomDirection();
    }

    void RandomDirection(bool hit = false)
    {
        if (!hit)
        {
            if (_randomMove > 0f) _randomMove -= Time.deltaTime;

            if (_randomMove <= 0f)
            {
                _movingRight = !_movingRight;
                _randomMove = Random.Range(_min, _max);
            }
        }
        else
        {
            _movingRight = !_movingRight;
            _randomMove = Random.Range(_min, _max);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.transform.tag);
        
        base.OnTriggerEnter2D(other);

        if(other.transform.tag == "Bounds")
        {
            RandomDirection(true);
        }
    }

    protected override void DeathSequence()
    {
        base.DeathSequence();

        _hSpeed = 0f;
    }
}
