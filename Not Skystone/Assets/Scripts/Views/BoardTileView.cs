using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardTileView : MonoBehaviour, IGameView
{
	[field: Header("position")]
	[field: SerializeField]
	public int x { get; private set; } = -1;
	[field:SerializeField]
	public int y { get; private set; } = -1;

	[Header("References")]
	[SerializeField]
	private SkyStone SkyStoneTemplate;
	private SkyStone activeSkyStone;
	private MeshRenderer mesh;
	private SkystoneState? previousStoneState = null;
	private Color defaultColor;

	private void Awake()
	{
		mesh = GetComponent<MeshRenderer>();
		defaultColor = mesh.material.color;
	}

	public void OnGameStateUpdate(GameState state)
	{
		if (x == -1 || y == -1) return;

		SkystoneState? stoneState = state.board[new Vector2Int(x,y)];
		var stoneColor = GetCurrentColor(stoneState, state.players.Values.ToList());

		TryToDestroyInvalidStone(stoneState);
		TryToSpawnStone(stoneState, stoneColor);
		TryToUpdateStone(stoneState, stoneColor);
		
		previousStoneState = stoneState;
	}

	private Color GetCurrentColor(SkystoneState? stoneState, List<PlayerState> playerStates)
	{
		if(stoneState == null)
		{
			return Color.gray;
		}

		return stoneState.Value.ownerID == playerStates.First().playerID ? Color.blue : Color.red;
	}

	private bool TryToSpawnStone(SkystoneState? state, Color stoneColor)
	{
		if(state == null || activeSkyStone != null)
		{
			return false;
		}

		activeSkyStone = Instantiate(SkyStoneTemplate);
		activeSkyStone.SetUp(state.Value, false);
		activeSkyStone.PlayEntryAnimation();

		activeSkyStone.transform.position = transform.position + new Vector3(0, 0.5f, 0);
		activeSkyStone.targetPosition = transform.position + new Vector3(0, 0.5f, 0);
		activeSkyStone.transform.SetParent(transform);

		activeSkyStone.GetComponentInChildren<RandomMovement>().enabled = false;
		mesh.material.color = stoneColor;
		return true;
	}
	
	private bool TryToUpdateStone(SkystoneState? state, Color stoneColor)
	{
		if (previousStoneState != null && previousStoneState.Value.ownerID != state.Value.ownerID)
		{
			StartCoroutine(PlayDelayedAnimation());
			return true;
		}

		return false;

		IEnumerator PlayDelayedAnimation()
		{
			yield return new WaitForSeconds(0.75f);
			activeSkyStone?.PlaySwitchAnimation();
			yield return new WaitForSeconds(0.75f);
			mesh.material.color = stoneColor;
		}
	}
	
	private bool TryToDestroyInvalidStone(SkystoneState? state)
	{
		if (state == null)
		{
			if(activeSkyStone != null) Destroy(activeSkyStone.gameObject);
			mesh.material.color = defaultColor;
			previousStoneState = null;
			return true;
		}

		return false;
	}
}
