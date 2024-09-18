using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour, IGameView
{
	public Guid playerID;

	[SerializeField]
	private TextMeshProUGUI scoreText;
	[SerializeField]
	private Image turnHighlight;

	public void OnGameStateUpdate(GameState state)
	{
		var score = state.board.ToList()
		.Select(pair => pair.Value)
		.Where(v => v != null)
		.Count(v => v.Value.ownerID == playerID);
		
		scoreText.text = score.ToString();
		turnHighlight.gameObject.SetActive(state.currentPlayerID == playerID);
	}
}
