using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public struct Spring {
    public float min;
    public float max;
    public float dampRatio;
    public float stiffness;
    public float rest;
    public float vel;

    public float GetValue(float position, float deltaTime) {
        float x = position - rest;
		vel += (-dampRatio * vel) - (stiffness * x);
		position += vel * deltaTime;
        return position;
    }
}