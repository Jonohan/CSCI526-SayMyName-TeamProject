using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IsoHelper
{
   public static Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
   public static Matrix4x4 inverseIso = isoMatrix.inverse;
   public static Vector3 ToIsoView(this Vector3 input)
   {
      return isoMatrix.MultiplyPoint3x4(input);
   }

   public static Vector3 IsoToWorld(this Vector3 isoPos)
   {
      return inverseIso.MultiplyPoint3x4(isoPos);
   }
}
