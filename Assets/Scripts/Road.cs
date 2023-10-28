using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Road : MonoBehaviour
{
	public static float roadWidth = .65f;
	public static float arc = 300;

	public float roadDiameter = 2;
	private SpriteMask mask;
	private SpriteRenderer sr;
	public SpriteRenderer greenZone;
	private float relArc;
	private float offset;

	private void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
		mask = GetComponentInChildren<SpriteMask>();
		greenZone = this.GetComponentInChildrenExclusive<SpriteRenderer>();
	}

	public void Setup(float roadDiameter, bool offset180)
	{
		this.roadDiameter = roadDiameter;
		var innerDiameter = roadDiameter - roadWidth * 2;

		transform.localScale = Vector3.one * roadDiameter;
		mask.transform.localScale = Vector3.one * innerDiameter/roadDiameter;

		relArc = arc / roadDiameter;
		var propBlock = new MaterialPropertyBlock();
		greenZone.GetPropertyBlock(propBlock);
		propBlock.SetFloat("_Arc2", 360 - relArc);
		greenZone.SetPropertyBlock(propBlock);
		offset = Random.Range(0f, 179);

		greenZone.transform.eulerAngles = Vector3.forward * (offset + (offset180 ? 180 : 0));
	}

	public bool IsInZone(float angle)
	{
		angle -= offset;
		if (angle < 0)
			angle = 360 + angle;

		return angle > 360 - relArc && angle < 360;
	}

	public float GetAngleDifference(float angle)
	{
		angle -= offset;
		if (angle < 0)
			angle = 360 + angle;

		float ang = 360 - relArc;
		float add = 360 - ang;
		float mid = ang + add / 2;

		return Mathf.Abs(angle - mid); 
	}

	public void UpdateZone(float playerAngle)
	{
		bool inZone = IsInZone(playerAngle);
		greenZone.SetAlpha(inZone ? .75f : .3f);
	}

	public void DecreaseVisibility()
	{
		if (greenZone)
			Destroy(greenZone);

		sr.SetAlpha(sr.color.a - .2f);
	}

	public float GetRelativeAngle()
	{
		return relArc;
	}
}
