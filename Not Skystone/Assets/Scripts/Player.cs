using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour, IGameView 
{
	[SerializeField]
	private int index = -1;

	private HandController handcontroller;
	private HandView handView;
	private ScoreView scoreView;

	public void OnGameStateUpdate(GameState state)
	{
		var id = state.players.Values.ToArray()[index].playerID;

		handcontroller.playerID = id;
		handView.playerID = id;
		scoreView.playerID = id;
	}

	private void Awake()
	{
		handcontroller = GetComponentInChildren<HandController>();
		handView = GetComponentInChildren<HandView>();
		scoreView = GetComponentInChildren<ScoreView>();

		if (index == -1)
		{
			Debug.LogError("Error: Playerindex not set");
		}
	}
}
