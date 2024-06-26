using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Snowflake : MonoBehaviour
{
    public float endPosX;
    public float endPosY;

    // SnowStorm effect
    [SerializeField] private float size;
    [SerializeField] private int opacity;
    [SerializeField] private int speed;


    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer snowRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        snowRenderer.color = new Color( 1f, 1f, 1f, Random.Range(0.25f, 1f));


        speed = Random.Range( 15, 31);

        size = Random.Range(0.1f, 0.25f);
        transform.localScale = new Vector3(transform.localScale.x * size, transform.localScale.y * size, 1); 

        StartCoroutine(LerpMove(Random.Range(-endPosX, endPosX), endPosY));
    }


    private IEnumerator LerpMove(float p_endPosX, float p_endPosY)
    {
        float t = 0;
        float _distance = Vector3.Distance(transform.position, new Vector3(p_endPosX, p_endPosY, 0));

        var dir = new Vector3(p_endPosX,p_endPosY) - transform.position;

        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

        while (_distance >= 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(p_endPosX, p_endPosY, 0), t);

            // We re-calculate the distance
            _distance = Vector3.Distance(transform.position, new Vector3(p_endPosX, p_endPosY, 0));

            yield return new WaitForEndOfFrame();

            t = speed * Time.deltaTime / _distance;
        }

        if(_distance <= 0.1f) Destroy(gameObject);

    }
}
