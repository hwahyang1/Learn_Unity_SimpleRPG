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

	private float nowHealth = 100f;
	private float nowCooldown = 0f;

	private GameObject target;

	private Animator ani;
	private Rigidbody rig;
	private NavMeshAgent nav;

	private void Start()
	{
		nowHealth = maxHealth;

		ani = GetComponent<Animator>();
		rig = GetComponent<Rigidbody>();
		nav = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		if (nowHealth <= 0f)
		{
			if (nav.enabled)
			{
				nav.enabled = false;
				rig.isKinematic = true;

				ani.SetTrigger("Die");

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
}
