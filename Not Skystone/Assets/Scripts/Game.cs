using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviourPunCallbacks
{
	private GameModel model;

	private static Game instance;

	[SerializeField]
	private List<GameObject> customViewUpdateOrder;
	[SerializeField]
	private HandController enemyHand;
	[SerializeField]
	private PauseScreen pauseScreen;
	[SerializeField]
	private VictoryView victory;

	private GameState? lastState;

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this.gameObject);
		}
	}
	
	public static void RestartOffline()
	{
		instance.enemyHand.enabled = true;
		
		instance.model = new GameModel();
		instance.model.GameStateChanged += instance.OnNewGameState;
		instance.model.GenerateGameState();

		instance.pauseScreen.gameObject.SetActive(true);
	}
	
	public static void RestartOnline()
	{
		instance.photonView.RPC("OnRestartOnlineGameOnline", RpcTarget.AllBuffered);
		
		instance.pauseScreen.gameObject.SetActive(true);
	}
	
	public static void PlayStone(Guid stoneID, Vector2Int position)
	{
		if(PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient)
		{
			instance.photonView.RPC("OnPlaySkyStoneOnline", RpcTarget.OthersBuffered, stoneID.ToString(), position.x, position.y);
		}
		else
		{
			instance.model.PlaceSkyStone(stoneID, position);
		}
	}
	
	public static void Concede()
	{
		if(instance.lastState != null)
		{
			if(PhotonNetwork.InRoom)
			{
				instance.photonView.RPC("PerformConcede", RpcTarget.AllBuffered, instance.lastState.Value.players.First().Key.ToString());
			}
			else
			{
				instance.PerformConcede(instance.lastState.Value.currentPlayerID.ToString());
			}
		}
	}
	
	private void OnNewGameState(GameState state)
	{
		lastState = state;
		
		customViewUpdateOrder.ForEach(v => v.GetComponent<IGameView>().OnGameStateUpdate(state));

		var leftOverViews = FindObjectsOfType<MonoBehaviour>(true)
		.Where(m => !customViewUpdateOrder.Contains(m.gameObject))
		.OfType<IGameView>();
		
		foreach(IGameView view in leftOverViews)
		{
			view.OnGameStateUpdate(state);
		}
	}
	
	[PunRPC]
	private void OnPlaySkyStoneOnline(string stoneID, int x, int y)
	{
		instance.model.PlaceSkyStone(Guid.Parse(stoneID), new Vector2Int(x, y));
	}
	
	[PunRPC]
	private void OnNewGameStateOnline(string serializedGameState)
	{
		var state = GameState.DeSerialize(serializedGameState);
		if(!PhotonNetwork.IsMasterClient)
		{
			state = state.FlipSides();
		}
		
		OnNewGameState(state);
	}
	
	[PunRPC]
	private void OnRestartOnlineGameOnline()
	{
		enemyHand.enabled = false;
		pauseScreen.gameObject.SetActive(true);
		victory.ResetRematchVotes();
		
		if(PhotonNetwork.IsMasterClient)
		{
			model = new GameModel();
			model.GameStateChanged += (state) => instance.photonView.RPC("OnNewGameStateOnline", RpcTarget.AllBuffered, state.Serialize());
			model.GenerateGameState();
		}
	}
	
	[PunRPC]
	private void PerformConcede(string player)
	{
		model?.Concede(Guid.Parse(player));
	}
}
