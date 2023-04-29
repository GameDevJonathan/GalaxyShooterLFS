using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShieldIndicator : MonoBehaviour
{
    public Image LivesDisplay;
    public GameObject Player;

    private void Update()
    {
        if (Player != null)
        {
            if (Player.GetComponent<PlayerBehaviour>()._shieldsActive)
            {
                switch (Player.GetComponent<PlayerBehaviour>()._shieldHp) 
                { 
                 
                    case 3:
                        LivesDisplay.color = Color.blue;
                        break;
                    case 2:
                        LivesDisplay.color = Color.magenta;
                        break;
                    case 1:
                        LivesDisplay.color = Color.red;
                        break;
                }
               
            } else
            {
                LivesDisplay.color = Color.white;
            }
        }


    }




}
