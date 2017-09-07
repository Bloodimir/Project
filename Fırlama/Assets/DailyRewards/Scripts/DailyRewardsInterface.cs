using UnityEngine;
using UnityEngine.UI;
using System;
using com.niobiumstudios.dailyrewards;

namespace DailyRewards.Scripts
{
    public class DailyRewardsInterface : MonoBehaviour
    {

        public GameObject dailyRewardPrefab;

        public GameObject panelReward;
        public Text txtReward;

        public UnityEngine.UI.Button btnClaim;

        public Text txtTimeDue;

        public GridLayoutGroup dailyRewardsGroup;

        void Start ()
        {
            DailyRewards.instance.CheckRewards();

            DailyRewards.instance.onClaimPrize += OnClaimPrize;
            DailyRewards.instance.onPrizeAlreadyClaimed += OnPrizeAlreadyClaimed;

            UpdateUI();
        }

        void OnDestroy()
        {
            DailyRewards.instance.onClaimPrize -= OnClaimPrize;
            DailyRewards.instance.onPrizeAlreadyClaimed -= OnPrizeAlreadyClaimed;
        }

        public void OnClaimClick()
        {
            DailyRewards.instance.ClaimPrize(DailyRewards.instance.availableReward);
            UpdateUI();
        }

        public void UpdateUI()
        {
            foreach (Transform child in dailyRewardsGroup.transform)
            {
                Destroy(child.gameObject);
            }

            bool isRewardAvailableNow = false;

            for (int i = 0; i < DailyRewards.instance.rewards.Count; i++)
            {
                int reward = DailyRewards.instance.rewards[i];
                int gün = i + 1;

                GameObject dailyRewardGo = GameObject.Instantiate(dailyRewardPrefab) as GameObject;

                DailyRewardUI dailyReward = dailyRewardGo.GetComponent<DailyRewardUI>();
                dailyReward.transform.SetParent(dailyRewardsGroup.transform);
                dailyRewardGo.transform.localScale = Vector2.one;

                dailyReward.day = gün;
                dailyReward.reward = reward;

                dailyReward.isAvailable = gün == DailyRewards.instance.availableReward;
                dailyReward.isClaimed = gün <= DailyRewards.instance.lastReward;

                if (dailyReward.isAvailable)
                {
                    isRewardAvailableNow = true;
                }

                dailyReward.Refresh();
            }

            btnClaim.gameObject.SetActive(isRewardAvailableNow);
            txtTimeDue.gameObject.SetActive(!isRewardAvailableNow);
        }

        void Update()
        {
            if (txtTimeDue.IsActive())
            {
                TimeSpan difference = (DailyRewards.instance.lastRewardTime - DailyRewards.instance.timer).Add(new TimeSpan(0, 24, 0, 0));

                if (difference.TotalSeconds <= 0)
                {
                    DailyRewards.instance.CheckRewards();
                    UpdateUI();
                    return;
                }

                string formattedTs = string.Format("{0:D2}:{1:D2}:{2:D2}", difference.Hours, difference.Minutes, difference.Seconds);

                txtTimeDue.text = "Bir Sonraki Ödül İçin " + formattedTs + " Süre Geçmesi Gerekiyor.";
            }
        }

        private void OnClaimPrize(int day)
        {
            panelReward.SetActive(true);
            txtReward.text = "Tebrikler " + DailyRewards.instance.rewards[day-1] + " altın kazandınız!";
        }

        private void OnPrizeAlreadyClaimed(int day)
        {
        }

        public void OnCloseRewardsClick()
        {
            panelReward.SetActive(false);
        }

    }
}