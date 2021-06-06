using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

public class GManager : MonoBehaviourPunCallbacks
{
    public GameObject PlayerPrefab1;
    public GameObject PlayerPrefab2;
   
    // Start is called before the first frame update
    void Start()
    {
        Vector2 pos;
        
        if (PhotonNetwork.IsMasterClient) 
        {
            Debug.Log("master");
            pos = new Vector3(5.49066f, 1.798032f, 0f);
            PhotonNetwork.Instantiate(PlayerPrefab1.name, pos, Quaternion.identity, 0);
        }
        else
        {
            Debug.Log("no master");
            pos = new Vector3(126.49066f, -16.98032f, 0f);
            PhotonNetwork.Instantiate(PlayerPrefab2.name, pos, Quaternion.identity, 0);
        }
        PhotonPeer.RegisterType(typeof(Vector2Int), 242, SerializeVector2Int, DeserializeVector2Int);
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        //Когда текущий игрок (мы) покидаем комноту
        SceneManager.LoadScene(0);
        base.OnLeftRoom();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("Player {0} entered room", newPlayer.NickName);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("Player {0} left room", otherPlayer.NickName);
    }
    //превращение object в byte[]
    public static object DeserializeVector2Int(byte[] data)
    {
        Vector2Int result = new Vector2Int();
        result.x = BitConverter.ToInt32(data, 0);
        result.y = BitConverter.ToInt32(data, 4);

        return result;
    }
    //превращение byte[] а object
    public static byte[] SerializeVector2Int(object obj)
    {
        Vector2Int vector = (Vector2Int)obj;
        byte[] result = new byte[8];

        BitConverter.GetBytes(vector.x).CopyTo(result, 0);
        BitConverter.GetBytes(vector.y).CopyTo(result, 4);

        return result;
    }
}
