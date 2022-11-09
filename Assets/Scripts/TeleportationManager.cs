using Oculus.Interaction;
using UnityEngine;

public class TeleportationManager : MonoBehaviour
{
	//teleportation positioning
	[SerializeField] private GameObject rig;
	[SerializeField] private Transform head;
	[SerializeField] private Transform[] leftFingertips;

	[SerializeField] private float maxRayCastDistance = Mathf.Infinity;
	[SerializeField] private LayerMask teleportLayerMask;
	[SerializeField] private GameObject teleportPreview;
	[SerializeField] private AudioTrigger audioTrigger;
	[SerializeField] private AudioSource teleportSound;

	[SerializeField] private float teleportConfirmationTime;

	private bool _aiming, _preTeleporting;
	private GameObject _teleportLocation, _oldTeleportLocation;

	private void Update()
	{
		if (!_aiming || _preTeleporting) return;
		RayCastAim();
	}

	private void RayCastAim()
	{
		var rayCastDirection = Vector3.Lerp(leftFingertips[0].position, leftFingertips[1].position, 0.5f) - head.position;

		if (!Physics.Raycast(head.position, rayCastDirection, out var hit, maxRayCastDistance, teleportLayerMask))
		{
			DisablePreview();
			return;
		}

		EnablePreview(hit);
	}

	private void EnablePreview(RaycastHit hit)
	{
		_teleportLocation = hit.transform.gameObject;
		teleportPreview.SetActive(true);
		teleportPreview.transform.position = hit.transform.position;

		if (_teleportLocation == _oldTeleportLocation) return;
		
		_oldTeleportLocation = _teleportLocation;
		audioTrigger.PlayAudio();
	}

	private void DisablePreview()
	{
		_teleportLocation = null;
		_oldTeleportLocation = null;
		teleportPreview.SetActive(false);
	}

	private void StopAiming()
	{
		_aiming = false;
		_preTeleporting = false;
		DisablePreview();
	}

	public void SetAiming(bool setTrue)
	{
		if (setTrue) _aiming = true;
		else
		{
			_preTeleporting = true;
			Invoke(nameof(StopAiming), teleportConfirmationTime);
		}
	}

	public void TeleportToLocation()
	{
		if (!_aiming || _teleportLocation == null) return;

		rig.transform.position = _teleportLocation.transform.position;
		teleportSound.Play();
		StopAiming();
	}
}