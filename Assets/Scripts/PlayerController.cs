using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, 0, 0);

        if (Input.GetKeyDown(KeyCode.S))
            GivingBuff(BuffList.TEST_EXTRA_SPEED);

        if (Input.GetKeyDown(KeyCode.W))
            GivingBuff(BuffList.TEST_SPEED_MULT);
    }
}
