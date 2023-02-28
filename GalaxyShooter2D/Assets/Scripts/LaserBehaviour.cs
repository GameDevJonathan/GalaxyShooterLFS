using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    /// <summary>
    /// This is a Unity Game object script. Imma break
    /// it down best I can so you can understand it.
    /// </summary>
    
    /*
     * [SerializedField] is a field attribute it will
     * all variable's to be scene in the inspector. 
     * 
     * PRIVATE - is the accessor level. Meaning only this script
     *           can access this variable.
     *           
     * FLOAT - the data type. in this a floating point value 0.1;
     * 
     * SPEED - variable name: self explanitory 
     * 
     * 5f = is the value assigned. Remember when using floats put "f"
     * after the number to indicate it's a float or the compiler will complain.
     */
    [SerializeField]
    private float speed = 5f;    

    //Start function is call at the very first frame when the game starts
    void Start()
    {
        
        //this condition checks if the transform component of this object has a parent
        //component and if it's null. If if does then call the Destroy method on the parent
        //gameobject after 1second. It the parent is null then just destory this object.

        /*
         * The transform component holds a game objects multiple properties. Such as
         * Position - X Y Z  position / coordinates in the game world
         * Rotation - "    " rotation - which way to rotate the object. (BTW this is probably the most confusing to calculate at times)
         * Scale    - "    " scale - which way to stretch said object
         */
        if(transform.parent != null)
        {
            Destroy(transform.parent.gameObject,1f);
        }
        Destroy(this.gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * Transform.translate moves the object in a direction 
         * be aware though when moving objects with out the Time.deltaTime function.
         * The Update function on a good up-to-date computer may run thousands if not a millions
         * times faster then a slow computer. So using Time.delta time will make your movement framerate independant 
         * which helps on slow computers. 
         */
        transform.Translate(Vector2.up * speed * Time.deltaTime);       
    }    
}
