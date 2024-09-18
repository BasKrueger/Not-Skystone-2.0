using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class StartScreen : MonoBehaviour, INetworkSubscriber
{
	public void NetworkAwake()
	{
		NetworkEvents.LeftRoom += () => this.gameObject.SetActive(true);
	}

	public void StartOffline()
	{
		Game.RestartOffline();
		this.gameObject.SetActive(false);
	}
}
