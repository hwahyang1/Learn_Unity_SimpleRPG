using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*
 * [Class] PlayerAttack
 * 플레이어가 공격을 할 때의 판정을 관리합니다.
 */
public class PlayerAttack : MonoBehaviour
{
	private PlayerController player;

	private void Start()
	{
		player = GetComponentInParent<PlayerController>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Monster" && other.name == "Body")
		{
			player.SetMonsterControl(other.GetComponentInParent<MonsterController>());
		}
	}
}
