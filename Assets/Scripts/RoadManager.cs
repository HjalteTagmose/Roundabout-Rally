using System.Collections.Generic;
using UnityEngine;

public class RoadManager : Singleton<RoadManager>
{	
	public Road roadPrefab;
	[Space(5)]
	public Road curRoad;
	public Transform car;
	public ExitSign exitSign;

	private List<Road> pastRoads;

	private void Start()
	{
		pastRoads = new List<Road>();
		PlayerController.Instance.OnAfterTakeExit += SpawnNewRoad;
		curRoad.Setup(2, false);
	}

	public void SpawnNewRoad()
	{
		var player = PlayerController.Instance;

		pastRoads.Add(curRoad);
		foreach (var pastRoad in pastRoads)
		{
			pastRoad.DecreaseVisibility();
		}

		var road = Instantiate(roadPrefab, transform);
		road.Setup(Random.Range(2.7f, 5f), !player.clockwise);

		var carPos = car.position;
		var curPos = curRoad.transform.position;
		var dir	   = (curPos - carPos).normalized;
		var offset = curRoad.roadDiameter / 2 + road.roadDiameter / 2 - Road.roadWidth;

		road.transform.position = curPos - dir * offset;
		player.transform.position = road.transform.position;
		car.position = carPos;
		curRoad = road;

		player.IncreaseSpeed();
		exitSign.SetPosition(road);
	}

	private void Update()
	{
		float playerAngle = PlayerController.Instance.transform.eulerAngles.z;
		curRoad.UpdateZone(playerAngle);
	}

	public bool CanTakeExit()
	{
		float playerAngle = PlayerController.Instance.transform.eulerAngles.z;
		return curRoad.IsInZone(playerAngle);
	}
}