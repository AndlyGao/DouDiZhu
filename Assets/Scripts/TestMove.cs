using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    public float speed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private float tempH;
    private float tempV;
    Vector3 dir = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        var h = Input.GetAxisRaw("Horizontal");
        
        var v = Input.GetAxisRaw("Vertical");

        //只能 x z轴移动
        if (h != tempH) //按键发生了变化
        {
            if (h != 0) //wasPressed
            {
                v = 0;
            }
            else // wasReleased
            {

            }
            dir = new Vector3(h,v,0);
        }
        else if ( v != tempV)
        {
            if (v != 0) //wasPressed
            {
                h = 0;
            }
            else // wasReleased
            {

            }
            dir = new Vector3(h, v, 0);
        }
        //后面的会覆盖前面的  比如按了下 之前按的左右就停了


        transform.position += dir * Time.deltaTime * speed;
        tempH = Input.GetAxisRaw("Horizontal");
        tempV = Input.GetAxisRaw("Vertical");
    }
}
