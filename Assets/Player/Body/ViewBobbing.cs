using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
[RequireComponent(typeof(PositionFollow))]
public class ViewBobbing : MonoBehaviour
{
    public float EffectIntensityY;
    public float EffectIntensityX;
    public float EffectSpeed;

    public PositionFollow followerInstance;
    private Vector3 originalOfsset;
    private float sinTime;

    // Start is called before the first frame update
    void Start()
    {
        //followerInstance = GetComponent<PositionFollow>();
        originalOfsset = followerInstance.offset;
    }
    public void ProcessViewBobbing(Vector2 input)
    {
        if(input.magnitude >0f)
        {
            sinTime += Time.deltaTime * EffectSpeed;
            float sinAmountY = -Mathf.Abs(EffectIntensityY * Mathf.Sin(sinTime));
            Vector3 sinAmountX = followerInstance.transform.right * EffectIntensityY * Mathf.Cos(sinTime) * EffectIntensityX;

            followerInstance.offset = new Vector3
            {
                x = originalOfsset.x,
                y = originalOfsset.y + sinAmountY,
                z = originalOfsset.z,
            };
            followerInstance.offset += sinAmountX;

        }
        else
            sinTime = 0f;

    }
}
