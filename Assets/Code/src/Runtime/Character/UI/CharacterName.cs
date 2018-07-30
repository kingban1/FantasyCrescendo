﻿using System.Threading.Tasks; 
using UnityEngine;
using TMPro;

namespace HouraiTeahouse.FantasyCrescendo.Characters.UI {

public class CharacterName : CharacterUIBase {

  public TMP_Text Text;
  public bool UseLongName;
  public bool Uppercase;

  public override Task Initialize(CharacterData character) {
    if (Text != null) {
      var text = UseLongName ? character.LongName : character.ShortName;
      if (Uppercase) {
        text = text.ToUpperInvariant();
      }
      Text.text = text;
    }
    return Task.CompletedTask;
  }

}

}
