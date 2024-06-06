using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject canvasWaiting;
    Animator anim;
    public GameObject cameraFollow;
    public GameObject mainMenu;
    public Text player1Text;
    public Text player2Text;
    bool isPlayerJoin = false;
    bool isLobby = false;
    PhotonView view;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        view = GetComponent<PhotonView>();
        ConnectToPhoton();
        anim = cameraFollow.gameObject.GetComponent<Animator>();
        mainMenu.SetActive(true);
        canvasWaiting.SetActive(false);
        PhotonNetwork.AddCallbackTarget(this);
    }

    void ConnectToPhoton()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");
        JoinLobby();
    }

    void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Photon Lobby");
    }

    public void JoinRoom()
    {
        Debug.Log("Attempting to join a random room...");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"Failed to join a random room ({returnCode}): {message}. Creating a new room.");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("Creating a new room...");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created a new room successfully.");
        isLobby = true;
        canvasWaiting.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room successfully.");
        isLobby = true;
        canvasWaiting.SetActive(true);
        SetPlayerNickname();
    }

    private void Update()
    {
        if (isLobby)
        {
            SetPlayerNickname();
        }
    }

    void SetPlayerNickname()
    {
        PhotonNetwork.NickName = "Player" + PhotonNetwork.LocalPlayer.ActorNumber;
        UpdatePlayerUI();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player joined: " + newPlayer.NickName);
        UpdatePlayerUI();

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            StartGame();
            //UImanager.instance.play();
            anim.SetBool("play", true);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player left: " + otherPlayer.NickName);
        UpdatePlayerUI();
    }

    void UpdatePlayerUI()
    {
        player1Text.text = GetPlayerName(0);
        player2Text.text = GetPlayerName(1);
    }

    string GetPlayerName(int index)
    {
        if (PhotonNetwork.CurrentRoom.Players.TryGetValue(index + 1, out Photon.Realtime.Player player))
        {
            return player.NickName;
        }
        else
        {
            return "Waiting...";
        }
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Notify other clients to load the game scene
            Debug.Log("RPC Run");
            view.RPC("StartGameRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    void StartGameRPC()
    {
        isLobby = false;
        canvasWaiting.SetActive(false);

        Debug.Log("Starting the game...");
        //UImanager.instance.play();
        mainMenu.SetActive(false);
        anim.SetBool("play", true);
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        if (playerIndex < 0 || playerIndex >= spawnPoints.Length)
        {
            Debug.LogError("Invalid player index or not enough spawn points.");
            return;
        }

        Transform spawnPoint = spawnPoints[playerIndex];
        PhotonNetwork.Instantiate("boy", spawnPoint.position, spawnPoint.rotation, 0);
    }

    public void GoToLobby()
    {
        canvasWaiting.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }
}
