using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpIcon : MonoBehaviour
{
    public Image weaponImage;
    public GameObject Player;


    public Sprite mainWeaponSprite;
    public Sprite tripleShotSprite;
    private void Update()
    {
        if (Player != null) 
        {
            if (Player.GetComponent<PlayerBehaviour>()._tripleShotActive)
            {
                weaponImage.sprite = tripleShotSprite;
            } else
            {
                weaponImage.sprite = mainWeaponSprite;
            }
                
        }

    }

}
