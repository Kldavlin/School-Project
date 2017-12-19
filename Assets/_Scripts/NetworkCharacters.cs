using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCharacters : Photon.MonoBehaviour
{

    Vector3 realPosition = Vector3.zero;
    Quaternion realRotation = Quaternion.identity;
    Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        if(animator == null)
        {
            Debug.LogError("NO ANIMATOR ON PREFAB");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.isMine) //not our view do stuff
        {
            transform.position = Vector3.Lerp(transform.position, realPosition, .1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, .1f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // This is OUR player. We need to send our actual position to the network.

            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(animator.GetFloat("Speed"));//you will need to send any info relating to animator variables!

        }
        else
        {
            //This is someone else's player. We need to receive their position
            //as of a few miliseconds ago, and update our version of that player.

            realPosition = (Vector3)stream.ReceiveNext();
            realRotation = (Quaternion)stream.ReceiveNext();
            animator.SetFloat("Speed", (float)stream.ReceiveNext()); // you will need to receieve any info relating to animator variables!
        }

    }
}
