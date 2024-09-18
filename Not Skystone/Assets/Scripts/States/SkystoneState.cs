using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public struct SkystoneState
{
	private static string CLASSKEY = "SkyStone";
	
   	public Dictionary<Vector2Int, int> spikes;
	private static string SPIKESKEY = "Spikes";

	public Guid stoneID;
	public static string STONEIDKEY = "Stone";
	public Guid ownerID;
	private static string OWNERIDKEY = "Owner";

 	public string Serialize()
	{
		var result = $"{CLASSKEY}(";

		//stone ID
		result += $"{STONEIDKEY}({stoneID}){STONEIDKEY}";
		//owner ID
		result += $"{OWNERIDKEY}({ownerID}){OWNERIDKEY}";

		//spikes
		foreach(var pair in spikes)
		{
			result += $"{SPIKESKEY}[{pair.Key.x}|{pair.Key.y}/{pair.Value}]{SPIKESKEY}";
		}

		result += $"){CLASSKEY}";

		return result;
	}

	public static SkystoneState DeSerialize(string text)
	{
		var result = new SkystoneState();

		var stone = text.GetSubStringBetweenKey(CLASSKEY);
		
		//stone ID
		result.stoneID = Guid.Parse(stone.GetSubStringBetweenKey(STONEIDKEY));
		//owner ID
		result.ownerID = Guid.Parse(stone.GetSubStringBetweenKey(OWNERIDKEY));
		//spikes
		result.spikes = new Dictionary<Vector2Int, int>();
				
		foreach(var pair in stone.GetAllSubStringsBetween($"{SPIKESKEY}[", $"]{SPIKESKEY}"))
		{
			var value = pair.Split("/")[1];
			var key = pair.Split("/")[0];

			var x = int.Parse(key.Split("|")[0]);
			var y = int.Parse(key.Split("|")[1]);

			result.spikes.Add(new Vector2Int(x, y), int.Parse(value));					
		}

		return result;
	}
}
