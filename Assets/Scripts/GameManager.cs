using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

/*
 * [Class] GameManager
 * 게임의 전반적인 실행을 관리합니다.
 */
public class GameManager : MonoBehaviour
{
	[Header("게임 오버 설정")]
	[SerializeField]
	private GameObject gameOver;

	[SerializeField]
	private GameObject playerPrefab;

	[Header("몬스터 스폰 설정")]
	[SerializeField]
	private List<Transform> spawnPoints = new List<Transform>();

	[SerializeField]
	private GameObject monsterPrefab;

	[SerializeField]
	private Transform monsterParent;

	[SerializeField]
	private int maxSpawnCount = 8; // 4의 배수면 상관 없음 (스폰 포인트가 4개여서)

	[SerializeField]
	private float maxCoolDown = 10f;

	private float nowCoolDown = 9f;

	private bool isMouseVisible = false;

	private PlayerController player;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

		Cursor.visible = isMouseVisible;
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		if (player != null && player.GetHealth() <= 0)
		{
			gameOver.SetActive(true);

			player = null;

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			isMouseVisible = !isMouseVisible;
			Cursor.visible = isMouseVisible;

			if (isMouseVisible)
			{
				Cursor.lockState = CursorLockMode.None;
			}
			else
			{
				Cursor.lockState = CursorLockMode.Locked;
			}
		}

		if (spawnPoints.Count > 0 & monsterPrefab != null & monsterParent != null & monsterParent.childCount < maxSpawnCount & !gameOver.activeInHierarchy)
		{
			if (nowCoolDown >= maxCoolDown)
			{
				nowCoolDown = 0f;

				int random = Random.Range(0, spawnPoints.Count);
				Instantiate(monsterPrefab, spawnPoints[random].position, Quaternion.identity, monsterParent);
			}
			else
			{
				nowCoolDown += Time.deltaTime;
			}
		}
	}

	/*
	 * [Method] OnRespawnButtonClicked(): void
	 * 버튼 클릭에 대한 이벤트를 처리합니다.
	 */
	public void OnRespawnButtonClicked()
	{
		for (int i = 0; i < monsterParent.childCount; i++)
		{
			Destroy(monsterParent.GetChild(i).gameObject);
		}

		GameObject playerObj = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		player = playerObj.GetComponent<PlayerController>();

		gameOver.SetActive(false);

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		Camera.main.gameObject.GetComponent<FollowCam>().target = playerObj.transform;
	}

	/*
	 * [Method] OnQuitButtonClicked(): void
	 * 버튼 클릭에 대한 이벤트를 처리합니다.
	 */
	public void OnQuitButtonClicked()
	{
		#if UNITY_EDITOR
			EditorApplication.ExecuteMenuItem("Edit/Play");
		#else
			Application.Quit();
		#endif
	}
}
