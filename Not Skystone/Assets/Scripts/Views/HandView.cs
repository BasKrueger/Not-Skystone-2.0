using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandView : MonoBehaviour, IGameView
{
	public Guid playerID;

	[SerializeField]
	private SkyStone stoneTemplate;
	private Dictionary<Guid,SkyStone> activeSkyStones = new Dictionary<Guid,SkyStone>();

	public void OnGameStateUpdate(GameState state)
	{
		var validStates = state.players[playerID].handStones;
		
		RemoveInvalidSkystones(validStates);
		SpawnMissingSkyStones(validStates);

		if (state.currentPlayerID == playerID)
		{
			ActivateStoneMovement();
		}
		else
		{
			DeActivateStoneMovement();
		}
	}

	private void Update()
	{
		var stones = activeSkyStones.Values.ToList();
		for (int i = 0; i < stones.Count; i++)
		{
			float percent = i / (float)stones.Count;
			stones[i].targetPosition = transform.position + new Vector3(0, 0, percent * 10f);
		}

		HighlightHoveredStone();
	}
	
	private void RemoveInvalidSkystones(List<SkystoneState> validStates)
	{
		foreach(var pair in new Dictionary<Guid, SkyStone>(activeSkyStones))
		{
			if(!validStates.Any(s => s.stoneID == pair.Key))
			{
				Destroy(pair.Value.gameObject);
				activeSkyStones.Remove(pair.Key);
			}
		}
	}
	
	private void SpawnMissingSkyStones(List<SkystoneState> validStates)
	{
		foreach(var state in validStates)
		{
			if(!activeSkyStones.ContainsKey(state.stoneID))
			{
				SkyStone instance = Instantiate(stoneTemplate);

				instance.transform.position = transform.position;
				instance.SetUp(state);
				instance.transform.SetParent(transform);
				
				activeSkyStones.Add(state.stoneID, instance);
			}
		}
	}
	private void HighlightHoveredStone()
	{
		foreach(SkyStone stone in activeSkyStones.Values)
		{
			stone.Highlight(false);
		}

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		foreach (RaycastHit hit in Physics.RaycastAll(ray, Mathf.Infinity))
		{
			SkyStone stone = hit.transform.GetComponent<SkyStone>();
			if (stone != null && stone.CanGrab(playerID))
			{
				stone.Highlight(true);
				return;
			}
		}
	}

	private void ActivateStoneMovement()
	{
		foreach(SkyStone stone in activeSkyStones.Values)
		{
			stone.FloatAround(true);
		}
	}

	private void DeActivateStoneMovement()
	{
		foreach (SkyStone stone in activeSkyStones.Values)
		{
			stone.FloatAround(false);
		}
	}
}
