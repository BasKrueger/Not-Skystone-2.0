using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class JoinGamePanel : MonoBehaviourPunCallbacks
{
	[SerializeField]
	private Transform roomContent;
	[SerializeField]
	private JoinButton buttonTemplate;
	[SerializeField]
	private GameObject noGameMessage;

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		foreach(Transform t in roomContent)
		{
			Destroy(t.gameObject);
		}
		
		foreach(var room in roomList.Where(room => room.PlayerCount == 1))
		{
			if(room.PlayerCount > 0)
			{
				var instance = Instantiate(buttonTemplate);
				instance.SetUp(room);
				instance.transform.SetParent(roomContent, false);
			}
		}
		
		noGameMessage.gameObject.SetActive(!roomList.Any(room => room.PlayerCount == 1));
	}
}
