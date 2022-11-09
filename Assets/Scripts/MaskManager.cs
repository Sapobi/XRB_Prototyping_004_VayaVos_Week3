using UnityEngine;

public class MaskManager : MonoBehaviour
{
	[SerializeField] private Transform fingertip;
	[SerializeField] private GameObject lightParticle;
	[SerializeField] private AudioSource sound;

	private bool _isActive;
	private float _radius = 0.4f;
	private float _softness = 0.1f;

	private void Start()
	{
		Shader.SetGlobalFloat("GLOBAL_MASK_Radius", 0);
		Shader.SetGlobalFloat("GLOBAL_MASK_Softness", _softness);
	}

	void Update()
	{
		if (!_isActive) return;

		var fingerPos = fingertip.position;
		lightParticle.transform.position = fingerPos;
		Vector4 pos = new Vector4(fingerPos.x, fingerPos.y, fingerPos.z, 0);
		Shader.SetGlobalVector("GLOBAL_MASK_Position", pos);

		Debug.Log(Shader.GetGlobalFloat("GLOBAL_MASK_Radius"));
	}

	public void SetActive(bool active)
	{
		_isActive = active;
		sound.mute = !active;
		lightParticle.SetActive(active);

		if (active) Shader.SetGlobalFloat("GLOBAL_MASK_Radius", _radius);
		else Shader.SetGlobalFloat("GLOBAL_MASK_Radius", 0);
	}
}