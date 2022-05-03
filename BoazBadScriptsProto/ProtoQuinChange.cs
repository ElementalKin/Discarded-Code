using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoQuinChange : MonoBehaviour
{
    public GameObject qinHead;
    public GameObject qinTorso;
    public GameObject qinLegs;
    public GameObject qinRArm;
    public GameObject qinLArm;
    public GameObject bearHead;
    public GameObject bearTorso;
    public GameObject bearLegs;
    public GameObject bearRArm;
    public GameObject bearLArm;
    public GameObject robotHead;
    public GameObject robotTorso;
    public GameObject robotBack;
    public GameObject robotLegs;
    public GameObject robotRArm;
    public GameObject robotLArm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EquipBearHead()
    {
        qinHead.SetActive(false);
        robotHead.SetActive(false);
        bearHead.SetActive(true);
    }
    public void EquipBearTorso()
    {
        qinTorso.SetActive(false);
        robotTorso.SetActive(false);
        bearTorso.SetActive(true);
    }
    public void EquipBearBack()
    {
        robotBack.SetActive(false);
    }
    public void EquipBearLegs()
    {
        qinLegs.SetActive(false);
        robotLegs.SetActive(false);
        bearLegs.SetActive(true);
    }
    public void EquipBearRArm()
    {
        qinRArm.SetActive(false);
        robotRArm.SetActive(false);
        bearRArm.SetActive(true);
    }
    public void EquipBearLArm()
    {
        qinLArm.SetActive(false);
        robotLArm.SetActive(false);
        bearLArm.SetActive(true);
    }
    public void EquipRobotHead()
    {
        qinHead.SetActive(false);
        robotHead.SetActive(true);
        bearHead.SetActive(false);
    }
    public void EquipRobotTorso()
    {
        qinTorso.SetActive(false);
        robotTorso.SetActive(true);
        bearTorso.SetActive(false);
    }
    public void EquipRobotBack()
    {
        robotBack.SetActive(true);
    }
    public void EquipRobotLegs()
    {
        qinLegs.SetActive(false);
        robotLegs.SetActive(true);
        bearLegs.SetActive(false);
    }
    public void EquipRobotRArm()
    {
        qinRArm.SetActive(false);
        robotRArm.SetActive(true);
        bearRArm.SetActive(false);
    }
    public void EquipRobotLArm()
    {
        qinLArm.SetActive(false);
        robotLArm.SetActive(true);
        bearLArm.SetActive(false);
    }

}
