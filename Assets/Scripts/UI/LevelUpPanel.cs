using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace RPG.UI
{
	/// <summary>
	/// LevelUpPanel
	/// </summary>
	public class LevelUpPanel : MonoBehaviour
	{
        [Header("Buttons")]
        public Button StrengthButton;
        public Button DexterityButton;
        public Button ConstitutionButton;
        public Button SpellpowerButton;
        public Button IntelligenceButton;

        [Header("Labels")]
        public Text StrengthLabel;
        public Text DexterityLabel;
        public Text ConstitutionLabel;
        public Text SpellpowerLabel;
        public Text IntelligenceLabel;

        public void Show(PlayerCharacter pc)
        {
            StrengthLabel.text = pc.Strength.ToString();
            DexterityLabel.text = pc.Dexterity.ToString();
            ConstitutionLabel.text = pc.Constitution.ToString();
            SpellpowerLabel.text = pc.Spellpower.ToString();
            IntelligenceLabel.text = pc.Intelligence.ToString();

            StrengthButton.onClick.RemoveAllListeners();
            StrengthButton.onClick.AddListener(() => { ++pc.Strength; UseAbilityPoint(pc); });
            DexterityButton.onClick.RemoveAllListeners();
            DexterityButton.onClick.AddListener(() => { ++pc.Dexterity; UseAbilityPoint(pc); });
            ConstitutionButton.onClick.RemoveAllListeners();
            ConstitutionButton.onClick.AddListener(() => { ++pc.Constitution; UseAbilityPoint(pc); });
            SpellpowerButton.onClick.RemoveAllListeners();
            SpellpowerButton.onClick.AddListener(() => { ++pc.Spellpower; UseAbilityPoint(pc); });
            IntelligenceButton.onClick.RemoveAllListeners();
            IntelligenceButton.onClick.AddListener(() => { ++pc.Intelligence; UseAbilityPoint(pc); });

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        private void UseAbilityPoint(PlayerCharacter pc)
        {
            if (pc.UseAbilityPoint() > 0)
                Show(pc);
            else
                Hide();
        }
    }
}