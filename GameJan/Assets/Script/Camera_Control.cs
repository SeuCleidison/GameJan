using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Control : MonoBehaviour
{
    public bool StopCam = false;

    private GameObject Players;
    private Vector3 cam_pos;
    float valMIn = 0;
    static public Camera_Control canCont;

    void Start()
    {
        canCont = this;
        valMIn = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Players)
        {
            Players = GameObject.FindGameObjectWithTag("Player");
        }
        if(Players && !StopCam)
        {
            FollowCan();
        }
    }

    void FollowCan()
    {
        valMIn = transform.position.x;
        if (Players)
        {
            cam_pos = new Vector3(Players.transform.position.x, transform.position.y, transform.position.z);

            if (valMIn < (Players.transform.position.x -2))
            {
                transform.position = Vector3.Slerp(transform.position, cam_pos, 2 * Time.deltaTime);
            }
        }
    }
}
