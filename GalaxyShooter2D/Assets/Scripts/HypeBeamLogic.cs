using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HypeBeamLogic : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("trigger stay: " + other.transform.name);
        EnemyBehaviour enemy = other.GetComponent<EnemyBehaviour>();
        enemy?.BeamHit();
        
    }

    //    private void OnTriggerEnter2D(Collider2D other)
    //    {
    //        Debug.Log("trigger enter: " + other.transform.name);

    //    }
}
