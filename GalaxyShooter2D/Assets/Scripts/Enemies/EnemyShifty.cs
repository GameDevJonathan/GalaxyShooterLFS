using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShifty : EnemyBehaviour2
{
    [SerializeField]
    private float _detectRadius = 3f;
    [SerializeField]
    private LayerMask _laserLayer;
    [SerializeField]
    private bool _canDodge = false;
    [SerializeField]
    private bool _dodgeRight = false;
    [SerializeField]
    private float _dodgeSpeed = 5f;
    [SerializeField]
    private Coroutine _dodgeRoutine;
    // Start is called before the first frame update
   

    // Update is called once per frame
    protected override void Update()
    {
        if (_dodgeRoutine != null) return;
        LaserDodge();
        base.Update();
    }

    void LaserDodge()
    {
        Collider2D hit =
            Physics2D.OverlapCircle(transform.position, _detectRadius,_laserLayer);

        if (hit)
        {            
            hit.TryGetComponent(out LaserBehaviour laser);
            if (laser._human == true && _dodgeRoutine != null)
            {
                //_dodgeRoutine = StartCoroutine(DodgeRoutine());   
            }
        }
    }

    //Coroutine DodgeRoutine()
    //{


    //}

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _detectRadius);
    }
}
