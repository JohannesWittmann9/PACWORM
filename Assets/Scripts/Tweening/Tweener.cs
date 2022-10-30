using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour
{

    //private Tween activeTween;
    private List<Tween> activeTweens = new List<Tween>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < activeTweens.Count; i++)
        {
            Tween activeTween = activeTweens[i];
            if (Vector3.Distance(activeTween.Target.position, activeTween.EndPos) > 0.1f)
            {
                float fraction = (Time.time - activeTween.StartTime) / activeTween.Duration;
                activeTween.Target.position = Vector3.Lerp(activeTween.StartPos, activeTween.EndPos, fraction);
            }
            else
            {
                activeTween.Target.position = activeTween.EndPos;
                activeTweens.Remove(activeTween);
            }
        }

    }

    public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    {
        if (TweenExists(targetObject))
        {
            return false;
        }
        activeTweens.Add(new Tween(targetObject, startPos, endPos, Time.time, duration));
        return true;
        
    }

    public bool TweenExists(Transform target)
    {
        foreach (Tween t in activeTweens)
        {
            if(t.Target == target)
            {
                return true;
            }
        }
        return false;
    }

    public void StopTween(Transform target)
    {
        Tween targetTween = null;
        foreach (Tween t in activeTweens)
        {
            if (t.Target == target)
            {
                targetTween = t;
            }
        }

        if(targetTween != null) activeTweens.Remove(targetTween);
    }
}
