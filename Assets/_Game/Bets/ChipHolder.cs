using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Game.Bets
{
    public enum ChipType
    {
        BLACK,
        BLUE,
        GREEN
    }
    public class ChipHolder : MonoBehaviour
    {
        public ChipType type;

        public List<Chip> holdedChips;
        public Chip chipPrefab;

        public float offsetY = 1f;

        public TMP_Text countTmp;

        public event Action<ChipHolder> OnClick;

        private void Update()
        {
            if(countTmp != null)
                countTmp.text = holdedChips.Count.ToString();
        }
        

        
        
        public void GenerateChips(int count)
        {
            G.feel.PlayChiGives();
            for (int i = 0; i < count; i++)
            {
                
                var chip = Instantiate(chipPrefab, transform);
                chip.transform.localPosition = new Vector3(0,
                    chip.transform.localPosition.y + i * offsetY, 0);
                chip.Init(type);
                holdedChips.Add(chip);
            }
        } 

        public void SetHighLight(bool value)
        {
            foreach (var holdedChip in holdedChips)
            {
                holdedChip.highLight = value;
            }
        }

        public void SetHoverColor()
        {
            foreach (var holdedChip in holdedChips)
            {
                holdedChip.outline.OutlineColor = Color.yellow;
            }
        }
        
        public void SetDefaultColor()
        {
            foreach (var holdedChip in holdedChips)
            {
                holdedChip.outline.OutlineColor = Color.white;
            }
        }
       

        public void PickChip(Chip chip)
        {
            holdedChips.Add(chip);
            chip.transform.SetParent(transform);
            chip.transform.DOLocalMove(Vector3.zero, 0.3f);
        }
        
        public bool MoveTo(ChipHolder another)
        {
            if(holdedChips.Count == 0) return false;
            
            var last = holdedChips[^1];
            holdedChips.Remove(last);
            another.PickChip(last);
            last.outline.OutlineColor = Color.white;
            
            G.feel.PlayChipMove();
            return true;
        }

        private void OnMouseDown()
        {
            OnClick?.Invoke(this);
        }

        public void Clear()
        {
            foreach (var holdedChip in holdedChips)
            {
                Destroy(holdedChip.gameObject);
            }
            holdedChips.Clear();
        }
    }
}