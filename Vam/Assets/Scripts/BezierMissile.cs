using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniVRM10;

public class BezierMissile : MonoBehaviour
{
    Vector3[] point = new Vector3[4];       //4개의 베지어 컨트롤 포인트를 저장할 배열 
    bool hit = false;                       //미사일이 목표에 도달했는지 여부를 나타내는 변수

    [SerializeField][Range(0f, 1f)] private float t = 0;    //베지어 곡선에서의 시간 변수 
    [SerializeField] public float spd = 5;                  //미사일의 이동속도
    [SerializeField] public float posA = 0.55f;             //al미사일 궤적을 제어하는 파라미터 A
    [SerializeField] public float posB = 0.45f;             //al미사일 궤적을 제어하는 파라미터 B

    public GameObject master;           //미사일의 시작 위치를 나타내는 게임 오브젝트
    public GameObject enemy;            //미사일의 목표 위치를 나타내는 게임 오브젝트

    public void Start()
    {
        point[0] = master.transform.position;                   //시작위치
        point[1] = PointSetting(master.transform.position);     //랜덤 컨트롤
        point[2] = PointSetting(enemy.transform.position);      //랜덤 컨트롤
        point[3] = enemy.transform.position;                    //목표위치

    }
    private void FixedUpdate()
    {
        if (t > 1) return;      //시간이 1을 초과하면 미사일 이동 중지
        if (hit) return;        //목표에 도달하면 미사일 이동 중지
        t += Time.deltaTime * spd;      //시간 업데이트
        DrawTrajectory();           //미사일 궤적 업데이트
    }

    Vector3 PointSetting(Vector3 origin)
    {
        float x, y, z;

        x = posA * Mathf.Cos(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.x;
        y = posB * Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.y;
        z = posA * Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.z;

        return new Vector3(x, y, z);
    }

    void DrawTrajectory()   //미사일의 현재 위치를 베지어 곡선을 사용하여 업데이트 
    {
        transform.position = new Vector3(
            FourPointBezier(point[0].x, point[1].x, point[2].x, point[3].x),
            FourPointBezier(point[0].y, point[1].y, point[2].y, point[3].y),
            FourPointBezier(point[0].z, point[1].z, point[2].z, point[3].z)
        );
    }

    private float FourPointBezier(float a, float b, float c, float d) //4개의 컨트롤 포인틀를 사용해서 베지어 곡선에서의 값을 계산 
    {       
        return Mathf.Pow((1 - t), 3) * a + Mathf.Pow((1 - t), 2) * 3 * t * b + Mathf.Pow(t, 2) * 3 * (1 - t) * c + Mathf.Pow(t, 3) * d;
    }

}
