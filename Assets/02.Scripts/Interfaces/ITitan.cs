using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITitan
{
    public void ScratchBody(Vector3 hitPoint, Vector3 normal);
    public void SliceNeck(Vector3 hitPoint, Vector3 normal);
    // public void SliceArm(Vector3 hitPoint, Vector3 normal);
    // public void SliceLeg(Vector3 hitPoint, Vector3 normal);
}
