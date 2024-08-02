using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using brainflow;
using brainflow.math;
/*
using brainflow.BoardShim;
using brainflow.DataFilter;
using brainflow.Enums;*/

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
	private float prev_time;
	private bool calibrated;
	// Use this for initialization

	public double get_max(double[,] in_data)
	{
		double max = in_data[0, 0];

		for (int i = 0; i < in_data.GetLength(0); i++)
		{
			for (int j = 0; j < in_data.GetLength(1); j++)
			{
				max = Math.Max(in_data[i, j], max);
			}
		}

		return max;
	}

	//what is getlength(0) vs getlength(1)
	public double get_avg(double[,] in_data)
	{
		double sum = 0;
		int rows = in_data.GetLength(0);
		int cols = in_data.GetLength(1);

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				sum += in_data[i, j];
			}
		}

		return sum / (rows * cols);
	}

	double[] GetFirstRow(double[,] in_data)
	{
		int columns = in_data.GetLength(1);
		double[] firstRow = new double[columns];

		for (int j = 0; j < columns; j++)
		{
			firstRow[j] = in_data[1, j]; //0 or 1??
		}

		return firstRow;
	}


	public void StartCalibration()
	{
		try
		{
			BrainFlowInputParams inputParams = new BrainFlowInputParams();
			BoardIds board_id = BoardIds.CROWN_BOARD;
			inputParams.serial_number = "76dcd666ce9ddeaed072838d28b9c5ac";
			sampling_rate = BoardShim.get_sampling_rate((int)board_id);
			BoardShim.enable_dev_board_logger();

			board_shim = new BoardShim((int)board_id, inputParams);
			board_shim.prepare_session();

			board_shim.start_stream(450000);
			Debug.Log("Starting calibration");
			//thread.Sleep(5000);
			data = board_shim.get_current_board_data((int) num_samples);
			Debug.Log("start blinking");

			// modularize this loop code into a method
			while (samples < num_samples)
			{
				data = board_shim.get_current_board_data((int)num_samples);
				if (data.GetLength(1) > 0)
				{
					DataFilter.perform_rolling_filter(data, 1, 2, (int) AggOperations.MEAN);
					vals_mean += get_avg(data) / num_samples;
					samples += data.GetLength(1);
					if (get_max(data) > max_value)
					{
						max_value = get_max(data);
					}
					
				}
			}

			// add the code for calibration then set this blink_threshold
			blink_thresh = 0.5 * (Math.Pow((max_value - vals_mean), 2));

			Debug.Log("mean value");
			Debug.Log(vals_mean);
			Debug.Log("max value");
			Debug.Log(max_value);
			Debug.Log("threshold");
			Debug.Log(blink_thresh);

			Debug.Log("CALIBRATION COMPLETE, START PLAYING");
			prev_time = (int) Mathf.Round(Time.time * 1000);
			calibrated = true;
		}
		catch (BrainFlowError err)
		{
			Debug.Log(err);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (board_shim == null || !calibrated)
		{
			return;
		}

		data = board_shim.get_current_board_data((int)num_samples);

		if (data.GetLength(1) > 0)
        {
			DataFilter.perform_rolling_filter(data, 1, 2, (int)AggOperations.MEAN);

			foreach (double element in GetFirstRow(data))
            {
				Debug.Log("ELEMENT: " + element + " Curr Mean:" + vals_mean);
            }

			if (((int)Mathf.Round(Time.time * 1000) - time_threshold) > prev_time)
            {
				prev_time = (int)Mathf.Round(Time.time * 1000);

				foreach (double element in GetFirstRow(data)) 
				{
					if (Math.Pow((element - vals_mean),2) >= blink_thresh)
					{
						Debug.Log("WOOO BLINK DETECTED");
					}
				}

						

			}
                

		}
            
	}

	private void OnDestroy()
	{
		if (board_shim != null)
		{
			try
			{
				board_shim.start_stream();
				board_shim.release_session();
			}
			catch (BrainFlowError err)
			{
				Debug.Log(err);
			}
		}
	}
}
