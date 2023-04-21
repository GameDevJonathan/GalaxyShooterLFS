using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShifty : EnemyBehaviour
{
    [SerializeField]
    private float _detectRadius = 3f; // size of the sphere
    [SerializeField]
    private LayerMask _laserLayer; // layer mask to detect object
    [SerializeField]
    private bool _canDodge = false; // flag to see if object can dodge
    [SerializeField]
    private bool _dodgeRight = false; // flag for which direction to dodge
    
    [SerializeField] //floats for how fast to dodge, how long to dodge, and how time between dodges.
    private float _dodgeSpeed = 5f, _dodgeTime = 1f, _dodgeCounter = 0f; 
    [SerializeField]
    private Coroutine _dodgeRoutine; //coroutine to dodge
    [SerializeField]
    private Vector2 _dodgeVector = new Vector2(4, 0); // dodge vector
    // Start is called before the first frame update

   


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        //if (_dodgeRoutine != null) return;
        LaserDodge();
    }

    void LaserDodge()
    {
        Collider2D hit =
            Physics2D.OverlapCircle(transform.position, _detectRadius, _laserLayer);
        if (hit)
        {
            hit.TryGetComponent(out LaserBehaviour laser);
            if (laser._human == true && !_canDodge)
            {
                Debug.Log("start dodging");
                //_dodgeRoutine = StartCoroutine(DodgeRoutine());
                _canDodge = Random.value > 0.5f;
                if (_canDodge)
                {
                    _dodgeRight = Random.value > 0.5f;
                    _dodgeCounter = _dodgeTime;
                }
            }
        }
        
        if(!hit && _dodgeCounter < 0 && _canDodge)
        {
            _canDodge = false;
            _dodgeCounter = 0;
        }

        if (_dodgeCounter > 0 && _canDodge)
        {
            if (_dodgeRight)
            {
                transform.Translate(_dodgeVector * _dodgeSpeed * Time.deltaTime);
            }

            if (!_dodgeRight)
            {
                transform.Translate(-_dodgeVector * _dodgeSpeed * Time.deltaTime);
            }

            _dodgeCounter -= Time.deltaTime;
        }
    }
  

    //    if (Input.GetButtonDown("Fire2") && standing.activeSelf && abilities.canDash)
    //                {
    //                    dashCounter = dashTime;
    //                    ShowAfterImage();
    //    AudioManager.instance.PlaySFXAdjusted(7);
    //                }

    //            }



    //            if (dashCounter > 0)
    //{
    //    dashCounter = dashCounter - Time.deltaTime;

    //    body.velocity = new Vector2(dashSpeed * transform.localScale.x, body.velocity.y);

    //    afterImageCounter -= Time.deltaTime;

    //    if (afterImageCounter <= 0)
    //    {
    //        ShowAfterImage();
    //    }

    //    dashRecharge = coolDown;
    //}






    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _detectRadius);
    }
}
