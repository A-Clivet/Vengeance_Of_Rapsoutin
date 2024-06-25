using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Snowflake : MonoBehaviour
{
    public float endPosX;
    public float endPosY;
    public int speed;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LerpMove(endPosX, endPosY));
    }


    private IEnumerator LerpMove(float p_endPosX, float p_endPosY)
    {
        float t = 0;
        float _distance = Vector3.Distance(transform.position, new Vector3(p_endPosX, p_endPosY, 0));

        while (_distance >= 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(p_endPosX, p_endPosY, 0), t);

            // We re-calculate the distance
            _distance = Vector3.Distance(transform.position, new Vector3(p_endPosX, p_endPosY, 0));

            yield return new WaitForEndOfFrame();

            t = speed * Time.deltaTime / _distance;
        }

        Destroy(gameObject);

    }
}
