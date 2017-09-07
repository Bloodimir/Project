using UnityEngine;
using System;
using System.Globalization;
using System.Collections.Generic;


namespace DailyRewards.Scripts
{
    public class DailyRewards : MonoBehaviour
    {

        public List<int> rewards;          

        public DateTime timer;             
        public DateTime lastRewardTime;   

        [HideInInspector]
        public int availableReward;       

        [HideInInspector]
        public int lastReward;              

        public delegate void OnClaimPrize(int day);
        public OnClaimPrize onClaimPrize;
        public delegate void OnPrizeAlreadyClaimed(int day);
        public OnPrizeAlreadyClaimed onPrizeAlreadyClaimed;
        private float t;                   
        private bool isInitialized;       

        private const string LAST_REWARD_TIME = "LastRewardTime";
        private const string LAST_REWARD = "LastReward";
        private const string FMT = "O";

        private static DailyRewards _instance;
        public static DailyRewards instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<DailyRewards>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.hideFlags = HideFlags.HideAndDontSave;
                        _instance = obj.AddComponent<DailyRewards>();
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this.gameObject);

            if (_instance == null)
            {
                _instance = this as DailyRewards;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Update()
        {
            t += Time.deltaTime;
            if (t >= 1)
            {
                timer = timer.AddSeconds(1);
                t = 0;
            }
        }

        private void Initialize()
        {
            timer = DateTime.Now;
            isInitialized = true;
        }

        public void CheckRewards()
        {
            if (!isInitialized)
            {
                Initialize();
            }

            string lastClaimedTimeStr = PlayerPrefs.GetString(LAST_REWARD_TIME);
            lastReward = PlayerPrefs.GetInt(LAST_REWARD);


            if (!string.IsNullOrEmpty(lastClaimedTimeStr))
            {
                lastRewardTime = DateTime.ParseExact(lastClaimedTimeStr, FMT, CultureInfo.InvariantCulture);

                TimeSpan diff = timer - lastRewardTime;

                int days = (int)(Math.Abs(diff.TotalHours) / 24);

                if (days == 0)
                {
                    availableReward = 0;
                    return;
                }

                if (days >= 1 && days < 2)
                {
                    if (lastReward == rewards.Count)
                    {
                        availableReward = 1;
                        lastReward = 0;
                        return;
                    }
                    availableReward = lastReward + 1;

                    return;
                }

                if (days >= 2)
                {
                    availableReward = 1;
                    lastReward = 0;
                }
            }
            else
            {
                availableReward = 1;
            }
        }

        public void ClaimPrize(int day)
        {
            if (availableReward == day)
            {
                if (onClaimPrize != null)
                {
                    onClaimPrize(day);
                }

                PlayerPrefs.SetInt(LAST_REWARD, availableReward);

                string lastClaimedStr = timer.ToString(FMT);
                PlayerPrefs.SetString(LAST_REWARD_TIME, lastClaimedStr);
            }
            else if (day <= lastReward)
            {
                if (onPrizeAlreadyClaimed != null)
                {
                    onPrizeAlreadyClaimed(day);
                }
            }
            CheckRewards();
        }

        public void Reset()
        {
            PlayerPrefs.DeleteKey(DailyRewards.LAST_REWARD);
            PlayerPrefs.DeleteKey(DailyRewards.LAST_REWARD_TIME);
        }

    }
}