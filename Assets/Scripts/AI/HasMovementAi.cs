using UnityEngine;

interface HasMovementAi {

    bool Enabled{get;set;}

    Transform Target{get; set;}

    bool PathCompleted{get;}
}