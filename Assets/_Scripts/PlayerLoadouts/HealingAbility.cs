using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilites/HealingAbility")]
public class HealingAbility : Ability {

    public int healthPerSecond = 3;
    public float range = 50f;
    public Color laserColor = Color.green;

    //private RayCastShootTriggerable rcShoot; //when doing graphics we will need to add a shooting script

    public override void Initialize(GameObject obj)
    {
        //rcShoot = obj.GetComponent<RayCastShootTriggerable>();
        //rcShoot.Initialize();
        //rcShoot.gunDamage = gunDamage;/Will be the negative of healthPerSecond
        //rcShoot.laserLine.material = new Material (Shader.Find ("Unlit/Color"));
        //rcShoot.laserLine.material.color = laserColor;
    }

    public override void TriggerAbility()
    {
        //rcShoot.Fire();
    }


}
