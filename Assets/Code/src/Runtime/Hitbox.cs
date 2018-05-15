using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HouraiTeahouse.FantasyCrescendo {

public class Hitbox : AbstractHitDetector {
  public HitboxType Type;

  public Vector3 Offset;
  public float Radius = 0.5f;

  public float BaseDamage = 10f;
  public float BaseKnockback = 5f;
  [Range(-180f, 180f)] public float KnockbackAngle = 45f;
  public float KnockbackScaling = 1f;
  public uint BaseHitstun = 1;
  public float HitstunScaling = 0.1f;
  public bool MirrorDirection = true;

  public bool IsActive => isActiveAndEnabled && Type != HitboxType.Inactive;
  public Vector3 Center => transform.TransformPoint(Offset);

  public uint Hitlag => (uint)Mathf.FloorToInt(Mathf.Abs(BaseDamage) / 3 + 3);

  float KnockbackAngleRad => Mathf.Deg2Rad * KnockbackAngle;

  public Vector2 GetKnockbackDirection(bool direction) {
    var dirMult = 1f;
    if (MirrorDirection && !direction) {
      dirMult = -1;
    }
    return new Vector2(dirMult * Mathf.Cos(KnockbackAngleRad), Mathf.Sin(KnockbackAngleRad));
  }

  public float GetKnockbackScale(float damage) => Mathf.Max(0, BaseKnockback + KnockbackScaling * damage);
  public Vector2 GetKnocback(float damage, bool dir) => GetKnockbackScale(damage) * GetKnockbackDirection(dir);

  public uint GetHitstun(float damage) => (uint)Mathf.Max(0, BaseHitstun + Mathf.FloorToInt(HitstunScaling * damage));

#if UNITY_EDITOR
  /// <summary>
  /// Callback to draw gizmos that are pickable and always drawn.
  /// </summary>
  void OnDrawGizmos() {
    if (EditorApplication.isPlayingOrWillChangePlaymode && !IsActive) { return; }
    Gizmos.color = HitboxUtil.GetHitboxColor(Type);
    Gizmos.DrawWireSphere(Center, Radius);
  }
#endif 

}

}
