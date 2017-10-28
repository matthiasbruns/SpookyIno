using UnityEngine;

interface Interactable{
    bool CanUse{get;}
    bool CanHighlight{get;}

    void Use(GameObject executer);

    void Highlight(GameObject executer, bool activate);
}