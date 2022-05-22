using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Control : MonoBehaviour
{
    public bool StopCam = false;

    private GameObject Players,cenaObj;
    private Vector3 cam_pos;
    float valMIn = 0;
    static public Camera_Control canCont;
    public bool cena = false;
    Player plaey;


    void Start()
    {
        canCont = this;
        valMIn = transform.position.x;
        cenaObj = GameObject.Find("Foco_Cam");
    }

    // Update is called once per frame
    void Update()
    {
        if(!Players)
        {

            Players = GameObject.FindGameObjectWithTag("Player");
        }
        if(Players && StopCam == false && cena == false)
        {
            FollowCan();
        }
        if(cena)
        {
            FollowCena();
        }

    }  
    void FollowCena()
    {     
        if (cenaObj)
        {
            cam_pos = new Vector3(cenaObj.transform.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.Slerp(transform.position, cam_pos, 1.5f * Time.deltaTime);
        }
    }
    void FollowCan()
    {
        valMIn = transform.position.x;
        if (Players)
        {
            cam_pos = new Vector3(Players.transform.position.x, transform.position.y, transform.position.z);

            if (valMIn < (Players.transform.position.x - 2))
            {
                transform.position = Vector3.Slerp(transform.position, cam_pos, 2 * Time.deltaTime);
            }
        }
        if (!plaey)
        {
            plaey = Players.GetComponent<Player>();           
        }
        if(plaey)
        {
            plaey.AbreDialogo = false;
        }
    }
}
