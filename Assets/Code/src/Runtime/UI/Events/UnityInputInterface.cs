﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Controls;

namespace HouraiTeahouse.FantasyCrescendo {

public class UnityInputInterface : BaseInput {

  public override bool mousePresent => Mouse.current != null;
  public override Vector2 mousePosition => Mouse.current?.position?.ReadValue() ?? Vector2.zero;
  //public override Vector2 mouseScrollDelta => Mouse.current?.scroll?.ReadValue() ?? Vector2.zero;

  public override bool GetMouseButton(int button) => GetMouseButtonControl(button)?.isPressed ?? false;
  public override bool GetMouseButtonDown(int button) => GetMouseButtonControl(button)?.wasJustPressed ?? false;
  public override bool GetMouseButtonUp(int button) => GetMouseButtonControl(button)?.wasJustReleased ?? false;

  public override bool GetButtonDown(string controlPath) {
    foreach (var control in InputSystem.GetControls(controlPath)) {
      var button = control as ButtonControl;
      var isMouse = control.device is Mouse;
      // Ignore mouse buttons
      if (button != null && !isMouse && button.wasJustPressed) {
        return true;
      }
    }
    return false;
  }

  public override float GetAxisRaw(string controlPath) {
    float sum = 0.0f;
    foreach (var control in InputSystem.GetControls(controlPath)) {
      var axis = control as AxisControl;
      var isMouse = control.device is Mouse;
      // Ignore mouse axes
      if (axis == null && !isMouse) continue;
      sum += axis.ReadValue();
    }
    return 0.0f; //Mathf.Clamp(sum, -1.0f, 1.0f);
  }

  ButtonControl GetMouseButtonControl(int buttonId) {
    switch (buttonId) {
      case 0: return Mouse.current?.leftButton;
      case 1: return Mouse.current?.rightButton;
      case 2: return Mouse.current?.middleButton;
      default: return null;
    }
  }

}

}