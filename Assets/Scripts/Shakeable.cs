using UnityEngine;

public class Shakeable : MonoBehaviour
{
	public Vector2 Offset { get; set; }

	[SerializeField] private float trauma = 0;
	[SerializeField] private float recoverySpeed = 1.5f;
	[SerializeField] private float frequency = 25;
	[Space(5)]
	[SerializeField] private Vector2 maxTranslationShake = Vector3.one * 0.5f;
	[SerializeField] private float maxAngularShake = 2;
	[Space(5)]
	[SerializeField] private float zOffset = 0;
		
	private bool canShake = true;
	private float seed;
	private float shake = 0;

	protected virtual void Start()
	{
		seed = Random.value;
	}

	protected virtual void Update()
	{
		if (!canShake)
			return;

		shake = Mathf.Pow(trauma, 2);

		transform.localPosition = Offset + new Vector2(
			maxTranslationShake.x * (Mathf.PerlinNoise(seed	  , Time.time * frequency) * 2 - 1),
			maxTranslationShake.y * (Mathf.PerlinNoise(seed + 1, Time.time * frequency) * 2 - 1)
		) * shake;

		Vector3 localPos = transform.localPosition;
		localPos.z = zOffset;
		transform.localPosition = localPos;

		transform.localRotation = Quaternion.Euler(new Vector3(
			0, 0, maxAngularShake * (Mathf.PerlinNoise(seed + 2, Time.time * frequency) * 2 - 1)
		) * shake);

		trauma = Mathf.Clamp01(trauma - recoverySpeed * Time.deltaTime);
	}

	public void InduceStress(float stress)
	{
		trauma = Mathf.Clamp01(trauma + stress);
	}

	public void Set(bool on)
	{
		canShake = on;
	}
}
