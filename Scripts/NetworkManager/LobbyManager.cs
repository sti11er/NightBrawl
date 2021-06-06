using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public GameObject PanelLoading;
    string gameVersion = "1";
    private byte maxPlayersPerRoom = 2;

    // Start is called before the first frame update
    private void Start()
    {
        PhotonNetwork.NickName = "Player" + Random.Range(1000, 9999);
        Log("Player's name is set to " + PhotonNetwork.NickName);

        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.AutomaticallySyncScene = true; //Автомотическое переключение сцен
        PhotonNetwork.ConnectUsingSettings(); //Подключение к мастер серверу
    }

    public override void OnConnectedToMaster()
    {
        Log("Connected to Master");
        PanelLoading.SetActive(false);
    }
    public void StartGame()
    {
        Log("Join room");
        PhotonNetwork.JoinRandomRoom();
    }
    public void Options()
    {
        SceneManager.LoadScene(2);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom()");
        // Debug.Log(PhotonNetwork.IsConnected);
        // if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        // {  
        //     PhotonNetwork.LoadLevel("Game");
        //     Debug.Log("Master Connected in Room");
        // }
        // if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        // {
        //     PhotonNetwork.LoadLevel("Game");
        // }
        PhotonNetwork.LoadLevel("Game");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Log("Failed to create room " + message);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Log("Failed to join room " + message);
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = maxPlayersPerRoom});
    }
    // Update is called once per frame
    private void Log(string message)
    {
        Debug.Log(message);
    }
}


