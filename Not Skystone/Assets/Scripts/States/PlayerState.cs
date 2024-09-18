using System;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerState 
{
	private static string CLASSKEY = "Player";
	
	public List<SkystoneState> handStones;
	private static string STONEKEY = "HandStones";
	public Guid playerID;
	private static string PLAYERIDKEY = "ID";
	
	public string Serialize()
	{
		var result = $"{CLASSKEY}(";

		//player ID
		result += $"{PLAYERIDKEY}({playerID}){PLAYERIDKEY}";

		//skystones
		foreach(var element in handStones)
		{
			result += $"{STONEKEY}[{element.Serialize()}]{STONEKEY}";
		}

		result += $"){CLASSKEY}";

		return result;
	}

	public static PlayerState DeSerialize(string text)
	{
		var result = new PlayerState();

		var player = text.GetSubStringBetweenKey(CLASSKEY);

		//player ID
		result.playerID = Guid.Parse(player.GetSubStringBetweenKey(PLAYERIDKEY));

		//skystones
		result.handStones = new List<SkystoneState>();		
		foreach(var element in player.GetAllSubStringsBetween($"{STONEKEY}[", $"]{STONEKEY}"))
		{
			result.handStones.Add(SkystoneState.DeSerialize(element));
		}

		return result;
	}
}
