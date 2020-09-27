﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class StompEvent : MonoBehaviour
    {
        public event EventHandler stomp;

        public void OnStomp()
        {
            stomp?.Invoke(gameObject, EventArgs.Empty);
        }
    }
}