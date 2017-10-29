using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActor : HasAnimator, HasAudioSource {

    Vector2 LookAngle { get; }

}
