using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmart : EnemyBehaviour
{
    [SerializeField]
    private Vector2 _detectionBoxForward = new Vector2(.5f, .5f),
                    _detectionBoxBackward = new Vector2(.5f, .5f);
    [SerializeField]
    private float _detectionRangeForward = 20f,
                  _detectionRangeBackward = 20f;

    [SerializeField]
    private LaserBehaviour _laser;

    [SerializeField]
    private Transform _firePointFront, _firePointBack;

    [SerializeField]
    private Coroutine _fireAtPlayer;

    [SerializeField]
    private float _timeToFire;


    protected override void Update()
    {
        DetectionRange();
        base.Update();

    }


    private void DetectionRange()
    {
        RaycastHit2D hitForward =
            Physics2D.BoxCast(gameObject.transform.position, _detectionBoxForward,
                              gameObject.transform.rotation.z, Vector2.down,
                              _detectionRangeForward, _playerMask);

        RaycastHit2D hitBackwards =
            Physics2D.BoxCast(gameObject.transform.position, _detectionBoxBackward,
                              gameObject.transform.rotation.z, Vector2.up,
                              _detectionRangeBackward, _playerMask);

        if (hitForward)
        {
            Debug.Log(hitForward.transform.name);
            if (_fireAtPlayer == null)
            {
                _fireAtPlayer = StartCoroutine(FireLaser(_firePointFront,false));
            }
        }

        if (hitBackwards)
        {
            if (_fireAtPlayer == null)
            {
                _fireAtPlayer = StartCoroutine(FireLaser(_firePointBack,true));
            }
        }
    }

    IEnumerator FireLaser(Transform firePoint,bool fireUp)
    {
        _timeToFire = Random.Range(.5f, 1.5f);
        yield return new WaitForSeconds(_timeToFire);
        LaserBehaviour laser = Instantiate(_laser.gameObject, firePoint.position,
                                            Quaternion.identity).GetComponent<LaserBehaviour>();
        laser._human = false;
        laser._up = fireUp;
        _fireAtPlayer = null;
    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.matrix = gameObject.transform.localToWorldMatrix;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.down * _detectionRangeForward / 2,
            new Vector2(_detectionBoxForward.x, _detectionRangeForward));

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(Vector3.up * _detectionRangeBackward / 2,
            new Vector2(_detectionBoxBackward.x, _detectionRangeBackward));
    }


}
