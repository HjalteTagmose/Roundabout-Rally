using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public float speed = 5;
    private Camera cam;
    private Shakeable shake;

	protected override void Awake()
	{
        base.Awake();
		cam = GetComponent<Camera>();
		shake = GetComponent<Shakeable>();
	}

	void LateUpdate()
    {
        var targetPos = RoadManager.Instance.curRoad.transform.position;
        var targetSize= RoadManager.Instance.curRoad.roadDiameter;
        targetPos.z = -10;

		var newPos = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
        transform.position = newPos;
		shake.Offset = newPos;
		cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, speed * Time.deltaTime);
	}

	public void Shake(float stress)
	{
		shake.InduceStress(stress);
	}
}
