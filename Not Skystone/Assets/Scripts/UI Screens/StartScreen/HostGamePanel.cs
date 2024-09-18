using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class HostGamePanel : MonoBehaviourPunCallbacks
{
	[SerializeField]
	private TMP_InputField roomName;
	
	public void CreateRoom()
	{
		if(PhotonNetwork.IsConnected)
		{
			PhotonNetwork.CreateRoom(roomName.text, new RoomOptions(){MaxPlayers = 2});
			roomName.text = "";
		}
	}
}
