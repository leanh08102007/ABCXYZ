from threading import Thread
import numpy as np
from Communication import Comu
import time
pts = np.array([[40, 113], [104, 117], [480, 265], [480, 320], [300, 320]])
Comu.ConnectSerialPort("/dev/ttyUSB0")
def main():
    print("in main")
    Comu.ReadACParameter()
    while True:
        starttime=time.time()
        time.sleep(5)
        print("############ AC ##############")
        Comu.ReadACParameter()
        print("############ DC ##############")
        Comu.ReadDCParameter()
        print("############ Time ##############")
        print(time.time()-starttime)
if __name__ == '__main__':
    # main()
    print(pts)
