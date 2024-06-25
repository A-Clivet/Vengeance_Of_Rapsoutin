using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class S_SnowStorm : MonoBehaviour
{
    //trajectory ( combined to give snow a trajectory ) 
    [SerializeField] private float startingPosX; // minimum starting pos X over the camera ( used in the starting pos of a snowflake ) 
    [SerializeField] private float finishingPosX; // maximum starting pos X under the camera ( used in the finishing pos of a snowflake ) 

    [SerializeField] private float startEndPosY; // serves as the value when going outside of camera range, goes for positive as well as negative

    [SerializeField] private Vector3 spawnPos;

    //Randomness to get different visuals and make the effect prettier
    [SerializeField] private int size;
    [SerializeField] private int opacity;
    [SerializeField] private int speed;

    //When and if snow should spawn
    [SerializeField] private float spawnTimer;
    [SerializeField] private bool stormActive;


    //Visuals
    [SerializeField] private GameObject snow;
    [SerializeField] private Sprite snowSprite;
    [SerializeField] private SpriteRenderer snowRenderer;

    public void Start()
    {
        snowRenderer.sprite = snowSprite;
    }

    public Vector3 SpawnPosition()
    {
        startingPosX = Random.Range( startingPosX, 0); //  creates a snowflake on the upper-left corner of the screen

        startingPosX = startEndPosY; // just above the maximum Y for the camera view to make sure the snow flake can't be seen appearing

        return spawnPos = new Vector3(startingPosX, startEndPosY, 0.0f );
    }

    //strats the snow cycle
    public void StartSnowstorm() // function to turn on the snow, made in case more needs to be added
    {
        stormActive = true;
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
                GameObject spawnedSnow = Instantiate(snow);
                S_Snowflake snowScript = spawnedSnow.GetComponent<S_Snowflake>();

                spawnedSnow.transform.position = SpawnPosition();

                snowScript.endPosX = finishingPosX;
                snowScript.endPosY = -startEndPosY;
            }
        }
    }
}
