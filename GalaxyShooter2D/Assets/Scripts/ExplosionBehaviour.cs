using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{
    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private float _clipTime;
    // Start is called before the first frame update
    void Start()
    {
        _anim.GetComponent<Animator>();        
        Debug.Log(_clipTime);
        Destroy(this.gameObject, _clipTime);
    }
   
}
