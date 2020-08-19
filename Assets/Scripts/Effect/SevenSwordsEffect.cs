﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SevenSwordsEffect : Effect
{
    protected override void OnStart()
    {
        var five = GetComponent<FiveSwordsEffect>();
        if (five != null)
        {
            five.Kill();
        }

        GetComponent<DaoshiAutoAttack>().missileCount = 7;

        base.OnStart();
        currentVfx.transform.localPosition = originalPos;
        currentVfx.transform.localScale = originalScale;
    }

    protected override void OnFinish()
    {
        GetComponent<DaoshiAutoAttack>().missileCount = 3;
        base.OnFinish();
    }
}
