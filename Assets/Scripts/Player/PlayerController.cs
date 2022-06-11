using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/*
 * [Class] PlayerController
 * 플레이어의 움직임을 관리합니다.
 */
public class PlayerController : MonoBehaviour
{
	[Header("플레이어 기본 속성")]
	[SerializeField]
	private float health = 100f;

	[Header("플레이어 움직임 설정")]
	[SerializeField, Range(0f, 10f)]
	private float moveSpeed = 5f;

	[SerializeField, Range(0f, 10f)]
	private float rotationSpeed = 5f;

	private Animator ani;
	private Rigidbody rig;
	private Slider slider;

	private void Start()
	{
		ani = GetComponent<Animator>();
		rig = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if (health > 0)
		{
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");

			float mouseX = Input.GetAxisRaw("mouseX");
		}
		else
		{
			ani.SetTrigger("Die");
		}

		rig.angularVelocity = Vector3.zero; // 다른 GameObject와 부딪혀서 의도치 않게 움직이는 현상 방지
	}
}
