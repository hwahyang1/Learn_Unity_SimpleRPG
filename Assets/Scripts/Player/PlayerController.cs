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

			float mouseX = Input.GetAxisRaw("Mouse X");

			int nowAni = ani.GetCurrentAnimatorStateInfo(0/*Layer Order*/).fullPathHash;

			// 회전
			transform.Rotate(new Vector3(0f, mouseX, 0f) * rotationSpeed);

			if (Input.GetMouseButtonDown(0))
			{
				ani.SetTrigger("Attack 1");
				ani.SetBool("Run", false);
			}

			// 공격 중이면 이동 차단
			if (nowAni != Animator.StringToHash("Base Layer.Attack 1"))
			{
				// 이동
				if (h != 0f || v != 0f)
				{
					ani.SetBool("Run", true);

					if (nowAni == Animator.StringToHash("Base Layer.Idle") || nowAni == Animator.StringToHash("Base Layer.Run"))
					{
						transform.Translate(new Vector3(h * moveSpeed * Time.deltaTime, 0f, v * moveSpeed * Time.deltaTime));
					}
				}
				else
				{
					ani.SetBool("Run", false);
				}
			}
		}
		else
		{
			ani.SetTrigger("Die");
		}

		rig.angularVelocity = Vector3.zero; // 다른 GameObject와 부딪혀서 의도치 않게 움직이는 현상 방지
	}

	/*
	 * [Method] GetDamaged(float damage): void
	 * 플레이어가 데미지를 받으면 이벤트를 발생합니다.
	 * 
	 * <float damage>
	 * 플레이어의 체력을 얼마나 깎을지 결정합니다.
	 */
	public void GetDamaged(float damage)
	{
		if (health > 0)
		{
			health -= damage;
			ani.SetTrigger("Damaged");
		}
	}

	/*
	 * [Method] GetHealth(): float
	 * 플레이어의 체력을 반환합니다.
	 * 
	 * <RETURN: float>
	 * 체력을 반환합니다.
	 */
	public float GetHealth()
	{
		return health;
	}
}
