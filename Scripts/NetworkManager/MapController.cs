using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;


public class MapController : MonoBehaviourPunCallbacks
{
    /*
    private List<PlayerControls> players = new List<PlayerControls>();
    
    //Отправление данных синхронизации
    public void SendSyncData(Player player)
    {
        //Создать объект, содержащий все данные для синхронизации
        SyncData data = new SyncData();
        
        //Заполнение положение и количество здоровье 
        data.Lifes = new int[2];
        PlayerControls[] sortedPlayers  = players
            .Where(p => !p.IsDead)
            .OrderBy(p => p.photonView.Owner.ActorNumber)
            .ToArray();

        for (int i = 0; i < sortedPlayers.Length; i++)
        {
            data.Lifes[i] = sortedPlayers[i].Life;
        }
        RaiseEventOptions options = new RaiseEventOptions{ TargetActors = new[] { player.ActorNumber } };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(43, data, options, sendOptions);
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case 43:
                SyncData data = (SyncData) photonEvent.CustomData;

                OnSyncDataReceived(data);
                break;
        }
    }
    //Получение данных синхронизации
    private void OnSyncDataReceived(SyncData data)
    {
        //Открытие данных синхронизации
        //...
    }
    */
}
