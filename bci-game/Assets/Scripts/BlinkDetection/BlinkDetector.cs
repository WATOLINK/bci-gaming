using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Notion.Unity.Example
{
    public class BlinkDetector : MonoBehaviour
    {
        static public bool calibrationComplete = false;
        private decimal threshold;
        private decimal meanValue;
        private decimal maxValue;
        int sampleCount = 0;


        public void Calibrate(List<decimal> rawData)
        {
            decimal sum = 0;
            maxValue = decimal.MinValue;

            foreach (var value in rawData)
            {
                sum += value;
                sampleCount++;

                if (value > maxValue)
                {
                    maxValue = value;
                }
            }

            meanValue = sum / sampleCount;
            threshold = 0.4m * (decimal)Math.Pow((double)(maxValue - meanValue), 2.0);

            Debug.Log(meanValue);
            Debug.Log(threshold);

        }

        public void DetectBlink(decimal[] rawData)
        {
            foreach (var value in rawData)
            {
                decimal squaredDifference = (decimal)Math.Pow((double)(value - meanValue), 2.0);


                // Debug.Log("Thershold" + squaredDifference);
                if (squaredDifference >= threshold)
                {
                    Debug.Log("Blink Detected!");
                    return; // If you only want to detect one blink in the current data
                }
        }
        }
    }
}
