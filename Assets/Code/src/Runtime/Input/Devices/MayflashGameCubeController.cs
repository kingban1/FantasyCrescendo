using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Utilities;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.Experimental.Input.Editor;
#endif

namespace HouraiTeahouse.FantasyCrescendo {

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public static class InputExtensions {

  static InputExtensions() {
    InputSystem.RegisterControlLayout(@"
{
    ""name"" : ""MayflashGamepad"",
    ""extend"" : ""Gamepad"",
    ""format"" : ""HID"",
    ""device"" : { 
      ""interface"" : ""HID"", 
      ""manufacturer"" : ""mayflash.+"" ,
      ""product"" : ""MAYFLASH.+"",
      ""version"" : "".+"" 
    },
    ""controls"" : [
    ]
}
    ");
  }

}

}