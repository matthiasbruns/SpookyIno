using UnityEngine;

interface HasMovementAi {

    Transform Target{get; set;}

    bool PathCompleted{get;}
}