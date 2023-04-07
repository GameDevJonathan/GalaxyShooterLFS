using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour2 : EnemyBehaviour
{
    [SerializeField]
    private float _hSpeed = 4; //horizontal speed
    [SerializeField]
    private bool _movingRight = false; // flag to see if i'm moving left or right
    [SerializeField]
    private float _min, _max, _randomMove; // floats for random move counter, min and max values
    
    protected override void Update()
    {
        _isShielded = false;
        base.Update();

        if (_movingRight)
            transform.Translate(Vector2.right * _hSpeed * Time.deltaTime);
        else
            transform.Translate(Vector2.left * _hSpeed * Time.deltaTime);
        RandomDirection();
    }

    protected virtual void RandomDirection(bool hit = false)
    {
        if (!hit)
        {
            if (_randomMove > 0f) _randomMove -= Time.deltaTime; // check to see if the random move counter is greater then zero

            if (_randomMove <= 0f) // if it's less then or equal to zero set bool to the opposite of itself and reset the counter
            {
                _movingRight = !_movingRight;
                _randomMove = Random.Range(_min, _max);
            }
        }
        else //if hit is true set bool to opposite of itself and reset the counter. 
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
