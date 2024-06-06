using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager instance;

    public GameObject lobbyGameObject;
    public GameObject player1Object;
    public GameObject player2Object;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");
        PhotonNetwork.JoinLobby(); // Automatically join the lobby
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby.");
        lobbyGameObject.SetActive(true); // Activate the lobby GameObject when joined
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a random room, creating a new room.");
        CreateRoom();
    }

    void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room successfully.");
        CheckAndStartGame();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player joined: " + newPlayer.NickName);

        // Assign player names when they enter the lobby
        if (PhotonNetwork.PlayerList.Length == 1)
        {
            newPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "PlayerName", "Player 1" } });
            player1Object.SetActive(true); // Activate Player 1 GameObject
        }
        else if (PhotonNetwork.PlayerList.Length == 2)
        {
            newPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "PlayerName", "Player 2" } });
            player2Object.SetActive(true); // Activate Player 2 GameObject
        }
    }

    public override void OnLeftRoom()
    {
        player1Object.SetActive(false); // Deactivate Player 1 GameObject when leaving the room
        player2Object.SetActive(false); // Deactivate Player 2 GameObject when leaving the room
    }

    public void CheckAndStartGame()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            // Start the game if 2 players are in the room
            StartCoroutine(StartGameAfterDelay(2f));
        }
    }

    IEnumerator StartGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Load your game scene or start the game
        UImanager.instance.play();
    }
}
