using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ConnectButton : MonoBehaviourPunCallbacks
{
	[SerializeField]
	private TextMeshProUGUI text;
	private Button button;
	
	void Awake()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(Clicked);
	}

	new void OnEnable() 
	{
		base.OnEnable();
		UpdateButton();
	}
	
	private void Clicked()
	{
		if(!PhotonNetwork.IsConnected)
		{
			PhotonNetwork.GameVersion = "0.0.1";
			PhotonNetwork.ConnectUsingSettings();
			text.text = "Connecting...";
		}
		else
		{
			PhotonNetwork.Disconnect();
		}
		
 		EventSystem.current.SetSelectedGameObject(null);
		button.interactable = false;
 	}
	
	public override void OnConnectedToMaster()
	{
		base.OnConnectedToMaster();
		PhotonNetwork.JoinLobby();
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		base.OnDisconnected(cause);
		UpdateButton();
	}

	public override void OnJoinedLobby()
	{
		base.OnJoinedLobby();
		UpdateButton();
	}

	private void UpdateButton()
	{
		if(button != null)
		{
			var colors = button.colors;
			colors.normalColor = PhotonNetwork.InLobby ? Color.yellow : Color.white;
			button.colors = colors;
			
			text.text = PhotonNetwork.InLobby ? "Disconnect from server" : "Connect to server";
			button.interactable = true;
		}
	}
}
