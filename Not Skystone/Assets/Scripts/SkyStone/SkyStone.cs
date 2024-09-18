using UnityEngine;
using System;

public class SkyStone : MonoBehaviour
{
	[field:Header("Spikes")]
	[field:SerializeField]
	public SkyStoneSpike northSpikes { get; private set; }
	[field:SerializeField]
	public SkyStoneSpike eastSpikes { get; private set; }
	[field:SerializeField]
	public SkyStoneSpike southSpikes { get; private set; }
	[field:SerializeField]
	public SkyStoneSpike westSpikes { get; private set; }

	[HideInInspector]
	public Guid ownerID;
	[HideInInspector]
	public Guid stoneID;

	[HideInInspector]
	public Vector3 targetPosition = new Vector3();

	private RandomMovement movement;
	private Animator anim;

	private const float speed = 3;
	private const float rotationForce = 10;
	private bool highlightable;

	private void Awake()
	{
		movement = GetComponentInChildren<RandomMovement>();
		anim = GetComponent<Animator>();
	}

	public void SetUp(SkystoneState state, bool highlightable = true)
	{
		northSpikes.SetValue(state.spikes[Vector2Int.up]);
		eastSpikes.SetValue(state.spikes[Vector2Int.right]);
		southSpikes.SetValue(state.spikes[Vector2Int.down]);
		westSpikes.SetValue(state.spikes[Vector2Int.left]);

		this.stoneID = state.stoneID;
		this.ownerID = state.ownerID;
		this.highlightable = highlightable;
	}

	private void Update()
	{
		if (Vector3.Distance(transform.position, targetPosition) > 0.01f)
		{
			Vector3 direction = targetPosition - transform.position;
			transform.position += direction * speed * Time.deltaTime;
		}

		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, rotationForce * Time.deltaTime);
	}

	public void FloatAround(bool state)
	{
		movement.enabled = state;
		anim.enabled = state;
	}

	public void GrabUpdate(Vector3 mouseDelta)
	{
		transform.Rotate(new Vector3(-mouseDelta.y, 0, mouseDelta.x) * 0.25f);
	}

	public bool CanGrab(Guid currentPlayerID)
	{
		return currentPlayerID == ownerID;
	}

	public void Highlight(bool state)
	{
		anim.SetBool("highlighted", state && highlightable);
	}

	public void PlayEntryAnimation()
	{
		anim.Play("Entry");
	}

	public void PlaySwitchAnimation()
	{
		anim.Play("Switch");
	}

	#region Animation Events
	public void OnEntryAnimationFinish()
	{
		movement.enabled = false;
	}

	public void IncreaseIntensity()
	{
		movement.range = 0.05f;
		movement.durationLength = 0.25f;
		movement.enabled = true;
	}

	public void ResetIntensity()
	{
		movement.Reset();
	}
	#endregion
}
