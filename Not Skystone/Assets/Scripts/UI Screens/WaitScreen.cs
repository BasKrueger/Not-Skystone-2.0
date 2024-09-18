using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class WaitScreen : MonoBehaviour, INetworkSubscriber
{
	[SerializeField]
	private TextMeshProUGUI title;
	[SerializeField]
	private StartScreen startScreen;
	
	public void NetworkAwake()
	{
		NetworkEvents.JoinedRoom += OnJoinedRoom;
		NetworkEvents.LeftRoom += () => this.gameObject.SetActive(false);
		NetworkEvents.AnotherJoinedRoom += TryStartGame;
	}
	
	private void OnJoinedRoom()
	{
		this.gameObject.SetActive(true);
		title.text = $"Waiting for another player to join\n\n\"{PhotonNetwork.CurrentRoom.Name}\"";
		TryStartGame();
	}
	
	public void CancelClicked()
	{
		PhotonNetwork.LeaveRoom();
	}
	
	private void TryStartGame()
	{
		if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
		{
			Game.RestartOnline();
			PhotonNetwork.CurrentRoom.IsVisible = false;
			this.gameObject.SetActive(false);
			startScreen.gameObject.SetActive(false);
		}
	}
}
