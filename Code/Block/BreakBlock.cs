using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlock : Block
{
    private string anim_name = "BreakBlock";

    public override void Init()
    {
        StopAnim();
        obj.GetComponent<Animator>().Rebind();
    }

    public override bool IsBreakEnd()
    {
        Animator anim = obj.GetComponent<Animator>();
        bool ret = false;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(anim_name) &&
            anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            StopAnim();
            ret = true;
        }

        return ret;
    }

    public override void Break()
    {
        if(!IsPlayingAnim())
        {
            StartAnim();
        }
    }

    public override bool CanChangeNextBlock()
    {
        return IsBreakEnd();
    }

    public override bool CanMoveBlock()
    {
        return false;
    }

    public override EnumBlockType NextBlockType()
    {
        return EnumBlockType.Empty;
    }

    private void StartAnim()
    {
        obj.GetComponent<Animator>().speed = 1.0f;
    }

    private void StopAnim()
    {
        obj.GetComponent<Animator>().speed = 0.0f;
    }

    private bool IsPlayingAnim()
    {
        return obj.GetComponent<Animator>().speed > 0;
    }
}
