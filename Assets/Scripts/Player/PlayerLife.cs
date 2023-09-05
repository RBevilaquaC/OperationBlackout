using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : LifeSystem
{
   #region Parameters

   private Slider lifeBar;
   private TMP_Text lifeValue;
   [SerializeField] private int wagonMod;
   
   #endregion

   protected override void Start()
   {

      maxLife += wagonMod * GameController.gm.wagonCount;
      currentLife = maxLife;
      lifeBar = GameController.gm.lifebar;
      lifeValue = GameController.gm.lifeValue;
      UpdateLifeBar();
   }

   public override void TakeDamage(int damageAmount)
   {
      base.TakeDamage(damageAmount);
      UpdateLifeBar();
   }

   protected override void Death()
   {
      base.Death();
      transform.parent.gameObject.SetActive(false);
      GameController.gm.Defeat();
   }

   public override void Heal(int healAmount)
   {
      base.Heal(healAmount);
      UpdateLifeBar();
   }

   private void UpdateLifeBar()
   {
      // ReSharper disable once PossibleLossOfFraction
      lifeBar.maxValue = maxLife;
      lifeBar.value = currentLife;
      // ReSharper disable once HeapView.BoxingAllocation
      lifeValue.text = $"{currentLife}/{maxLife}";
   }
}
