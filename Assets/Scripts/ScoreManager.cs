using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
	public event Action OnGameOver;

	public float multiplier = 1;
	public float multiplierMax = 5;
	public float maxScore = 1000;
	public float score = 0;
	[Space(5)]
	public Color negativeScore;
	public Color positiveScore;
	public Color overlayColor;
	[Space(5)]
	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI newScoreText;
	public Image overlay;
	public Button restartButton;

	private Vector2 newScoreStartPos;

	private void Start()
	{
		scoreText.text = "0";
		PlayerController.Instance.OnBeforeTakeExit += DoScore;
		newScoreStartPos = newScoreText.rectTransform.anchoredPosition;
		overlay.color = Color.clear;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			Restart();
		}
	}

	private void DoScore()
	{
		var road = RoadManager.Instance.curRoad;
		float playerAngle = PlayerController.Instance.transform.eulerAngles.z;
		float scoreAdj = 0;

		if (road.IsInZone(playerAngle))
			scoreAdj = maxScore * multiplier;
		else
			scoreAdj = -road.GetAngleDifference(playerAngle) * 2 * multiplier;

		score += scoreAdj;

		bool scoreIsPositive = scoreAdj > 0;
		scoreText.text = score.ToString();
		newScoreText.text = scoreAdj.ToString();
		multiplier = Mathf.Lerp(multiplier, multiplierMax, 0.1f);

		if (!scoreIsPositive)
		{
			CameraManager.Instance.Shake(.3f);
			OnGameOver?.Invoke();
			AnimateFinalScore();
			return;
		}

		AnimateNewScore(scoreIsPositive);
	}

	private void AnimateNewScore(bool isPositive)
	{
		var newScoreRT = newScoreText.transform as RectTransform;

		newScoreText.DOKill();
		newScoreText.color = isPositive ? positiveScore : negativeScore;
		newScoreText.DOColor(Color.clear, .5f).SetEase(Ease.InFlash);

		newScoreRT.DOKill();
		if (isPositive)
		{
			newScoreRT.anchoredPosition = newScoreStartPos + Vector2.down * 50;
			newScoreRT.DOAnchorPos(newScoreStartPos, .5f).SetEase(Ease.InFlash);
		}
        else
        {
			newScoreRT.anchoredPosition = newScoreStartPos;
			newScoreRT.DOAnchorPos(newScoreStartPos + Vector2.down * 80, .5f).SetEase(Ease.InFlash);
		}
    }

	private void AnimateFinalScore()
	{
		var scoreRT = scoreText.transform as RectTransform;
		scoreRT.DOKill();
		overlay.DOKill();
		restartButton.DOKill();

		scoreRT.SetTop(0);
		scoreRT.SetBottom(0);
		scoreRT.DOAnchorMin(new Vector2(0.2f, 0), 1).SetEase(Ease.OutElastic, .1f, 0f);
		scoreRT.DOAnchorMax(new Vector2(0.8f, 1), 1).SetEase(Ease.OutElastic, .1f, 0f);
		overlay.DOColor(overlayColor, 1).SetEase(Ease.OutExpo);
		restartButton.GetComponent<RectTransform>().DOAnchorPos(Vector2.up * 270, 0.8f).SetDelay(0.2f).SetEase(Ease.OutExpo);
	}

	public void Restart()
	{
		Application.LoadLevel(0);
	}
}
