using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastShoot : MonoBehaviour {

    private int gunDamage = 100;
    public float fireRate = .25f; //The shooter will have to wait .25 seconds to shoot again
    public float weaponRange = 50f;
    public Transform gunEnd; //This is an empty game object at the end of the gun where the raycast will begin

    private Camera fpsCam;
    private WaitForSeconds shotDuration = new WaitForSeconds(.07f); //this determines how long the laser should remain visible (for medic this will be needed)
    //this will be used for test purposes for now
    //private AudioSource gunAudio;
    private LineRenderer laserLine; //this will be used to color the laser (on for test purposes for now)
    private float nextFire; //holds time for which the player will be allowed to fire again after a fire

	// Use this for initialization
	void Awake ()
    {
        laserLine = GetComponent<LineRenderer>();
        //gunAudio = GetComponent<AudioSource>();
        fpsCam = GetComponentInParent<Camera>();
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            StartCoroutine(ShotEffect());

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            laserLine.SetPosition(0, gunEnd.position);

            if(Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
            {
                laserLine.SetPosition(1, hit.point);

                PhotonView photonView = hit.collider.GetComponent<PhotonView>();
                if(photonView != null)
                {
                    print("Shot Fired and Hit");
                    PlayerManagement.Instance.ModifiyHealth(photonView.owner, -gunDamage);
                }
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange));
            }
        }
	}

    private IEnumerator ShotEffect()
    {
        //gunAudio.Play();

        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
    }
}
