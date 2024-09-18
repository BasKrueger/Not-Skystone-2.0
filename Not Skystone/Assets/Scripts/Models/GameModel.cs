using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameModel
{
	public event Action<GameState> GameStateChanged;

	private PlayerModel currentPlayer;

	private Dictionary<Vector2Int, SkystoneModel> board = new Dictionary<Vector2Int, SkystoneModel>();
	private List<PlayerModel> players = new List<PlayerModel>();
	private Guid? wonPlayerID = null;
	
	public GameModel()
	{
		players = new List<PlayerModel>() { new PlayerModel(), new PlayerModel() };
		currentPlayer = players[UnityEngine.Random.Range(0,2)];
		
		for(int i = 0;i < 3;i++)
		{
			for(int j =0; j < 3;j++)
			{
				board.Add(new Vector2Int(i, j), null);
			}
		}

		GenerateGameState();
	}
	
	public void PlaceSkyStone(Guid stoneID, Vector2Int slot)
	{
		var stone = currentPlayer.TryGetStoneInHand(stoneID);
		
		if(stone != null && stone.ownerID == currentPlayer.playerID)
		{
			if(board.ContainsKey(slot) && board[slot] == null)
			{
				board[slot] = stone;

				stone.Attack(board.TryGetValue(slot + Vector2Int.up), Vector2Int.up);
				stone.Attack(board.TryGetValue(slot + Vector2Int.right), Vector2Int.right);
				stone.Attack(board.TryGetValue(slot + Vector2Int.down), Vector2Int.down);
				stone.Attack(board.TryGetValue(slot + Vector2Int.left), Vector2Int.left);
				
				currentPlayer.RemoveStoneFromHand(stoneID);

				CheckVictory();
				SwitchActivePlayer();
				GenerateGameState();
			}
		}
	}
	
	public void Concede(Guid playerID)
	{
		if(players[0].playerID == playerID)
		{
			wonPlayerID = players[1].playerID;
		}
		if(players[1].playerID == playerID)
		{
			wonPlayerID = players[0].playerID;
		}

		GenerateGameState();
	}
	
	private void SwitchActivePlayer()
	{
		var nextPlayerIndex = players.IndexOf(currentPlayer) + 1;
		if(nextPlayerIndex > players.Count - 1)
		{
			nextPlayerIndex = 0;
		}

		currentPlayer = players[nextPlayerIndex];
	}
	
	private void CheckVictory()
	{
		if(!board.Values.Any(v => v == null))
		{
			var placedStones = board.Values.ToList();
			var winningScore = 0;
			
			foreach(var player in players)
			{
				var score = placedStones.Count(v => v.ownerID == player.playerID);
				
				if(score > winningScore)
				{
					winningScore = score;
					wonPlayerID = player.playerID;
				}
			}
		}
	}
	
	public void GenerateGameState()
	{
		var playerStates = new Dictionary<Guid, PlayerState>();
		players.ForEach(p => playerStates.Add(p.playerID, p.GetState()));

		var boardStates = new Dictionary<Vector2Int, SkystoneState?>();
		board.ToList().ForEach(pair => boardStates.Add(pair.Key, pair.Value?.GetState()));
		
		var state = new GameState()
		{
			players = playerStates,
			board = boardStates,
			currentPlayerID = currentPlayer.playerID,
			wonPlayerID = wonPlayerID,
		};

		GameStateChanged?.Invoke(state);
	}
}

public static partial class Extensions
{
	public static TV TryGetValue<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV defaultValue = default)
	{
		TV value;
		return dict.TryGetValue(key, out value) ? value : defaultValue;
	}
}
