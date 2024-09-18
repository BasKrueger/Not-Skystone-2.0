using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkystoneModel 
{
	public Guid stoneID { get; private set; } = Guid.NewGuid();
	public Guid ownerID{ get; private set; }
	
	private Dictionary<Vector2Int, int> spikes;

	public SkystoneModel(Guid ownerID, int north, int east, int south, int west)
	{
		this.ownerID = ownerID;
		
		spikes.Add(Vector2Int.up, north);
		spikes.Add(Vector2Int.right, east);
		spikes.Add(Vector2Int.down, south);
		spikes.Add(Vector2Int.left, west);
	}
	
	public SkystoneModel(Guid ownerID)
	{
		this.ownerID = ownerID;

		List<Dictionary<Vector2Int, int>> randomPresets = new List<Dictionary<Vector2Int, int>>
		{
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 3 }, { Vector2Int.right, 1 }, { Vector2Int.down, 1 }, {Vector2Int.left,3 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 3 }, { Vector2Int.right, 1 }, { Vector2Int.down, 3 }, {Vector2Int.left,1 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 1 }, { Vector2Int.right, 3 }, { Vector2Int.down, 1 }, {Vector2Int.left,3 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 3 }, { Vector2Int.right, 3 }, { Vector2Int.down, 0 }, {Vector2Int.left,0 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 0 }, { Vector2Int.right, 0 }, { Vector2Int.down, 4 }, {Vector2Int.left,3 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 3 }, { Vector2Int.right, 3 }, { Vector2Int.down, 3 }, {Vector2Int.left,3 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 3 }, { Vector2Int.right, 2 }, { Vector2Int.down, 2 }, {Vector2Int.left,2 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 2 }, { Vector2Int.right, 3 }, { Vector2Int.down, 2 }, {Vector2Int.left,2 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 2 }, { Vector2Int.right, 2 }, { Vector2Int.down, 2 }, {Vector2Int.left,3 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 2 }, { Vector2Int.right, 2 }, { Vector2Int.down, 3 }, {Vector2Int.left,2 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 2 }, { Vector2Int.right, 0 }, { Vector2Int.down, 1 }, {Vector2Int.left,1 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 0 }, { Vector2Int.right, 2 }, { Vector2Int.down, 1 }, {Vector2Int.left,1 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 0 }, { Vector2Int.right, 0 }, { Vector2Int.down, 3 }, {Vector2Int.left,0 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 1 }, { Vector2Int.right, 0 }, { Vector2Int.down, 1 }, {Vector2Int.left,1 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 3 }, { Vector2Int.right, 3 }, { Vector2Int.down, 3 }, {Vector2Int.left,3 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 4 }, { Vector2Int.right, 0 }, { Vector2Int.down, 0 }, {Vector2Int.left,0 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 0 }, { Vector2Int.right, 0 }, { Vector2Int.down, 4 }, {Vector2Int.left,0 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 0 }, { Vector2Int.right, 0 }, { Vector2Int.down, 0 }, {Vector2Int.left,4 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 0 }, { Vector2Int.right, 4 }, { Vector2Int.down, 0 }, {Vector2Int.left,0 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 2 }, { Vector2Int.right, 0 }, { Vector2Int.down, 2 }, {Vector2Int.left,2 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 2 }, { Vector2Int.right, 2 }, { Vector2Int.down, 1 }, {Vector2Int.left,1 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 2 }, { Vector2Int.right, 0 }, { Vector2Int.down, 1 }, {Vector2Int.left,1 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 1 }, { Vector2Int.right, 2 }, { Vector2Int.down, 1 }, {Vector2Int.left,1 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 2 }, { Vector2Int.right, 3 }, { Vector2Int.down, 1 }, {Vector2Int.left,3 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 1 }, { Vector2Int.right, 3 }, { Vector2Int.down, 1 }, {Vector2Int.left,1 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 2 }, { Vector2Int.right, 0 }, { Vector2Int.down, 1 }, {Vector2Int.left,3 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 0 }, { Vector2Int.right, 2 }, { Vector2Int.down, 3 }, {Vector2Int.left,1 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 2 }, { Vector2Int.right, 2 }, { Vector2Int.down, 1 }, {Vector2Int.left,1 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 1 }, { Vector2Int.right, 1 }, { Vector2Int.down, 3 }, {Vector2Int.left,2 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 1 }, { Vector2Int.right, 1 }, { Vector2Int.down, 2 }, {Vector2Int.left,2 } },
			new Dictionary<Vector2Int, int> { { Vector2Int.up, 4 }, { Vector2Int.right, 0 }, { Vector2Int.down, 1 }, {Vector2Int.left,1 } },
		};

		this.spikes = randomPresets[UnityEngine.Random.Range(0, randomPresets.Count)];
	}
	
	public void Attack(SkystoneModel other, Vector2Int direction)
	{
		if(other != null)
		{
			if(this.ownerID != other.ownerID && this.spikes[direction] > other.spikes[direction * - 1])
			{
				other.ownerID = this.ownerID;
			}
		}
	}
	
	public SkystoneState GetState()
	{
		return new SkystoneState()
		{
			spikes = new Dictionary<Vector2Int, int>(this.spikes),
			stoneID = this.stoneID,
			ownerID = this.ownerID
		};
	}
}
