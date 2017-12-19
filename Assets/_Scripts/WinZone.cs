using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour {

    private void OnTriggerEnter(Collider other) //testing damage currently later will be players win
    {
        if(!PhotonNetwork.isMasterClient)
        {
            return;
        }
        PhotonView photonView = other.GetComponent<PhotonView>();
        if(photonView != null)
        {
            PlayerManagement.Instance.ModifiyHealth(photonView.owner, -10);
        }
    }
}
