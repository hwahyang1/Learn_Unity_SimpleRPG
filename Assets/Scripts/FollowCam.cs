using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*
 * [Class] FollowCam
 * 카메라가 플레이어를 따라가도록 처리합니다.
 */
public class FollowCam : MonoBehaviour
{
	public Transform target;

	[Range(0f, 10f)]
	public float distance = 10f;

	[Range(0f, 10f)]
	public float height = 5f;

	[Range(0f, 10f)]
	public float rotationSpeed = 5f;

	private void LateUpdate()
	{
		float angle = Mathf.LerpAngle(transform.eulerAngles.y, target.eulerAngles.y, rotationSpeed * Time.deltaTime);

		Quaternion rotation = Quaternion.Euler(0f, angle, 0f);

		transform.position = target.position - (rotation * Vector3.forward * distance) + (Vector3.up * height);
		transform.LookAt(target);
	}
}
