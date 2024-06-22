using Newtonsoft.Json;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

namespace Notion.Unity.Example
{
    public class BrainwavesRawHandler : IMetricHandler
    {
        int count = 0;
        public Metrics Metric => Metrics.Brainwaves;
        public string Label => "raw";
        BlinkDetector blinkDetector = new BlinkDetector();
        List<decimal> epochCalibration = new List<decimal>();
        int CALIBRATIONTIME = 100;

        public GameObject StartCalibration;
        public GameObject FinishCalibration;
        public GameObject Player;

        private readonly StringBuilder _builder;

        public BrainwavesRawHandler()
        {
            _builder = new StringBuilder();
        } 

        public void setModals(GameObject startCalibration, GameObject finishCalibration, GameObject Player) 
        {
            StartCalibration = startCalibration;
            FinishCalibration = finishCalibration;
            Player = Player;
        }


        public void Handle(string metricData)
        {
            Epoch epoch = JsonConvert.DeserializeObject<Epoch>(metricData);

            _builder.AppendLine("Handling Raw Brainwaves")
                .Append("Label: ").AppendLine(epoch.Label)
                .Append("Notch Frequency: ").AppendLine(epoch.Info.NotchFrequency)
                .Append("Sampling Rate: ").AppendLine(epoch.Info.SamplingRate.ToString())
                .Append("Start Time: ").AppendLine(epoch.Info.StartTime.ToString())
                .Append("Channel Names: ").AppendLine(string.Join(", ", epoch.Info.ChannelNames));

            // Debug.Log("Epoch data");
            // Debug.Log(JsonConvert.SerializeObject(epoch.Data)); // Log serialized JSON string for the data
            // Debug.Log(_builder.ToString());
            // Debug.Log(JsonConvert.SerializeObject(epoch.Data)); // Log serialized JSON string for the data
            _builder.Clear();
            
            if(count < CALIBRATIONTIME) {
                Debug.Log("Start collecting calibration data");
                count = count + 1;
                for (int i = 0; i < epoch.Data[0].Length; i++)
                {
                    epochCalibration.Add(epoch.Data[0][i]);
                    Debug.Log(epochCalibration);
                }
            }
            else if (count == CALIBRATIONTIME) 
            {
                Debug.Log(epochCalibration);
                Debug.Log("Calibrating");
                count = count + 1;
                blinkDetector.Calibrate(epochCalibration);
                StartCalibration.SetActive(false);
                FinishCalibration.SetActive(true);
            }
            else if (count > CALIBRATIONTIME)
            {
                Debug.Log("Calibration finished - start blinking");
                blinkDetector.DetectBlink(epoch.Data[0], Player);
            }
        }
    }
}