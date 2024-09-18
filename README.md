# Not-Skystones
<p align="center">
    <img src="readme/Banner.PNG" alt="Not Skystone Banner"><br>
</p>

This Project represents a revisited version of a previous project called ["Not Skystone"](https://github.com/BasKrueger/Not-Skystone). This Update also adds an online multiplayer mode using the Photon plugin.

# Highlight: Online Multiplayer


https://github.com/user-attachments/assets/5151959d-eb3a-44f3-baad-2b9bc6eda270


Since the project already made use of the Model-View-Controller code pattern I figured it's a nice starting point for experimenting with online multiplayer logic. For this purpose the project now makes use of the Photon plugin. 
Through a new UI the player is now able to host and join games. From a technical standpoint the gamelogic only runs on the Host, he basically becomes the server. If the other use wants to perform an action like playing a stone it now sends a RPC to the host, which then applies the action to its model. 
Any resulting gamestate then gets seriailzed and send to both players, where their Views then display the state.

On top of the multiplayer mode I also added supplementary features like
- Disconnect messages and automatic match closure when the other player leaves
- ability to concede against another player
- button to vote for a rematch

# General Improvements
While the previous iteration did work properly, in hindsight I wasn't completely satisified with the code architecture. Thus with this 2.0 release I rewrote large chunks of the previous gameplay code to increase readability and make the code easier to maintain. Take the following case for example:

[Now:](https://github.com/BasKrueger/Not-Skystone-2.0/blob/main/Not%20Skystone/Assets/Scripts/Models/SkystoneModel.cs)
```
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
```
[Previously:](https://github.com/BasKrueger/Not-Skystone/blob/main/Not%20Skystone/Assets/Scripts/Models/BoardModel.cs)
```
 private int TryToOvertakeNeighbours(SkystoneModel lastPlaced, int x, int y)
    {
        int score = 0;
        SkystoneModel target;
        if(x > 0)
        {
            target = tiles[x - 1, y];
            if (target != null && lastPlaced.westSpikes > target.eastSpikes && lastPlaced.TryToOvertake(target))
            {
                score++;
            }
        }

        if(x < 2)
        {
            target = tiles[x + 1, y];
            if(target != null && lastPlaced.eastSpikes > target.westSpikes && lastPlaced.TryToOvertake(target))
            {
                score++;
            }
        }

        if (y > 0)
        {
            target = tiles[x, y - 1];
            if (target != null && lastPlaced.northSpikes > target.southSpikes && lastPlaced.TryToOvertake(target))
            {
                score++;
            }
        }

        if (y < 2)
        {
            target = tiles[x, y +1];

            if (target != null && lastPlaced.southSpikes > target.northSpikes && lastPlaced.TryToOvertake(target))
            {
                score++;
            }
        }

        return score;
    }
```

# How to run
You can play the current version of it [here](https://suchti0352.itch.io/not-skystones). Alternativaly feel free to download the reposetory and open it with at least Unity 2022.3.
