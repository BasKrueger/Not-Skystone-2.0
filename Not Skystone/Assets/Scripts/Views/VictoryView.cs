using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class VictoryView : MonoBehaviourPunCallbacks, IGameView
{
	[SerializeField]
	private TextMeshProUGUI victoryText;
	[SerializeField]
	private StartScreen mainMenu;
	[SerializeField]
	private PauseScreen pauseScreen;
	[SerializeField]
	private TextMeshProUGUI rematchText;

	private bool wantRematch = false;
	private bool opponentWantsRematch = false;

	public void OnGameStateUpdate(GameState state)
	{
		this.gameObject.SetActive(state.wonPlayerID != null);
		
		if(state.wonPlayerID != null)
		{
			pauseScreen.gameObject.SetActive(false);
			UpdateRematchText();
			
			if(!PhotonNetwork.InRoom)
			{
				victoryText.text = $"Player {1 + state.players.Keys.ToList().IndexOf(state.wonPlayerID.Value)} Won";
			}
			else
			{
				victoryText.text = (state.players.Keys.ToList().IndexOf(state.wonPlayerID ?? Guid.NewGuid()) == 0 ? "You" : "Your Opponent") + " Won";
			}
		}
	}

	[PunRPC]
	public void OnResetPressed()
	{
		if(PhotonNetwork.InRoom)
		{
			if(opponentWantsRematch)
			{
				Game.RestartOnline();
			}
			else
			{
				wantRematch = !wantRematch;
				photonView.RPC("RematchVote", RpcTarget.OthersBuffered, wantRematch);
			}
		}
		else
		{
			Game.RestartOffline();
		}

		UpdateRematchText();
	}
	
	public void OnMainMenuPressed()
	{
		if(PhotonNetwork.InRoom)
		{
			PhotonNetwork.LeaveRoom();
		}
		
		mainMenu.gameObject.SetActive(true);
		this.gameObject.SetActive(false);
	}
	
	[PunRPC]
	public void RematchVote(bool state)
	{
		opponentWantsRematch = state;
		UpdateRematchText();
	}
	
	public void ResetRematchVotes()
	{
		wantRematch = false;
		opponentWantsRematch = false;
	}
	
	private void UpdateRematchText()
	{
		if(!PhotonNetwork.InRoom)
		{
			rematchText.text = "Restart";
		}
		else
		{
			rematchText.text = wantRematch || opponentWantsRematch ? "Rematch (1/2)" : "Rematch (0/2)";
		}
	}
}
