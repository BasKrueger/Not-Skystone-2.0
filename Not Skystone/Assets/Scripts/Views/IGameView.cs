using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameView
{
    void OnGameStateUpdate(GameState state);
}
