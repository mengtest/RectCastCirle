using UnityEngine;
using UnityEngine.UI;

public class RectCastCirle : MonoBehaviour
{
    Vector2 A;
    Vector2 B;
    Vector2 C;
    Vector2 D;

    public Transform rect;
    public Transform cirle;
    public Button button;
    public Text message;

    public Vector2 ab;
    public Vector2 cd;
    public Vector2 o;
    public float l1;
    public float r;

    void Start()
    {
        button.onClick.AddListener(CheckHit);
    }

    /// <summary>
    /// 检测按钮
    /// </summary>
    public void CheckHit()
    {
        float l2 = Mathf.Sqrt(( cd.x - ab.x ) * ( cd.x - ab.x ) + ( cd.y - ab.y ) * ( cd.y - ab.y ));
        Debug.Log("宽:" + l2);

        float angel = Mathf.Atan2(cd.y - ab.y, cd.x - ab.x) * 180 / Mathf.PI;
        Debug.Log("旋转:" + angel);

        rect.localPosition = new Vector3(( ab.x + cd.x ) / 2, ( ab.y + cd.y ) / 2, 0);
        rect.localScale = new Vector3(l2, l1, 1);
        rect.localRotation = Quaternion.Euler(0, 0, angel);

        cirle.localPosition = new Vector3(o.x, o.y, 0);
        cirle.localScale = new Vector3(2 * r, 2 * r, 2 * r);

        if (_RectCastCirle(ab, cd, l1, l2, o, r))
        {
            message.text = "Hit";
        }
        else
        {
            message.text = "No Hit";
        }
    }

    /// <summary>
    /// 碰撞检测
    /// </summary>
    /// <param name="ab"></param>
    /// <param name="cd"></param>
    /// <param name="l1"></param>
    /// <param name="l2"></param>
    /// <param name="o"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    public bool _RectCastCirle(Vector2 _ab, Vector2 _cd, float l1, float l2, Vector2 o, float r)
    {
        bool flag = false;

        InitABCD(_ab, _cd, l1 / 2);

        Vector2 AB = new Vector2(B.x - A.x, B.y - A.y);
        Vector2 BC = new Vector2(C.x - B.x, C.y - B.y);
        Vector2 CD = new Vector2(D.x - C.x, D.y - C.y);
        Vector2 DA = new Vector2(A.x - D.x, A.y - D.y);

        Vector2 AO = new Vector2(o.x - A.x, o.y - A.y);
        Vector2 BO = new Vector2(o.x - B.x, o.y - B.y);
        Vector2 CO = new Vector2(o.x - C.x, o.y - C.y);
        Vector2 DO = new Vector2(o.x - D.x, o.y - D.y);

        int _rAB = Cross(AB, AO);
        int _rBC = Cross(BC, BO);
        int _rCD = Cross(CD, CO);
        int _rDA = Cross(DA, DO);

        int _rArea = GetArea(_rAB, _rBC, _rCD, _rDA);

        flag = _CheckHit(_rArea, r, o);

        return flag;
    }

    /// <summary>
    /// 得到矩形的四个顶点坐标
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="length"></param>
    public void InitABCD(Vector2 v1, Vector2 v2, float length)
    {
        float kx = v2.x - v1.x;
        float ky = v2.y - v1.y;

        if (kx == 0)
        {
            A = new Vector2(v1.x - length, v1.y);
            B = new Vector2(v1.x + length, v1.y);
            C = new Vector2(v2.x + length, v2.y);
            D = new Vector2(v2.x - length, v2.y);
        }
        else if(ky == 0)
        {
            A = new Vector2(v1.x, v1.y + length);
            B = new Vector2(v1.x, v1.y - length);
            C = new Vector2(v2.x, v2.y - length);
            D = new Vector2(v2.x, v2.y + length);
        }
        else
        {
            float k = -( ky / kx );

            float angel = Mathf.Abs(Mathf.Atan(k));
            Debug.Log(angel);


            float xLen = Mathf.Abs(Mathf.Sin(angel) * length);
            float yLen = Mathf.Abs(Mathf.Cos(angel) * length);

            Debug.Log(xLen);
            Debug.Log(yLen);

            if (v2.x > v1.x && v2.y > v1.y)
            {
                A = new Vector2(v1.x - xLen, v1.y + yLen);
                B = new Vector2(v1.x + xLen, v1.y - yLen);
                C = new Vector2(v2.x + xLen, v2.y - yLen);
                D = new Vector2(v2.x - xLen, v2.y + yLen);
            }
            else if(v2.x>v1.x && v2.y < v1.y)
            {
                A = new Vector2(v1.x + xLen, v1.y + yLen);
                B = new Vector2(v1.x - xLen, v1.y - yLen);
                C = new Vector2(v2.x - xLen, v2.y - yLen);
                D = new Vector2(v2.x + xLen, v2.y + yLen);
            }
            else if(v2.x<v1.x && v2.y > v1.y)
            {
                A = new Vector2(v1.x - xLen, v1.y - yLen);
                B = new Vector2(v1.x + xLen, v1.y + yLen);
                C = new Vector2(v2.x + xLen, v2.y + yLen);
                D = new Vector2(v2.x - xLen, v2.y - yLen);
            }
            else if(v2.x<v1.x && v2.y < v1.y)
            {
                A = new Vector2(v1.x + xLen, v1.y - yLen);
                B = new Vector2(v1.x - xLen, v1.y + yLen);
                C = new Vector2(v2.x - xLen, v2.y + yLen);
                D = new Vector2(v2.x + xLen, v2.y - yLen);
            }

        }
        Debug.Log("A点坐标:" + A.x + "," + A.y);
        Debug.Log("B点坐标:" + B.x + "," + B.y);
        Debug.Log("C点坐标:" + C.x + "," + C.y);
        Debug.Log("D点坐标:" + D.x + "," + D.y);
    }

    /// <summary>
    /// 计算叉乘,圆心在矢量左边返回-1，右边返回1
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    public int Cross(Vector2 v1, Vector2 v2)
    {
        float r = ( v1.x ) * ( v2.y ) - ( v1.y ) * ( v2.x );

        if (r > 0)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }

    /// <summary>
    /// 计算圆心所在区域
    /// </summary>
    /// <param name="rAB"></param>
    /// <param name="rBC"></param>
    /// <param name="rCD"></param>
    /// <param name="rDA"></param>
    /// <returns></returns>
    public int GetArea(int rAB, int rBC, int rCD, int rDA)
    {
        int area = 0;

        if (rAB == 1)
        {
            if (rBC == 1)
            {
                area = 7;
            }
            else
            {
                if (rDA == 1)
                {
                    area = 1;
                }
            }
            area = 4;
        }
        else
        {
            if (rBC == 1)
            {
                if (rCD == 1)
                {
                    area = 9;
                }
                else
                {
                    area = 8;
                }
            }
            else
            {
                if (rCD == 1)
                {
                    if (rDA == 1)
                    {
                        area = 3;
                    }
                    else
                    {
                        area = 6;
                    }
                }
                else
                {
                    if (rDA == 1)
                    {
                        area = 2;
                    }
                    else
                    {
                        area = 5;
                    }
                }
            }
        }

        Debug.Log("圆心在区域：" + area);

        return area;
    }

    /// <summary>
    /// 计算点到点的距离平方
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public float DistanceFromPointToPoint(Vector2 from, Vector2 to)
    {
        float dis = ( to.x - from.x ) * ( to.x - from.x ) + ( to.y - from.y ) * ( to.y - from.y );

        Debug.Log("圆心距离顶点距离" + Mathf.Sqrt(dis));

        return ( to.x - from.x ) * ( to.x - from.x ) + ( to.y - from.y ) * ( to.y - from.y );
    }

    /// <summary>
    /// 求点到直线距离
    /// </summary>
    /// <param name="v">点</param>
    /// <param name="v1">直线上的点</param>
    /// <param name="v2">直线上的点</param>
    /// <returns></returns>
    public float DistanceFromPointToLine(Vector2 v, Vector2 v1, Vector2 v2)
    {
        float dis = 0;

        if (v1.x == v2.x)
        {
            dis = Mathf.Abs(v.x - v1.x);
            Debug.Log("圆心距离边" + dis);
            return dis * dis;
        }

        float lineK = ( v2.y - v1.y ) / ( v2.x - v1.x );
        float lineC = ( v2.x * v1.y - v1.x * v2.y ) / ( v2.x - v1.x );

        dis = ( Mathf.Abs(lineK * v.x - v.y + lineC) / Mathf.Sqrt(lineK * lineK + 1) );
        Debug.Log("圆心距离边:" + dis);

        return dis * dis;
    }

    /// <summary>
    /// 根据圆心区域和圆半径判断碰撞
    /// </summary>
    /// <param name="rArea"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    public bool _CheckHit(int rArea, float r, Vector2 o)
    {
        bool flag = false;

        switch (rArea)
        {
            case 1:
                if (DistanceFromPointToPoint(o, A) <= ( r * r ))
                {
                    flag = true;
                }
                break;
            case 2:
                if (DistanceFromPointToLine(o, A, D) <= ( r * r ))
                {
                    flag = true;
                }
                break;
            case 3:
                if (DistanceFromPointToPoint(o, D) <= ( r * r ))
                {
                    flag = true;
                }
                break;
            case 4:
                if (DistanceFromPointToLine(o, A, B) <= ( r * r ))
                {
                    flag = true;
                }
                break;
            case 5:
                flag = true;
                break;
            case 6:
                if (DistanceFromPointToLine(o, C, D) <= ( r * r ))
                {
                    flag = true;
                }
                break;
            case 7:
                if (DistanceFromPointToPoint(o, B) <= ( r * r ))
                {
                    flag = true;
                }
                break;
            case 8:
                if (DistanceFromPointToLine(o, B, C) <= ( r * r ))
                {
                    flag = true;
                }
                break;
            case 9:
                if (DistanceFromPointToPoint(o, C) <= ( r * r ))
                {
                    flag = true;
                }
                break;
            default:
                Debug.Log("error");
                break;
        }

        return flag;
    }
}