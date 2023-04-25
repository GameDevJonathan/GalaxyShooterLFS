using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossExplosion : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private float _minRange, _maxRange;
    [SerializeField]
    private BossBehaviour bossBehavior;
    private CameraBehaviour mainCam;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main.GetComponent<CameraBehaviour>();
        StartCoroutine(RandomExpolsion());
    }

    IEnumerator RandomExpolsion()
    {
        int explosionCount = 0;
        //Vector3 ranExplosion = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5));
        if (_explosion)
        {
            while (explosionCount < 20)
            {
                Instantiate(_explosion,
                    transform.position + new Vector3(Random.Range(_minRange, _maxRange), Random.Range(_minRange,_maxRange)),
                    Quaternion.identity);
                mainCam.ScreenShake();
                yield return new WaitForSeconds(0.25f);
                explosionCount++;
            }
        }
        Destroy(this.gameObject);
        bossBehavior.DeadState();        
    }

   
}
