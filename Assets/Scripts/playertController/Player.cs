using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Organism
{
    [SerializeField]
    JoysTick tt;
    Organism o;
    Animator ant;

    Action stateAction;
    // Use this for initialization
    void Start()
    {
        //o = gameObject.AddComponent<Organism>();
        ant = GetComponent<Animator>();
        fixPos = (v) =>
        {
            ant.SetBool("InSky", false);
            transform.position = v;
        };
        inSkyAction = () => { ant.SetBool("InSky", true); };

        tt.JoystickBeginHandle = (t) =>
        {
        };

        tt.JoystickMoveHandle = (v, f) =>
        {
            var forward = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up) * v.normalized;
            var tttt = Quaternion.FromToRotation(Vector3.up, transform.up);
            if (forward != Vector3.zero)
            {

                transform.forward = forward;
                transform.rotation = tttt * transform.rotation;

            }
            speed = f;
            ant.SetFloat("Speed", f);
        };
        tt.JoystickEndHandle = (t) =>
        {
            speed = 0;
            ant.SetFloat("Speed", 0);
        };
    }


    private void OnAnimatorIK(int layerIndex)
    {

        //if (ant.GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash("Idle"))
    }

    float footOffest = 0.2f;
    void MoveIK()
    {
        var rv3 = ant.GetIKPosition(AvatarIKGoal.RightFoot) + transform.up*1.5f;
        var lv3 = ant.GetIKPosition(AvatarIKGoal.LeftFoot) + transform.up * 1.5f;
        ant.SetIKPositionWeight(AvatarIKGoal.RightFoot, ant.GetFloat("RightFoot"));
        if (Physics.Raycast(rv3, -transform.up, out rayHit, 3f))
        {
            rv3.y = rayHit.point.y + footOffest;
            ant.SetIKPosition(AvatarIKGoal.RightFoot, rv3);
        }

        ant.SetIKPositionWeight(AvatarIKGoal.LeftFoot, ant.GetFloat("LeftFoot"));
        if (Physics.Raycast(lv3 , -transform.up, out rayHit, 3f))
        {
            lv3.y = rayHit.point.y + footOffest;
            ant.SetIKPosition(AvatarIKGoal.LeftFoot, lv3);
        }
    }

    void IdleIK()
    {
        var rv3 = ant.GetIKPosition(AvatarIKGoal.RightFoot) + transform.up * 1.5f;
        var lv3 = ant.GetIKPosition(AvatarIKGoal.LeftFoot) + transform.up * 1.5f;
        ant.SetIKPositionWeight(AvatarIKGoal.RightFoot,1);
        if (Physics.Raycast(rv3, -transform.up, out rayHit, 3f))
        {
            rv3.y = rayHit.point.y + footOffest;
            ant.SetIKPosition(AvatarIKGoal.RightFoot, rv3);
        }

        ant.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
        if (Physics.Raycast(lv3, -transform.up, out rayHit, 3f))
        {
            lv3.y = rayHit.point.y + footOffest;
            ant.SetIKPosition(AvatarIKGoal.LeftFoot, lv3);
        }
    }

    void ClimbIK()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump(-10);
            ant.SetTrigger("Jump");
        }
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        transform.position += Time.deltaTime * transform.forward * speed * 10;
        //if (WallCheck() == false) transform.position += Time.deltaTime * transform.forward * speed * 10;
        //else
        //{
        //    var dis = Vector3.Distance(transform.position, rayHit.point);
        //    if (dis < 1.8f) transform.position -= transform.forward * Time.deltaTime * 10;
        //}

    }
}
