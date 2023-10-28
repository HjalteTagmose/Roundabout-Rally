using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
	public event Action OnBeforeTakeExit;
	public event Action OnAfterTakeExit;

	public float maxSpeed = 1000;
	public float minSpeed = 200;
	public float speed = 200;
	public bool clockwise = true;

	private bool stopped = false;
    private float mult = 1;

	private void Start()
	{
		mult = 1 / RoadManager.Instance.curRoad.roadDiameter;
		ScoreManager.Instance.OnGameOver += Stop;
	}

	private void Update()
    {
		if (stopped)
			return;

        float dir = clockwise ? 1 : -1;
		float rot = dir * speed * mult * Time.deltaTime;
		transform.Rotate(Vector3.forward, rot);

        if (Input.GetMouseButtonDown(0))
            TakeExit();
    }

    private void TakeExit()
    {
        OnBeforeTakeExit?.Invoke();

		if (!RoadManager.Instance.CanTakeExit())
			return;

        clockwise = !clockwise;
        mult = 1 / RoadManager.Instance.curRoad.roadDiameter;
		OnAfterTakeExit?.Invoke();
	}

	public void IncreaseSpeed()
	{
        speed = Mathf.Lerp(speed, maxSpeed, 0.15f);
	}

	public void Stop()
	{
		stopped = true;
	}
}
