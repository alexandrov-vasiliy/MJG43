using System;
using _Game.Bets;
using TMPro;
using UnityEngine;

public class Chip : MonoBehaviour
{
   public int value = 10;
   public TMP_Text chipTmp;
   public ChipType type;

   public bool highLight = false;

   public Outline outline;
   
   private void Awake()
   {
      outline = GetComponent<Outline>();
      outline.enabled = false;
   }

   private void Update()
   {
      outline.enabled = highLight;
   }

   public void Init(ChipType _type)
   {
      type = _type;
      chipTmp.text = value.ToString();
   }
}
