using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActor : HasAnimator {

    Vector2 LookAngle { get; }

}
