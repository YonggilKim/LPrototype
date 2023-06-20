using Data;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public Define.eObjectType ObjectType { get; protected set; }
    public SkeletonAnimation SkeletonAnim;
    public SkeletonDataAsset SkeletonData;
    [SpineEvent] 
    public string EventName;

    bool _init = false;

    void Awake()
    {
        Init();
        //SkeletonAnim = GetComponent<SkeletonAnimation>();
        //if(SkeletonAnim != null ) 
        //    SkeletonAnim.AnimationState.Event += HandleEvent;

    }

    public virtual bool Init()
    {
        if (_init)
            return false;

        _init = true;
        return true;
    }

    public void PlayAnimation(int trackIndex, string AnimName, bool loop, Action EventCallback = null)
    {
        if (EventCallback != null)
        {
            SkeletonAnim.AnimationState.Event += (TrackEntry trackEntry, Spine.Event e) =>
            {
                //Debug.Log(e.Data.Name);
                //if (e.Data.Name == "YourEventName")
                {
                    EventCallback.Invoke();
                }
            };
        }

        SkeletonAnim.AnimationState.SetAnimation(trackIndex, AnimName, loop);

    }

    public void AddAnimation(int trackIndex, string AnimName, bool loop, float delay)
    {
        SkeletonAnim.AnimationState.AddAnimation(trackIndex, AnimName, loop,delay);
    }
    void HandleEvent(TrackEntry trackEntry, Spine.Event e)
    {
        // Play some sound if the event named "footstep" fired.

        //if (e.Data.Name == footstepEventName)

        {
            Debug.Log("Play a footstep sound!");
        }
    }
}
