using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class HandController : MonoBehaviour, IGameView
{
	public Guid playerID;

	private SkyStone currentlyHolding = null;
	private Vector3 lastMousePosition = new Vector3();

 	public void OnGameStateUpdate(GameState state)
	{
		if(!PhotonNetwork.InRoom)
		{
	   		this.enabled = state.currentPlayerID == playerID;
		}
	}

	private void LateUpdate()
	{
		if (Input.GetMouseButtonDown(0))
		{
			TryToGrabStone();
			currentlyHolding?.FloatAround(false);
		}
		else if(Input.GetMouseButton(0))
		{
			TryToMoveStone();
			Vector3 deltaPosition = lastMousePosition - Input.mousePosition;
			currentlyHolding?.GrabUpdate(deltaPosition);
		}
		else if (Input.GetMouseButtonUp(0))
		{
			TryToPlaceStone();
			currentlyHolding?.FloatAround(true);
			currentlyHolding = null;
		}

		lastMousePosition = Input.mousePosition;
	}

	private bool TryToGrabStone()
	{
		if(currentlyHolding != null) return false;

		SkyStone stone = GetObjectUnderMouseOfType<SkyStone>();
		if (stone != null && stone.CanGrab(playerID))
		{
			currentlyHolding = stone;
			return true;
		}

		return false;
	}

	private bool TryToPlaceStone()
	{
		if (currentlyHolding == null) return false;

		BoardTileView tile = GetObjectUnderMouseOfType<BoardTileView>();
		if (tile != null)
		{
			Game.PlayStone(currentlyHolding.stoneID, new Vector2Int(tile.x, tile.y));
			return true;
		}
		
		return false;
	}

	private bool TryToMoveStone()
	{
		if (currentlyHolding == null) return false;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		foreach (RaycastHit hit in Physics.RaycastAll(ray, Mathf.Infinity))
		{
			SkyStone stone = hit.transform.GetComponent<SkyStone>();
			if (stone != currentlyHolding)
			{
				currentlyHolding.transform.position = hit.point;
				currentlyHolding.targetPosition = hit.point;
				return true;
			}
		}

		return false;
	}

	private T GetObjectUnderMouseOfType<T>()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		foreach (RaycastHit hit in Physics.RaycastAll(ray, Mathf.Infinity))
		{
			T target = hit.transform.GetComponent<T>();
			if (target != null)
			{
				return target;
			}
		}

		return default;
	}
}
