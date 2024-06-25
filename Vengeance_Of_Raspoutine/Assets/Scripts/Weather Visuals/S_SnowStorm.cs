using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class S_SnowStorm : MonoBehaviour
{
    //trajectory ( combined to give snow a trajectory ) 
    [SerializeField] private float startingPosX; // minimum starting pos X over the camera ( used in the starting pos of a snowflake ) 
    [SerializeField] private float finishingPosX; // maximum starting pos X under the camera ( used in the finishing pos of a snowflake ) 

    [SerializeField] private float startEndPosY; // serves as the value when going outside of camera range, goes for positive as well as negative

    [SerializeField] private Vector3 spawnPos;

    //When and if snow should spawn
    [SerializeField] private float spawnTimer;
    [SerializeField] private bool stormActive;


    //GameObject
    [SerializeField] private GameObject snow;

    [SerializeField] private float spawnRate;

    public Vector3 SpawnPosition()
    {
        return spawnPos = new Vector3(Random.Range(startingPosX, -startingPosX), startEndPosY, 0.0f );
    }

    public IEnumerator DisableSnowstrom()
    {
        yield return new WaitForSeconds(3f);

        StopSnowstorm();
    }

    //strats the snow cycle
    public void StartSnowstorm() // function to turn on the snow, made in case more needs to be added
    {
        stormActive = true;

        StartCoroutine(DisableSnowstrom());
    }

    //end the snow cycle
    public void StopSnowstorm() // function to turn off the snow, made in case more needs to be added
    {
        stormActive = false;
    }

    void Update()
    { 
        if (stormActive)
        {
            spawnTimer -= Time.deltaTime;

            if(spawnTimer < 0)
            {
                SpawnPosition();

                GameObject spawnedSnow = Instantiate(snow, SpawnPosition(), Quaternion.AngleAxis(Mathf.Atan2(startingPosX - finishingPosX, startEndPosY + startEndPosY) * Mathf.Rad2Deg, Vector3.forward));
                S_Snowflake snowScript = spawnedSnow.GetComponent<S_Snowflake>();

                spawnTimer = Random.Range(spawnRate / 10f, spawnRate);

                snowScript.endPosX = finishingPosX;
                snowScript.endPosY = -startEndPosY;
            }
        }
    }
}
