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
        //StartCoroutine(ShowCurrentClipTime());
    }

    private void Update()
    {
        if(!_anim.IsInTransition(0) && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            Destroy(this.gameObject.gameObject);
        }
    }

    IEnumerator ShowCurrentClipTime()
    {
        yield return new WaitForEndOfFrame();
        _clipTime = _anim.GetCurrentAnimatorStateInfo(0).length;
        Debug.Log($"Clip Time:{_clipTime}");
    }
   
}
