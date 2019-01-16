using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organism : MonoBehaviour
{
    static float gravity = 36;
    static float MaxGravity = 18;

    protected RaycastHit rayHit = default(RaycastHit);
    float currentGravity = 0;

    public bool isInSky = false;

    /// <summary>
    /// 接觸到地面後修正高度
    /// </summary>
    public Action<Vector3> fixPos;

    /// <summary>
    /// 
    /// </summary>
    public Action inSkyAction;

    /// <summary>
   /// 
    /// </summary>
    public float speed = 0;

    /// <summary>
    /// 跳躍
    /// </summary>
    /// <param name="force">跳躍的強度越小跳躍高</param>
    /// <param name="action">跳躍的時候執行甚麼</param>
    public void Jump(float force)
    {
        currentGravity = force;
    }

    protected virtual void  FixedUpdate()
    {
        //WallCheck();
        GravityCheck();
        FixPlane();
    }


    /// <summary>
    /// 碰撞與牆壁檢查
    /// </summary>
    /// <returns></returns>
    protected bool WallCheck()
    {
        if (Mathf.Abs(0-speed)>0.01 && (Physics.Raycast(transform.position+ transform.up, transform.forward, out rayHit, 2f)||
            Physics.Raycast(transform.position + transform.up, Quaternion.AngleAxis(20, transform.up)*transform.forward, out rayHit, 2f)||
            Physics.Raycast(transform.position + transform.up, Quaternion.AngleAxis(-20, transform.up) * transform.forward, out rayHit, 2f)))
        {
            return Vector3.Angle(rayHit.point - transform.position, Vector3.up) >= 45;
        }
        else
        {
            return false;
        }
    }

    public void FixPlane()
    {
        if(Physics.Raycast(transform.position+ transform.up*0.3f, -transform.up, out rayHit, 0.5f))
        {
            Quaternion NextRot = Quaternion.LookRotation(Vector3.Cross(rayHit.normal, Vector3.Cross(transform.forward, rayHit.normal)), rayHit.normal);
            transform.rotation = NextRot;
        }
    }

    /// <summary>
    /// 重力判斷
    /// </summary>
    void GravityCheck()
    {
        if (currentGravity >= 0 && Physics.Raycast(transform.position + transform.up, Vector3.down , out rayHit, 1.5f))//射線判斷是否在地面
        {
            if (fixPos != null && isInSky == true)
            {
                fixPos(rayHit.point);
                FixPlane();
            } 
            isInSky = false;
            currentGravity = 0;
        }
        else//不在的話要受重力引響
        {
            if (isInSky == false) inSkyAction();
            isInSky = true;
            transform.position += Vector3.down * Time.deltaTime * currentGravity;
            currentGravity = currentGravity <= MaxGravity ? currentGravity + gravity * Time.deltaTime : MaxGravity;
        }
    }
}
