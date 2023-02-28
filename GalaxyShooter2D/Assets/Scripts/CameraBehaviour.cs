using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField]
    private Vector3 _initialPos;

    [SerializeField]
    private float _shakeTimeRemaining, _shakePower, testLength, testPower, _shakeFadeTime;

    private void Start()
    {
        _initialPos = transform.position;
    }

    void Update()
    {
        transform.position = _initialPos;

        if (Input.GetKeyDown(KeyCode.M))
        {
            ScreenShake();
        }
    }


    // Update is called once per frame
    void LateUpdate()
    {
        if(_shakeTimeRemaining > 0)
        {
            _shakeTimeRemaining -= Time.deltaTime;
            float xAmount = Random.Range(-1f, 1f) * _shakePower;
            float yAmount = Random.Range(-1f, 1f) * _shakePower;
            transform.position += new Vector3(xAmount,yAmount);

            _shakePower = Mathf.MoveTowards(_shakePower, 0, _shakeFadeTime * Time.deltaTime);
        }       
    }




    public void ScreenShake(float power = 0.3f, float length = .5f)
    {
        _shakeTimeRemaining = length;
        _shakePower = power;

        _shakeFadeTime = power / length;
    }
}
