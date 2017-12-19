using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public static Player Instance;
    public string PlayerName { get; private set; }
    private PhotonView PhotonView;
    private int playersInGame = 0;
    private PlayerMovement CurrentPlayer;


    public GameObject standbyCamera;
    private List<SpawnSpot> spawnSpots;

    // Use this for initialization
    private void Awake () {
        Instance = this;
        PhotonView = GetComponent<PhotonView>();

        PlayerName = "Distul#" + Random.Range(1000, 9999);

        PhotonNetwork.sendRate = 60;
        PhotonNetwork.sendRateOnSerialize = 30;

        SceneManager.sceneLoaded += OnSceneFinishedLoading;

	}

    void SpawnMyFPSPlayer()
    {
        print("Spawing player");
        spawnSpots = new List<SpawnSpot>();
        SpawnSpot[] spawnSpotsArray = FindObjectsOfType<SpawnSpot>();
        foreach (SpawnSpot spawn in spawnSpotsArray)
        {
            spawnSpots.Add(spawn);
        }

        if (spawnSpots == null)
        {
            Debug.LogError("NO SPAWN SPOTS");
            return;
        }

        SpawnSpot mySpawnSpot = spawnSpots[Random.Range(0, spawnSpots.Count)];
        spawnSpots.Remove(mySpawnSpot);

        GameObject myPlayer = PhotonNetwork.Instantiate("PlayerController", mySpawnSpot.transform.position, mySpawnSpot.transform.rotation, 0);
        CurrentPlayer = myPlayer.GetComponent<PlayerMovement>();
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            MasterLoadedGame();
        }
    }

    private void MasterLoadedGame()
    {
        PhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient, PhotonNetwork.player);
    }

    [PunRPC]
    private void RPC_LoadGameOthers()
    {
        PhotonNetwork.LoadLevel(1);
    }

    [PunRPC]
    private void RPC_LoadedGameScene(PhotonPlayer photonPlayer)
    {
        PlayerManagement.Instance.AddPlayerStats(photonPlayer);
        playersInGame++;
        if(playersInGame == PhotonNetwork.playerList.Length)
        {
            print("all players are in the game scene");
            PhotonView.RPC("RPC_CreatePlayer", PhotonTargets.All);
        }
    }

    public void NewHealth(PhotonPlayer photonPlayer, int health)
    {
        PhotonView.RPC("RPC_NewHealth", photonPlayer, health);
    }

    [PunRPC]
    private void RPC_NewHealth(int health)
    {
        if(CurrentPlayer == null)
        {
            return;
        }

        if(health <= 0)
        {
            PhotonNetwork.Destroy(CurrentPlayer.gameObject); //Figure out how to put maybe a way to view other players? Can figure this out later.
        }
        else
        {
            CurrentPlayer.setHealth(health);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        SpawnMyFPSPlayer();
    }
}
