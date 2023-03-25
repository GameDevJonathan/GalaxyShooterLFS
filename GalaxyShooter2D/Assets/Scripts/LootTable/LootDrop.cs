using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrop : MonoBehaviour
{
    [SerializeField]
    LootTable lootTable;
    [SerializeField]
    private bool _canDrop;
    // Start is called before the first frame update    

    // Update is called once per frame
    void Update()
    {
        if(_canDrop)
        {
            _canDrop = false;
            PowerUpBehavior powerUp = lootTable.GetDrop();
            Debug.Log(powerUp);
            if(powerUp != null) Instantiate(powerUp.gameObject,this.gameObject.transform.position, 
                Quaternion.identity);
            
        }
        
    }

    public void SetDrop()
    {
        _canDrop = true;
    }
}
