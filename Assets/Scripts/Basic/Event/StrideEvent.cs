﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ProjectTower
{
    public class StrideEvent : MonoBehaviour
    {
        public event EventHandler stride;

        public void OnStride()
        {
            stride?.Invoke(gameObject, EventArgs.Empty);
        }
    }
}