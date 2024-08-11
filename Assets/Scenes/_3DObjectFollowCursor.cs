using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _3DObjectFollowCursor : MonoBehaviour
{
    #region Variable Declaration
    public Rigidbody rig; 
    #endregion

    #region Update
    void Update()
    {
        rig.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
    } 
    #endregion
}
