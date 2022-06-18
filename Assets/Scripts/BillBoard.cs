using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*
 * [Class] BillBoard
 * 몬스터의 BillBoard를 관리합니다.
 */
public class BillBoard : MonoBehaviour
{
	private Transform cam;

	private void Start()
	{
		cam = Camera.main.transform;
	}

	private void Update()
	{
		transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
	}
}
