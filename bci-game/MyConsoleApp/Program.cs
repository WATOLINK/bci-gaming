using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using brainflow;
using brainflow.math;
using brainflow.BoardShim;
using brainflow.DataFilter;
using brainflow.Enums;

public class BoardStreamData : MonoBehaviour
{

    private BoardShim board_shim = null;
    private double sampling_rate = 0;
    private double[,] data;

    private double time_threshold = 100;
    private double max_value = -100000000000;
    private double vals_mean = 0;
    private double num_samples = 5000;
    private double samples = 0;
    private double blink_thresh = 0;

    // Use this for initialization
    void Start()
    {
        try
        {
            BrainFlowInputParams input_params = new BrainFlowInputParams();
            BoardIds board_id = BoardIds.CROWN_BOARD;
            inputParams.serial_number = "9bb2e2fad5668286a4b4f407002b4359";
            sample_rate = BoardShim.get_sampling_rate((int)board_id);
            BoardShim.enable_dev_board_logger();

            BoardShim board = new BoardShim((int)board_id, inputParams);
            board.prepare_session();

            board.start_stream(450000);
            Debug.Log("Starting calibration");
            Thread.Sleep(5000);
            data = board.get_current_board_data();
            Debug.Log("start blinking");

            // modularize this loop code into a method
            while (samples < num_samples)
            {
                data = board.get_current_board_data();
                if (data[1].Length > 0)
                {
                    DataFilter.perform_rolling_filter(data[1], 2, AggOperations.MEAN);
                    vals_mean += DataFilter.get_avg(data[1]) / num_samples;
                    samples += data[1].Length;
                    if (DataFilter.get_max(data[1]) > max_value)
                    {
                        max_value = DataFilter.get_max(data[1]);
                    }
                }
            }

            // add the code for calibration then set this blink_threshold
            blink_thresh = 0.5 * (Math.pow((max_val - val_mean), 2));

            Debug.Log("mean value");
            Debug.Log(vals_mean);
            Debug.Log("max value");
            Debug.Log(max_value);
            Debug.Log("threshold");
            Debug.Log(blink_thresh);

            Debug.Log("CALIBRATION COMPLETE, START PLAYING");


        }

    } catch (BrainFlowError as err) {
		Debug.Log(err);
	}

// Update is called once per frame
void Update()
{
    if (board_shim == null)
    {
        return;
    }

    int data_points = sample_rate * 4;

}

private void onDestroy()
{
    if (board_shim != null)
    {
        try
        {
            board_shim.release_session();
        }

        }
}
}