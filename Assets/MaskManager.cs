using UnityEngine;

public class MaskManager : MonoBehaviour
{
    [SerializeField] private Transform fingertip;
    [SerializeField] private GameObject lightParticle;

    public bool active;
    public float _radius, _softness;
    // Update is called once per frame
    private void Start()
    {
        active = true;
    }

    void Update()
    {
        if (!active) return;
        
        var fingerPos = fingertip.position;
        lightParticle.transform.position = fingerPos;
        
        Vector4 pos = new Vector4(fingerPos.x, fingerPos.y, fingerPos.z, 0);
        Shader.SetGlobalFloat("GLOBAL_MASK_Radius", _radius);
        Shader.SetGlobalFloat("GLOBAL_MASK_Softness", _softness);
        Shader.SetGlobalVector("GLOBAL_MASK_Position", pos);
    }
}
