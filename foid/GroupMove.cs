using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupMove : MonoBehaviour
{
    public Transform[] boids;
    public Transform head;
    void Update()
    {
        FoidMove();
    }

    void FoidMove()
    {
        //每帧预计算统一朝向力
        Vector3 wholeForward = Vector3.zero;
        for(int i = 0;i < boids.Length; i++)
        {
            wholeForward += boids[i].forward;
        }
        wholeForward /= boids.Length;
        //向心力，所有点的中心点
        Vector3 center = Vector3.zero;
        for(int i = 0;i < boids.Length; i++)
        {
            center += boids[i].position;
        }
        center /= boids.Length;
        //离心力：所有物体到此物体的方向(单位向量)/距离
        Vector3 breakForce = Vector3.zero;
        for (int b = 0; b < boids.Length; b++)
        {
            breakForce = Vector3.zero;
            for (int i = 0;i < boids.Length; i++)
            {
                if (b == i)
                    continue;
                Vector3 neighborToThis = boids[b].position - boids[i].position;
                if(neighborToThis.magnitude < 2f)
                    breakForce += neighborToThis.normalized * 1000000f;
                else
                    breakForce += neighborToThis.normalized / neighborToThis.magnitude;
            }
            breakForce /= (boids.Length - 1);
            Vector3 centerForce = center - boids[b].position;
            if(centerForce.magnitude > 10f)
                centerForce = centerForce.normalized;
            else
                centerForce = Vector3.zero;
            Vector3 targetForce = head.position - boids[b].position;
            Vector3 nextForward = breakForce + centerForce + targetForce;

            Vector3 verticalForward = Vector3.Cross(boids[b].forward.normalized, nextForward.normalized);
            boids[b].RotateAround(boids[b].position, verticalForward.normalized, 40f * Time.deltaTime);
            boids[b].transform.position += boids[b].forward.normalized * Time.deltaTime * 5f;
        }
    }
}
