using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedometer : MonoBehaviour
{
	public Transform indicator;

	private void Update()
	{
		var cur = PlayerController.Instance.speed;
		var max = PlayerController.Instance.maxSpeed;
		var speedPct = cur / max;
		indicator.eulerAngles = Vector3.forward * (90f * (1-speedPct));
	}
}
