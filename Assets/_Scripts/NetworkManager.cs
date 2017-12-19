using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour {
    // Use this for initialization
    public void Awake()
    {
        Connect();
    }

    private void Connect()
    {
        PhotonNetwork.ConnectUsingSettings("1.0.0"); //String has to be Unique used to only match players with same version
    }
    private void OnConnectedToMaster()
    {
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.playerName = Player.Instance.PlayerName;
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }
    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
    void OnJoinedLobby()
    {
        Debug.Log("onJoinedLobby");
        if (!PhotonNetwork.inRoom)
        {
            MainCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling();
        }
    }
    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("FAILURE");
    }
    void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
    }
}
