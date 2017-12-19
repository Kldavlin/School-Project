using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{ //this may or may not be needed
    public string aName = "New Ability";
    public Sprite aSprite; //Sprite for image (Maybe for ammo giving)
    public AudioClip aSound; //sound effect (may or may not be needed)
    public float aBaseCoolDown = 1f; //how long until it can be used next

    public abstract void Initialize(GameObject obj);

    public abstract void TriggerAbility();
}
