using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour {
    public static PlayerManagement Instance;
    private PhotonView PhotonView;

    private List<PlayerStats> PlayerStats = new List<PlayerStats>();

	// Use this for initialization
	void Awake () {
        Instance = this;
        PhotonView = GetComponent<PhotonView>();
	}

    public void AddPlayerStats(PhotonPlayer photonPlayer)
    {
        int index = PlayerStats.FindIndex(x => x.PhotonPlayer == photonPlayer);
        if(index == -1)
        {
            PlayerStats.Add(new PlayerStats(photonPlayer, 100));
        }
    }

    public void ModifiyHealth(PhotonPlayer photonPlayer, int value)
    {
        print("damage = " + value);
        int index = PlayerStats.FindIndex(x => x.PhotonPlayer == photonPlayer); //finds index of player that needs modifying
        if (index != -1)
        {
            print("Player found");
            PlayerStats playerStats = PlayerStats[index];
            playerStats.Health += value;
            print("Player health = " + playerStats.Health);
            Player.Instance.NewHealth(photonPlayer, playerStats.Health);
        }
    }
}

public class PlayerStats
{
    public PlayerStats(PhotonPlayer photonPlayer, int health)
    {
        PhotonPlayer = photonPlayer;
        Health = health;
    }

    public readonly PhotonPlayer PhotonPlayer;
    public int Health;
}