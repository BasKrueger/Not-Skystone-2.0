using System;
using System.Collections.Generic;
using System.Linq;

public class PlayerModel
{
	public Guid playerID { get; private set; } = Guid.NewGuid();
	
	private Dictionary<Guid, SkystoneModel> hand = new Dictionary<Guid, SkystoneModel>();
	
	public PlayerModel()
	{
		for(int i = 0;i < 5;i++)
		{
			AddStoneToHand();
		}
	}
	
	public void AddStoneToHand()
	{
		var stone = new SkystoneModel(playerID);
		hand.Add(stone.stoneID, stone);
	}
	
	public void RemoveStoneFromHand(Guid stoneID)
	{
		if(hand.ContainsKey(stoneID))
		{
			hand.Remove(stoneID);
		}
	}
	
	public SkystoneModel TryGetStoneInHand(Guid stoneID)
	{
		return hand.TryGetValue(stoneID);
	}
	
	public PlayerState GetState()
	{
		var handState = new List<SkystoneState>();
		hand.Values.ToList().ForEach(s => handState.Add(s.GetState()));

		return new PlayerState()
		{
			handStones = handState,
			playerID = this.playerID,
		};
	}
}