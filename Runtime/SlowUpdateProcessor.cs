using UnityEngine;
using System;

namespace UHelper
{

    public class SlowUpdateProcessor
    {
        private readonly Action action;
        private float interval;
        private float nextActionTime;
        private readonly bool useUnscaledTime;

        public float Interval
        {
            get => interval;
            set
            {
                if (value > 0)
                {
                    float currentTime = GetCurrentTime();
                    float elapsedTime = currentTime - (nextActionTime - interval);
                    interval = value;
                    nextActionTime = currentTime + Mathf.Max(0, interval - elapsedTime);
                }
            }
        }

        public SlowUpdateProcessor(Action action, float interval, bool useUnscaledTime = false)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.interval = interval > 0 ? interval : throw new ArgumentOutOfRangeException(nameof(interval), "Interval must be greater than zero.");
            this.useUnscaledTime = useUnscaledTime;

            nextActionTime = GetCurrentTime() + this.interval;
        }

        public void Update()
        {
            float currentTime = GetCurrentTime();

            if (!(currentTime >= nextActionTime)) return;

            action.Invoke();
            nextActionTime = currentTime + interval;
        }

        private float GetCurrentTime() => useUnscaledTime ? Time.unscaledTime : Time.time;
    }
}