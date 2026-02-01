
using System;
using UnityEngine;

public class ResetProgressOnStart : MonoBehaviour
{
   private void Start()
   {
      if (GameSession.Instance == null) return;
      GameSession.Instance.Clear();
   }
}
