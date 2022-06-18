using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

/*
 * [Class] MonsterController
 * 몬스터의 움직임을 관리합니다.
 */
public class MonsterController : MonoBehaviour
{
	[Header("몬스터 속성")]
	[SerializeField]
	private float maxHealth = 100f;

	[SerializeField]
	private float maxCooldown = 2f;

	[SerializeField]
	private float damage = 5f;

	[SerializeField]
	private float attackDist = 2.5f;

	[SerializeField]
	private float destroyDelay = 2.5f;

	[Header("몬스터 체력바 오브젝트")]
	[SerializeField]
	private GameObject hpFront;

	private float nowHealth = 100f;
	private float nowCooldown = 0f;

	private float maxWidth = 1.2f;

	private GameObject target;

	private Animator ani;
	private Rigidbody rig;
	private NavMeshAgent nav;

	private RectTransform rect;

	private void Start()
	{
		nowHealth = maxHealth;

		ani = GetComponent<Animator>();
		rig = GetComponent<Rigidbody>();
		nav = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		float nowWidth = maxWidth * (nowHealth / maxHealth);

		if (rect == null)
		{
			rect = hpFront.GetComponent<RectTransform>();
		}

		float sizeY = rect.sizeDelta.y;
		rect.sizeDelta = new Vector2(nowWidth, sizeY);

		if (nowHealth <= 0f)
		{
			ani.SetTrigger("Die");

			if (nav.enabled)
			{
				nav.enabled = false;
				rig.isKinematic = true;

				StartCoroutine("MonsterDead");
			}

			return;
		}
		else
		{
			if (target != null)
			{
				nav.isStopped = false;
				nav.SetDestination(target.transform.position);

				ani.SetBool("Run", true);

				if (target.GetComponent<PlayerController>().GetHealth() > 0)
				{
					float dist = Vector3.Distance(target.transform.position, transform.position);

					if (dist <= attackDist)
					{
						if (nowCooldown >= maxCooldown)
						{
							nowCooldown = 0f;

							ani.SetTrigger("Attack");
							StartCoroutine("AttackPlayer");
						}
						else
						{
							nowCooldown += Time.deltaTime;
						}
					}
					else
					{
						StopCoroutine("AttackPlayer");
					}
				}
				else
				{
					target = null;
				}
			}
			else
			{
				nav.isStopped = true;
				ani.SetBool("Run", false);
			}

			rig.velocity = Vector3.zero;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && other.name == "Player")
		{
			target = other.gameObject;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player" && other.name == "Player")
		{
			target = null;
		}
	}

	private IEnumerator MonsterDead()
	{
		yield return new WaitForSeconds(destroyDelay);
		Destroy(gameObject);
	}

	private IEnumerator AttackPlayer()
	{
		yield return new WaitForSeconds(0.5f);
		target.GetComponent<PlayerController>().GetDamaged(damage);
	}

	/*
	 * [Method] GetDamaged(float damage): void
	 * 플레이어가 데미지를 받았을 때 체력을 깎습니다.
	 * 
	 * <float damage>
	 * 플레이어의 체력을 얼마나 깎을지 결정합니다.
	 */
	public void GetDamaged(float damage)
	{
		if (nowHealth > 0)
		{
			ani.SetTrigger("Damaged");
			nowHealth -= damage;
			nowCooldown = 0f;
		}
	}
}
