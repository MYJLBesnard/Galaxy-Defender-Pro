using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ------------------------------------------------------------------------------------
// Name	:	RotateAnimator
// Desc :	Animates the roation of an object
// ------------------------------------------------------------------------------------
public class RotateAnimator : MonoBehaviour
{
    // Component Cache
    private Transform _myTransform = null;

    // The rotation deltas we would like to rotate our camera
    // per second about its 3 axis.
    public Vector3 _eulers = new Vector3(1.0f, 0.0f, 5.0f);

    // ------------------------------------------------------------------------------------
    // Name	:	Start
    // Desc :	Called just prior to the first render of the scene.  It is used
    //          to cache the Transform component of whatever object this script
    //          is attached to.
    // ------------------------------------------------------------------------------------
    void Start()
    {
        // Cache transform component
        _myTransform = transform;
    }

    // ------------------------------------------------------------------------------------
    // Name	:	Update
    // Desc :	Called every frame to rotate the obkect to which this
    //          script is attached.
    // ------------------------------------------------------------------------------------
    void Update()
    {
        // Apply the rotation to the transform
        _myTransform.Rotate(_eulers * Time.deltaTime);
    }
}
