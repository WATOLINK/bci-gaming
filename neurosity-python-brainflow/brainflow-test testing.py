import brainflow
import argparse
import time
import numpy as np
import collections
import pyautogui
# import pydirectinput
import pandas as pd
import matplotlib.pyplot as plt
#from IPython.display import display, clear_output

from brainflow import BoardIds
from brainflow.board_shim import BoardShim, BrainFlowInputParams
from brainflow.data_filter import DataFilter, FilterTypes, AggOperations, WindowOperations

def main ():

    # yaxin
    board_id = BoardIds.CROWN_BOARD.value # or BoardIds.NOTION_2_BOARD.value or BoardIds.NOTION_1_BOARD.value
    params = BrainFlowInputParams ()
    params.board_id = board_id
    params.serial_number = "76dcd666ce9ddCeaed072838d28b9c5ac"
    sampling_rate = BoardShim.get_sampling_rate (board_id)
    time_thres =  100
    max_val = -100000000000
    vals_mean = 0
    num_samples = 5000
    samples = 0
    BoardShim.enable_dev_board_logger() #TLDR -> is u pass in file path then it will log to that file, otherwise it will log to stdout
    
    
    board = BoardShim (board_id, params)
    board.prepare_session ()

    board.start_stream(450000)
    print("starting calibration")
    time.sleep(5)
    data = board.get_board_data()
    print("start blinking")
    
    ## put in start func as calibra
    while(samples < num_samples):
        data = board.get_board_data() 
        if(len(data[1]) > 0):
            DataFilter.perform_rolling_filter (data[1], 2, AggOperations.MEAN.value)
            vals_mean += sum([data[1,i]/num_samples for i in range(len(data[1]))]) 
            samples += len(data[1]) 
            if(np.amax(data[1]) > max_val):
                max_val = np.amax(data[1])
 
    blink_thres = 0.5*((max_val - vals_mean)**2) 
    ## end
    print("mean value")  
    print(vals_mean)
    print("max value")
    print(max_val)
    print("threshold")
    print(blink_thres)


    print("CALIBRATION DONE START PLAYING")

    eeg_channels = BoardShim.get_eeg_channels(board_id)
    timestamp_channel = BoardShim.get_timestamp_channel(board_id)
    
    # PSD calculation
    # nfft = DataFilter.get_nearest_power_of_two(sampling_rate)
    # eeg_channel = eeg_channels[0]
    nfft = 256
    fs = 250  # Sampling rate, adjust as needed
    # psd = DataFilter.get_psd_welch(data[fs], nfft, nfft // 2, sampling_rate, WindowOperations.NO_WINDOW.value)
    
    # Plotting before processing
    df = pd.DataFrame(np.transpose(data))
    plt.figure()
    df[eeg_channels[:3]].plot(subplots=True)
    plt.savefig("before_processing.png")

    prev_time = int(round(time.time() * 1000))

    
    while True:

        data = board.get_board_data() 

        if(len(data[1]) > 0):
            DataFilter.perform_rolling_filter (data[1], 2, AggOperations.MEAN.value) 
            if((int(round(time.time() * 1000)) - time_thres) > prev_time): 
                prev_time = int(round(time.time() * 1000))
                for element in data[1]:
                    if(((element - vals_mean)**2) >= blink_thres):  
                           print("WOOO BLINK DETECTED")
                           break              

    # while True:
    #     data = board.get_board_data()

    #     if len(data[1]) > 0:
    #         data[1] = DataFilter.perform_rolling_filter(data[1], 2, AggOperations.MEAN.value)

    #         current_time = int(round(time.time() * 1000))
    #         if (current_time - time_thres) > prev_time:
    #             prev_time = current_time

    #             blink_detected = any(((element - vals_mean) ** 2) >= blink_thres for element in data[1])
    #             if blink_detected:
    #                 print("WOOO BLINK DETECTED")
    #                 # Perform any additional actions or break the loop as needed
    #                 break   

    board.stop_stream ()
    board.release_session ()

if __name__ == "__main__":
    main ()
    # comment