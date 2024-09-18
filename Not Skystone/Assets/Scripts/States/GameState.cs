using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public struct GameState
{
	private static string CLASSKEY = "Game";
	
	public Dictionary<Guid, PlayerState> players;
	private static string PLAYERSKEY = "AllPlayers";
	public Dictionary<Vector2Int, SkystoneState?> board;
	private static string BOARDKEY = "Board";
	public Guid currentPlayerID;
	private static string CURRENTKEY = "CurrentPlayerID";
	public Guid? wonPlayerID;
	private static string WONKEY = "WonPlayerID";
	
	public GameState FlipSides()
	{
		var before = new Dictionary<Guid, PlayerState>(players).ToArray();
		
		players = new Dictionary<Guid, PlayerState>();
		players.Add(before[1].Key, before[1].Value);
		players.Add(before[0].Key, before[0].Value);
		
		return this;
	}
	
	public string Serialize()
	{
		var result = $"{CLASSKEY}(";

		//current player
		result += $"{CURRENTKEY}({currentPlayerID}){CURRENTKEY}";
		
		//won player
		result += $"{WONKEY}({wonPlayerID?.ToString() ?? "null"}){WONKEY}";

		//all players
		foreach(var pair in players)
		{
			result += $"{PLAYERSKEY}[{pair.Key}___{pair.Value.Serialize()}]{PLAYERSKEY}";
		}
		
		//board
		foreach(var pair in board)
		{
			var value = pair.Value?.Serialize() ?? "null";
			result += $"{BOARDKEY}[{pair.Key.x}|{pair.Key.y}___{value}]{BOARDKEY}";
		}

		return result + $"){CLASSKEY}";
	}

	public static GameState DeSerialize(string text)
	{
		var result = new GameState();

		var game = text.GetSubStringBetweenKey(CLASSKEY);
		
		//current player
		result.currentPlayerID = Guid.Parse(game.GetSubStringBetweenKey(CURRENTKEY));
		
		//won player
		var wonPlayer = game.GetSubStringBetweenKey(WONKEY);
		result.wonPlayerID = wonPlayer == "null" ? null : Guid.Parse(wonPlayer);
		
		//all players
		result.players = new Dictionary<Guid, PlayerState>();
		foreach(var pair in game.GetAllSubStringsBetween($"{PLAYERSKEY}[", $"]{PLAYERSKEY}"))
		{
			var value = pair.Split("___")[1];
			var key = pair.Split("___")[0];

			result.players.Add(Guid.Parse(key), PlayerState.DeSerialize(value));					
		}
		
		//board
		result.board = new Dictionary<Vector2Int, SkystoneState?>();
		foreach(var pair in game.GetAllSubStringsBetween($"{BOARDKEY}[", $"]{BOARDKEY}"))
		{
			var value = pair.Split("___")[1];
			var key = pair.Split("___")[0];
			
			var x = int.Parse(key.Split("|")[0]);
			var y = int.Parse(key.Split("|")[1]);

			result.board.Add(new Vector2Int(x,y), value != "null" ? SkystoneState.DeSerialize(value) : null);					
		}

		return result;
	}
}

public static partial class Extensions
{
	public static string Between(this string STR , string FirstString, string LastString)
	{       
		string FinalString;     
		int Pos1 = STR.IndexOf(FirstString) + FirstString.Length;
		int Pos2 = STR.IndexOf(LastString);
		FinalString = STR.Substring(Pos1, Pos2 - Pos1);
		return FinalString;
	}
	
	public static List<string> GetAllSubStringsBetween(this string text, string opener, string closer)
	{
		var result = new List<string>();
		
		while(text.Contains(opener) && text.Contains(closer))
		{
			var lastOpener = text.AllIndexesOf(opener).Last();
			var subString = text.Substring(lastOpener).GetSubStringBetween(opener, closer);
			result.Add(subString);

			text = text.Replace($"{opener}{subString}{closer}", "");
		}
		
		result.Reverse();
		return result;
	}
	
	public static IEnumerable<int> AllIndexesOf(this string text, string searchstring)
	{
		return Enumerable.Range(0, text.Length - searchstring.Length)
		.Where(i => searchstring.Equals(text.Substring(i, searchstring.Length)));
	}
	
	public static string GetSubStringBetween(this string text, string opener, string closer)
	{
		int Pos1 = text.IndexOf(opener) + opener.Length;
		int Pos2 = text.IndexOf(closer);
		return text.Substring(Pos1, Pos2 - Pos1);
	}
	
	public static string GetSubStringBetweenKey(this string text, string key)
	{
		return text.GetSubStringBetween($"{key}(",$"){key}");
	}
}