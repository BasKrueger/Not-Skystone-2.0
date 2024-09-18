using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class JoinButton : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI nameText;

	private RoomInfo info;
	
	public void SetUp(RoomInfo room)
	{
		this.info = room;
		nameText.text = room.Name;
	}
	
	public void Clicked()
	{
		PhotonNetwork.JoinRoom(info.Name);
	}
}
