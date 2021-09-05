#!/usr/bin/python
# -*- coding: utf-8 -*-
import struct

import serial
import serial.tools.list_ports
from Mylib.Communication import define as df
from Mylib.Debug import TermColor as cl
from serial import *

counter = 0
cmds = [
    10,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    5,
]


def CheckPortAvalibale(portname):
    ports = serial.tools.list_ports.comports(include_links=False)
    for port in ports:
        if port.device == portname:
            return True
    return False


def ConnectSerialPort(portname):
    if CheckPortAvalibale(portname):
        try:
            df.ser = serial.Serial(port=portname, baudrate=2400)
            if df.ser.isOpen():
                print('Serial port is openned!')
                df.Serialconnection = True
        except:
            print('error to open serial port')


def GetDataFrom(
        portname,
        add,
        command,
        timeout,
):
    start = 0
    stop = 0
    address = 1
    FrameSize = 30
    dataFrame = [
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
    ]
    flagStartFrame = False
    flagStopFrame = False
    bytecouter = 0
    Step = 0
    index = 0
    start = time.time()

    if CheckPortAvalibale(portname):
        if df.Serialconnection == True:
            if df.ser.isOpen():
                # print ('Connected to port:' + portname)
                df.ser.write(struct.pack('>B', 0x40))
                df.ser.write(struct.pack('>B', 0x30))
                df.ser.write(struct.pack('>B', 0x30))
                cmds[0] = add
                cmds[1] = 1
                cmds[2] = command
                for cmd in cmds:
                    df.ser.write(struct.pack('>B', cmd))
                # print ('Command sent')
                start = time.time()
                # print(cmds)
                flag = False
                while time.time() - start < timeout:
                    if df.ser.inWaiting() > 0:
                        inchar = int.from_bytes(df.ser.read(1), 'big')
                        # print(inchar)
                        bytecouter += 1
                        if inchar == 0x40 and bytecouter == 1:
                            Step = 1
                        elif inchar == 0x30 and Step == 1 \
                                and bytecouter == 2:
                            Step = 2
                        elif inchar == 0x30 and Step == 2 \
                                and bytecouter == 3:
                            flagStartFrame = True
                            bytecouter = 0
                            index = 0
                        else:
                            bytecouter = 0
                            Step = 0
                        if flagStartFrame == True:
                            dataFrame[index] = inchar

                            # print(dataFrame[index])

                            index += 1
                        if index > FrameSize:
                            if dataFrame[1] == address:
                                flagStopFrame = True
                            index = 0
                            flagStartFrame = False
                            bytecouter = 0
                            Step = 0

                            print(dataFrame)
                            flag = True
                            return dataFrame
                            break
                if flag == False:
                    print("Time out")
                    for i in range(len(dataFrame)):
                        dataFrame[i] = None
                    return dataFrame

            else:

                # print(inchar)
                # print(len(cmds))

                print('Port is closed!!!')
                for i in range(len(dataFrame)):
                    # dataFrame[i] = None
                    dataFrame[i] = None
                return dataFrame
        else:
            print('Try to reconnect to Serialport')
            for i in range(len(dataFrame)):
                # dataFrame[i] = None
                dataFrame[i] = None
            return dataFrame
    else:
        if df.Serialconnection == True:
            df.Serialconnection = False
            df.ser.close()
        print('Port is not invalible!!!')
        for i in range(len(dataFrame)):
            # dataFrame[i] = None
            dataFrame[i] = None
        return dataFrame


def ControlOutput(
        portname,
        OutNo,
        Type,
        State,
        timing,
        timeout,
):
    start = 0
    stop = 0
    address = 1
    FrameSize = 30
    dataFrame = [
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
    ]
    flagStartFrame = False
    flagStopFrame = False
    bytecouter = 0
    Step = 0
    index = 0
    start = time.time()
    if CheckPortAvalibale(portname):
        if df.Serialconnection == True:
            if df.ser.isOpen():
                # print ('Connected to port:' + portname)
                df.ser.write(struct.pack('>B', 0x40))
                df.ser.write(struct.pack('>B', 0x30))
                df.ser.write(struct.pack('>B', 0x30))
                if OutNo > 5:
                    cmds[0] = 2
                    cmds[2] = 3
                else:
                    cmds[0] = 100
                    cmds[2] = 2
                cmds[1] = 1
                cmds[3] = OutNo
                cmds[4] = Type
                cmds[5] = State
                cmds[6] = timing
                for cmd in cmds:
                    df.ser.write(struct.pack('>B', cmd))
                # print ('Command sent')
                start = time.time()
                while time.time() - start < timeout:
                    if df.ser.inWaiting() > 0:
                        inchar = int.from_bytes(df.ser.read(1), 'big')
                        bytecouter += 1
                        if inchar == 0x40 and bytecouter == 1:
                            Step = 1
                        elif inchar == 0x30 and Step == 1 \
                                and bytecouter == 2:
                            Step = 2
                        elif inchar == 0x30 and Step == 2 \
                                and bytecouter == 3:
                            flagStartFrame = True
                            bytecouter = 0
                            index = 0
                        else:
                            bytecouter = 0
                            Step = 0
                        if flagStartFrame == True:
                            dataFrame[index] = inchar

                            # print(index)

                            index += 1
                        if index > FrameSize:
                            if dataFrame[1] == address:
                                flagStopFrame = True
                            index = 0
                            flagStartFrame = False
                            bytecouter = 0
                            Step = 0

                            # print(dataFrame)

                            return dataFrame
                            break
            else:

                # print(inchar)
                # print(len(cmds))

                print('Port is closed!!!')
                return None
        else:
            print('Try to reconnect to Serialport')
    else:
        if df.Serialconnection == True:
            df.Serialconnection = False
            df.ser.close()
        print('Port is not invalible!!!')
    return None


def DigitalWrite(OutNo, State):
    return ControlOutput("/dev/ttyUSB0", OutNo, 1, State, 1, 1)


def DigitalWriteTiming(OutNo, Time):
    return ControlOutput("/dev/ttyUSB0", OutNo, 2, 1, Time, 1)


def Convertnumber(arg1, arg2, arg3):
    if arg1 == None or arg2 == None or arg3 == None:
        return -2
    NumFloat = 0
    NumFloat = arg1 << 8
    NumFloat = NumFloat + arg2
    for i in range(arg3):
        NumFloat = NumFloat / 10
    return round(NumFloat, 2)


def ReadACParameter():
    try:
        ReceiveFrame = []
        ReceiveFrame = GetDataFrom("/dev/ttyUSB0", 2, 2, 1)
        # print(ReceiveFrame)
        # print(ReceiveFrame)
        df.VAC[0] = Convertnumber(ReceiveFrame[3], ReceiveFrame[4], ReceiveFrame[5])
        df.VAC[1] = Convertnumber(ReceiveFrame[6], ReceiveFrame[7], ReceiveFrame[8])
        df.VAC[2] = Convertnumber(ReceiveFrame[9], ReceiveFrame[10], ReceiveFrame[11])
        df.IAC[0] = Convertnumber(ReceiveFrame[12], ReceiveFrame[13], ReceiveFrame[14])
        df.IAC[1] = Convertnumber(ReceiveFrame[15], ReceiveFrame[16], ReceiveFrame[17])
        df.IAC[2] = Convertnumber(ReceiveFrame[18], ReceiveFrame[19], ReceiveFrame[20])
        df.FAC[0] = Convertnumber(ReceiveFrame[21], ReceiveFrame[22], ReceiveFrame[23])
        df.FAC[1] = Convertnumber(ReceiveFrame[24], ReceiveFrame[25], ReceiveFrame[26])
        df.FAC[2] = Convertnumber(ReceiveFrame[27], ReceiveFrame[28], ReceiveFrame[29])
        ReceiveFrame = GetDataFrom("/dev/ttyUSB0", 2, 1, 1)
        df.VAC[3] = Convertnumber(ReceiveFrame[3], ReceiveFrame[4], ReceiveFrame[5])
        df.VAC[4] = Convertnumber(ReceiveFrame[6], ReceiveFrame[7], ReceiveFrame[8])
        df.VAC[5] = Convertnumber(ReceiveFrame[9], ReceiveFrame[10], ReceiveFrame[11])
        df.VAccuMayPhat = Convertnumber(ReceiveFrame[12], ReceiveFrame[13], ReceiveFrame[14])
        df.FAC[3] = Convertnumber(ReceiveFrame[15], ReceiveFrame[16], ReceiveFrame[17])
        df.FAC[4] = Convertnumber(ReceiveFrame[18], ReceiveFrame[19], ReceiveFrame[20])
        df.FAC[5] = Convertnumber(ReceiveFrame[21], ReceiveFrame[22], ReceiveFrame[23])
        df.inputByte[2] = ReceiveFrame[29]
        df.inputEventByte[2] = ReceiveFrame[30]
        # print(ReceiveFrame)
        # print(" AC0 AC1 AC2 AC3 AC4 AC5")
        ReceiveFrame = GetDataFrom("/dev/ttyUSB0", 2, 4, 1)
        print(ReceiveFrame)
        print(df.VAC)
        print(df.IAC)
        print(df.FAC)
        print(df.VAccuMayPhat)
    except:
        cl.ColorPrint(">Khong lay duoc du lieu!!!", cl.bcolors.FAIL)


def ReadAccuParameter():
    ReceiveFrame = []
    # accuOffsetNo=GroupNo*4
    accu = 0
    # ReceiveFrame = GetDataFrom("/dev/ttyUSB0", 20, 5, 5)
    # ReceiveFrame = GetDataFrom("/dev/ttyUSB0", 55, 5, 5)
    # ReceiveFrame = GetDataFrom("/dev/ttyUSB0", 10, 5, 5)
    # ReceiveFrame = GetDataFrom("/dev/ttyUSB0", 11, 5, 5)
    # try:
    for i in range(9):
        AddGroup = (i + 1) * 10
        if df.accu_config[i] != 0 and df.accu_config[i] != None:
            print("########################## Group " + str(i + 1) + " ######################")
            for j in range(df.accu_config[i]):
                accuOffsetNo = j * 4
                ReceiveFrame = GetDataFrom("/dev/ttyUSB0", AddGroup + j, 1, 1)
                if (i == 0):
                    df.VAccu1[accuOffsetNo] = Convertnumber(ReceiveFrame[12], ReceiveFrame[13], ReceiveFrame[14])
                    print("Vaccu " + str(accuOffsetNo) + ": " + str(df.VAccu1[accuOffsetNo]))
                    df.VAccu1[accuOffsetNo + 1] = Convertnumber(ReceiveFrame[9], ReceiveFrame[10], ReceiveFrame[11])
                    print("Vaccu " + str(accuOffsetNo + 1) + ": " + str(df.VAccu1[accuOffsetNo + 1]))
                    df.VAccu1[accuOffsetNo + 2] = Convertnumber(ReceiveFrame[6], ReceiveFrame[7], ReceiveFrame[8])
                    print("Vaccu " + str(accuOffsetNo + 2) + ": " + str(df.VAccu1[accuOffsetNo + 2]))
                    df.VAccu1[accuOffsetNo + 3] = Convertnumber(ReceiveFrame[3], ReceiveFrame[4], ReceiveFrame[5])
                    print("Vaccu " + str(accuOffsetNo + 3) + ": " + str(df.VAccu1[accuOffsetNo + 3]))
                elif (i == 1):
                    df.VAccu2[accuOffsetNo] = Convertnumber(ReceiveFrame[12], ReceiveFrame[13], ReceiveFrame[14])
                    print("Vaccu " + str(accuOffsetNo) + ": " + str(df.VAccu2[accuOffsetNo]))
                    df.VAccu2[accuOffsetNo + 1] = Convertnumber(ReceiveFrame[9], ReceiveFrame[10], ReceiveFrame[11])
                    print("Vaccu " + str(accuOffsetNo + 1) + ": " + str(df.VAccu2[accuOffsetNo + 1]))
                    df.VAccu2[accuOffsetNo + 2] = Convertnumber(ReceiveFrame[6], ReceiveFrame[7], ReceiveFrame[8])
                    print("Vaccu " + str(accuOffsetNo + 2) + ": " + str(df.VAccu2[accuOffsetNo + 2]))
                    df.VAccu2[accuOffsetNo + 3] = Convertnumber(ReceiveFrame[3], ReceiveFrame[4], ReceiveFrame[5])
                    print("Vaccu " + str(accuOffsetNo + 3) + ": " + str(df.VAccu2[accuOffsetNo + 3]))
                elif (i == 2):
                    df.VAccu3[accuOffsetNo] = Convertnumber(ReceiveFrame[12], ReceiveFrame[13], ReceiveFrame[14])
                    print("Vaccu " + str(accuOffsetNo) + ": " + str(df.VAccu3[accuOffsetNo]))
                    df.VAccu3[accuOffsetNo + 1] = Convertnumber(ReceiveFrame[9], ReceiveFrame[10], ReceiveFrame[11])
                    print("Vaccu " + str(accuOffsetNo + 1) + ": " + str(df.VAccu3[accuOffsetNo + 1]))
                    df.VAccu3[accuOffsetNo + 2] = Convertnumber(ReceiveFrame[6], ReceiveFrame[7], ReceiveFrame[8])
                    print("Vaccu " + str(accuOffsetNo + 2) + ": " + str(df.VAccu3[accuOffsetNo + 2]))
                    df.VAccu3[accuOffsetNo + 3] = Convertnumber(ReceiveFrame[3], ReceiveFrame[4], ReceiveFrame[5])
                    print("Vaccu " + str(accuOffsetNo + 3) + ": " + str(df.VAccu3[accuOffsetNo + 3]))
                elif (i == 3):
                    df.VAccu4[accuOffsetNo] = Convertnumber(ReceiveFrame[12], ReceiveFrame[13], ReceiveFrame[14])
                    print("Vaccu " + str(accuOffsetNo) + ": " + str(df.VAccu4[accuOffsetNo]))
                    df.VAccu4[accuOffsetNo + 1] = Convertnumber(ReceiveFrame[9], ReceiveFrame[10], ReceiveFrame[11])
                    print("Vaccu " + str(accuOffsetNo + 1) + ": " + str(df.VAccu4[accuOffsetNo + 1]))
                    df.VAccu4[accuOffsetNo + 2] = Convertnumber(ReceiveFrame[6], ReceiveFrame[7], ReceiveFrame[8])
                    print("Vaccu " + str(accuOffsetNo + 2) + ": " + str(df.VAccu4[accuOffsetNo + 2]))
                    df.VAccu4[accuOffsetNo + 3] = Convertnumber(ReceiveFrame[3], ReceiveFrame[4], ReceiveFrame[5])
                    print("Vaccu " + str(accuOffsetNo + 3) + ": " + str(df.VAccu4[accuOffsetNo + 3]))
                elif (i == 4):
                    df.VAccu5[accuOffsetNo] = Convertnumber(ReceiveFrame[12], ReceiveFrame[13], ReceiveFrame[14])
                    print("Vaccu " + str(accuOffsetNo) + ": " + str(df.VAccu5[accuOffsetNo]))
                    df.VAccu5[accuOffsetNo + 1] = Convertnumber(ReceiveFrame[9], ReceiveFrame[10], ReceiveFrame[11])
                    print("Vaccu " + str(accuOffsetNo + 1) + ": " + str(df.VAccu5[accuOffsetNo + 1]))
                    df.VAccu5[accuOffsetNo + 2] = Convertnumber(ReceiveFrame[6], ReceiveFrame[7], ReceiveFrame[8])
                    print("Vaccu " + str(accuOffsetNo + 2) + ": " + str(df.VAccu5[accuOffsetNo + 2]))
                    df.VAccu5[accuOffsetNo + 3] = Convertnumber(ReceiveFrame[3], ReceiveFrame[4], ReceiveFrame[5])
                    print("Vaccu " + str(accuOffsetNo + 3) + ": " + str(df.VAccu5[accuOffsetNo + 3]))
                elif (i == 5):
                    df.VAccu6[accuOffsetNo] = Convertnumber(ReceiveFrame[12], ReceiveFrame[13], ReceiveFrame[14])
                    print("Vaccu " + str(accuOffsetNo) + ": " + str(df.VAccu6[accuOffsetNo]))
                    df.VAccu6[accuOffsetNo + 1] = Convertnumber(ReceiveFrame[9], ReceiveFrame[10], ReceiveFrame[11])
                    print("Vaccu " + str(accuOffsetNo + 1) + ": " + str(df.VAccu6[accuOffsetNo + 1]))
                    df.VAccu6[accuOffsetNo + 2] = Convertnumber(ReceiveFrame[6], ReceiveFrame[7], ReceiveFrame[8])
                    print("Vaccu " + str(accuOffsetNo + 2) + ": " + str(df.VAccu6[accuOffsetNo + 2]))
                    df.VAccu6[accuOffsetNo + 3] = Convertnumber(ReceiveFrame[3], ReceiveFrame[4], ReceiveFrame[5])
                    print("Vaccu " + str(accuOffsetNo + 3) + ": " + str(df.VAccu6[accuOffsetNo + 3]))
                elif (i == 6):
                    df.VAccu7[accuOffsetNo] = Convertnumber(ReceiveFrame[12], ReceiveFrame[13], ReceiveFrame[14])
                    print("Vaccu " + str(accuOffsetNo) + ": " + str(df.VAccu7[accuOffsetNo]))
                    df.VAccu7[accuOffsetNo + 1] = Convertnumber(ReceiveFrame[9], ReceiveFrame[10], ReceiveFrame[11])
                    print("Vaccu " + str(accuOffsetNo + 1) + ": " + str(df.VAccu7[accuOffsetNo + 1]))
                    df.VAccu7[accuOffsetNo + 2] = Convertnumber(ReceiveFrame[6], ReceiveFrame[7], ReceiveFrame[8])
                    print("Vaccu " + str(accuOffsetNo + 2) + ": " + str(df.VAccu7[accuOffsetNo + 2]))
                    df.VAccu7[accuOffsetNo + 3] = Convertnumber(ReceiveFrame[3], ReceiveFrame[4], ReceiveFrame[5])
                    print("Vaccu " + str(accuOffsetNo + 3) + ": " + str(df.VAccu7[accuOffsetNo + 3]))
                elif (i == 7):
                    df.VAccu8[accuOffsetNo] = Convertnumber(ReceiveFrame[12], ReceiveFrame[13], ReceiveFrame[14])
                    print("Vaccu " + str(accuOffsetNo) + ": " + str(df.VAccu8[accuOffsetNo]))
                    df.VAccu8[accuOffsetNo + 1] = Convertnumber(ReceiveFrame[9], ReceiveFrame[10], ReceiveFrame[11])
                    print("Vaccu " + str(accuOffsetNo + 1) + ": " + str(df.VAccu8[accuOffsetNo + 1]))
                    df.VAccu8[accuOffsetNo + 2] = Convertnumber(ReceiveFrame[6], ReceiveFrame[7], ReceiveFrame[8])
                    print("Vaccu " + str(accuOffsetNo + 2) + ": " + str(df.VAccu8[accuOffsetNo + 2]))
                    df.VAccu8[accuOffsetNo + 3] = Convertnumber(ReceiveFrame[3], ReceiveFrame[4], ReceiveFrame[5])
                    print("Vaccu " + str(accuOffsetNo + 3) + ": " + str(df.VAccu8[accuOffsetNo + 3]))
                elif (i == 8):
                    df.VAccu9[accuOffsetNo] = Convertnumber(ReceiveFrame[12], ReceiveFrame[13], ReceiveFrame[14])
                    print("Vaccu " + str(accuOffsetNo) + ": " + str(df.VAccu9[accuOffsetNo]))
                    df.VAccu9[accuOffsetNo + 1] = Convertnumber(ReceiveFrame[9], ReceiveFrame[10], ReceiveFrame[11])
                    print("Vaccu " + str(accuOffsetNo + 1) + ": " + str(df.VAccu9[accuOffsetNo + 1]))
                    df.VAccu9[accuOffsetNo + 2] = Convertnumber(ReceiveFrame[6], ReceiveFrame[7], ReceiveFrame[8])
                    print("Vaccu " + str(accuOffsetNo + 2) + ": " + str(df.VAccu9[accuOffsetNo + 2]))
                    df.VAccu9[accuOffsetNo + 3] = Convertnumber(ReceiveFrame[3], ReceiveFrame[4], ReceiveFrame[5])
                    print("Vaccu " + str(accuOffsetNo + 3) + ": " + str(df.VAccu9[accuOffsetNo + 3]))
    # except:
    #     cl.ColorPrint(">Khong lay duoc du lieu!!!", cl.bcolors.FAIL)


def ReadDCParameter():
    try:
        ReceiveFrame = []
        ReceiveFrame = GetDataFrom("/dev/ttyUSB0", 100, 1, 1)
        print(ReceiveFrame)
        df.Temp[0] = Convertnumber(ReceiveFrame[3], ReceiveFrame[4], ReceiveFrame[5])
        df.Temp[1] = Convertnumber(ReceiveFrame[6], ReceiveFrame[7], ReceiveFrame[8])
        df.Temp[2] = Convertnumber(ReceiveFrame[9], ReceiveFrame[10], ReceiveFrame[11])
        df.IDevice[0] = Convertnumber(ReceiveFrame[12], ReceiveFrame[13], ReceiveFrame[14])
        df.IDevice[1] = Convertnumber(ReceiveFrame[15], ReceiveFrame[16], ReceiveFrame[17])
        df.IDevice[2] = Convertnumber(ReceiveFrame[18], ReceiveFrame[19], ReceiveFrame[20])
        df.VDC = Convertnumber(ReceiveFrame[21], ReceiveFrame[22], ReceiveFrame[23])
        df.inputByte[0] = ReceiveFrame[30]
        df.inputByte[1] = ReceiveFrame[29]
        df.inputEventByte[0] = ReceiveFrame[27]
        df.inputEventByte[1] = ReceiveFrame[26]
        print(df.Temp)
        print(df.IDevice)
        print(df.VDC)
    except:
        cl.ColorPrint(">Khong lay duoc du lieu!!!", cl.bcolors.FAIL)


def Adjust48VDC(
        portname,
        heso,
        timeout
):
    start = 0
    stop = 0
    address = 1
    FrameSize = 30
    dataFrame = [
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
    ]
    phan_nguyen = int(heso.split(".")[0])
    phan_du_1 = int(heso.split(".")[1]) // 256
    phan_du_2 = int(heso.split(".")[1]) % 256
    print('Phan nguyen la: ' + str(phan_nguyen))
    print('Phan du 1 la: ' + str(phan_du_1))
    print('Phan du 2 la: ' + str(phan_du_2))
    flagStartFrame = False
    flagStopFrame = False
    bytecouter = 0
    Step = 0
    index = 0
    start = time.time()
    if CheckPortAvalibale(portname):
        if df.Serialconnection == True:
            if df.ser.isOpen():
                # print ('Connected to port:' + portname)
                df.ser.write(struct.pack('>B', 0x40))
                df.ser.write(struct.pack('>B', 0x30))
                df.ser.write(struct.pack('>B', 0x30))
                cmds[0] = 100
                cmds[1] = 1
                cmds[2] = 5
                # cmds[3] = 22 #tinh toan he so byte 3 4 5
                # cmds[4] = phan_du_1
                # cmds[5] = phan_du_2
                cmds[3] = phan_nguyen
                cmds[4] = phan_du_1
                cmds[5] = phan_du_2
                for cmd in cmds:
                    df.ser.write(struct.pack('>B', cmd))
                # print ('Command sent')
                start = time.time()
                while time.time() - start < timeout:
                    if df.ser.inWaiting() > 0:
                        inchar = int.from_bytes(df.ser.read(1), 'big')
                        bytecouter += 1
                        if inchar == 0x40 and bytecouter == 1:
                            Step = 1
                        elif inchar == 0x30 and Step == 1 \
                                and bytecouter == 2:
                            Step = 2
                        elif inchar == 0x30 and Step == 2 \
                                and bytecouter == 3:
                            flagStartFrame = True
                            bytecouter = 0
                            index = 0
                        else:
                            bytecouter = 0
                            Step = 0
                        if flagStartFrame == True:
                            dataFrame[index] = inchar

                            # print(index)

                            index += 1
                        if index > FrameSize:
                            if dataFrame[1] == address:
                                flagStopFrame = True
                            index = 0
                            flagStartFrame = False
                            bytecouter = 0
                            Step = 0

                            # print(dataFrame)

                            return dataFrame
                            break
            else:
                # print(inchar)
                # print(len(cmds))

                print('Port is closed!!!')
                return None
        else:
            print('Try to reconnect to Serialport')
    else:
        if df.Serialconnection == True:
            df.Serialconnection = False
            df.ser.close()
        print('Port is not invalible!!!')
    return None


def AdjustLuoiAC(
        portname,
        heso,
        timeout
):
    start = 0
    stop = 0
    address = 1
    FrameSize = 30
    dataFrame = [
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
    ]
    phan_du_1 = int(heso) // 256
    phan_du_2 = int(heso) % 256
    print('Phan du 1 la: ' + str(phan_du_1))
    print('Phan du 2 la: ' + str(phan_du_2))
    flagStartFrame = False
    flagStopFrame = False
    bytecouter = 0
    Step = 0
    index = 0
    start = time.time()
    if CheckPortAvalibale(portname):
        if df.Serialconnection == True:
            if df.ser.isOpen():
                # print ('Connected to port:' + portname)
                df.ser.write(struct.pack('>B', 0x40))
                df.ser.write(struct.pack('>B', 0x30))
                df.ser.write(struct.pack('>B', 0x30))
                cmds[0] = 2
                cmds[1] = 1
                cmds[2] = 6
                # cmds[3] = 22 #tinh toan he so byte 3 4 5
                # cmds[4] = phan_du_1
                # cmds[5] = phan_du_2
                cmds[3] = phan_du_1
                cmds[4] = phan_du_2
                for cmd in cmds:
                    df.ser.write(struct.pack('>B', cmd))
                # print ('Command sent')
                start = time.time()
                while time.time() - start < timeout:
                    if df.ser.inWaiting() > 0:
                        inchar = int.from_bytes(df.ser.read(1), 'big')
                        bytecouter += 1
                        if inchar == 0x40 and bytecouter == 1:
                            Step = 1
                        elif inchar == 0x30 and Step == 1 \
                                and bytecouter == 2:
                            Step = 2
                        elif inchar == 0x30 and Step == 2 \
                                and bytecouter == 3:
                            flagStartFrame = True
                            bytecouter = 0
                            index = 0
                        else:
                            bytecouter = 0
                            Step = 0
                        if flagStartFrame == True:
                            dataFrame[index] = inchar

                            # print(index)

                            index += 1
                        if index > FrameSize:
                            if dataFrame[1] == address:
                                flagStopFrame = True
                            index = 0
                            flagStartFrame = False
                            bytecouter = 0
                            Step = 0

                            # print(dataFrame)

                            return dataFrame
                            break
            else:
                # print(inchar)
                # print(len(cmds))

                print('Port is closed!!!')
                return None
        else:
            print('Try to reconnect to Serialport')
    else:
        if df.Serialconnection == True:
            df.Serialconnection = False
            df.ser.close()
        print('Port is not invalible!!!')
    return None


def AdjustMayNoAC(
        portname,
        heso,
        timeout
):
    start = 0
    stop = 0
    address = 1
    FrameSize = 30
    dataFrame = [
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
    ]
    phan_du_1 = int(heso) // 256
    phan_du_2 = int(heso) % 256
    print('Phan du 1 la: ' + str(phan_du_1))
    print('Phan du 2 la: ' + str(phan_du_2))
    flagStartFrame = False
    flagStopFrame = False
    bytecouter = 0
    Step = 0
    index = 0
    start = time.time()
    if CheckPortAvalibale(portname):
        if df.Serialconnection == True:
            if df.ser.isOpen():
                # print ('Connected to port:' + portname)
                df.ser.write(struct.pack('>B', 0x40))
                df.ser.write(struct.pack('>B', 0x30))
                df.ser.write(struct.pack('>B', 0x30))
                cmds[0] = 2
                cmds[1] = 1
                cmds[2] = 5
                # cmds[3] = 22 #tinh toan he so byte 3 4 5
                # cmds[4] = phan_du_1
                # cmds[5] = phan_du_2
                cmds[3] = phan_du_1
                cmds[4] = phan_du_2
                for cmd in cmds:
                    df.ser.write(struct.pack('>B', cmd))
                # print ('Command sent')
                start = time.time()
                while time.time() - start < timeout:
                    if df.ser.inWaiting() > 0:
                        inchar = int.from_bytes(df.ser.read(1), 'big')
                        bytecouter += 1
                        if inchar == 0x40 and bytecouter == 1:
                            Step = 1
                        elif inchar == 0x30 and Step == 1 \
                                and bytecouter == 2:
                            Step = 2
                        elif inchar == 0x30 and Step == 2 \
                                and bytecouter == 3:
                            flagStartFrame = True
                            bytecouter = 0
                            index = 0
                        else:
                            bytecouter = 0
                            Step = 0
                        if flagStartFrame == True:
                            dataFrame[index] = inchar

                            # print(index)

                            index += 1
                        if index > FrameSize:
                            if dataFrame[1] == address:
                                flagStopFrame = True
                            index = 0
                            flagStartFrame = False
                            bytecouter = 0
                            Step = 0

                            # print(dataFrame)

                            return dataFrame
                            break
            else:
                # print(inchar)
                # print(len(cmds))

                print('Port is closed!!!')
                return None
        else:
            print('Try to reconnect to Serialport')
    else:
        if df.Serialconnection == True:
            df.Serialconnection = False
            df.ser.close()
        print('Port is not invalible!!!')
    return None


def AdjustAccu(
        portname,
        ID,  # Id cua Module
        type,  # loai binh( 2V ->0, 5V:12V->1)
        AccuNo,  # 1->4
        Vol,  # vol thuc te xx.xxx
        timeout
):
    start = 0
    stop = 0
    address = 1
    FrameSize = 30
    dataFrame = [
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
        None,
    ]
    phan_nguyen = int(Vol.split(".")[0])
    phan_du_1 = int(Vol.split(".")[1]) // 256
    phan_du_2 = int(Vol.split(".")[1]) % 256
    print('Phan nguyen la: ' + str(phan_nguyen))
    print('Phan du 1 la: ' + str(phan_du_1))
    print('Phan du 2 la: ' + str(phan_du_2))
    flagStartFrame = False
    flagStopFrame = False
    bytecouter = 0
    Step = 0
    index = 0
    start = time.time()
    if CheckPortAvalibale(portname):
        if df.Serialconnection == True:
            if df.ser.isOpen():
                # print ('Connected to port:' + portname)
                cmds[0] = ID
                cmds[1] = 1
                cmds[2] = type * 16 + AccuNo + 1
                # cmds[3] = 22 #tinh toan he so byte 3 4 5
                # cmds[4] = phan_du_1
                # cmds[5] = phan_du_2
                cmds[3] = phan_nguyen
                cmds[4] = phan_du_1
                cmds[5] = phan_du_2
                df.ser.write(struct.pack('>B', 0x40))
                df.ser.write(struct.pack('>B', 0x30))
                df.ser.write(struct.pack('>B', 0x30))
                for cmd in cmds:
                    df.ser.write(struct.pack('>B', cmd))
                # print ('Command sent')
            else:
                # print(inchar)
                # print(len(cmds))

                print('Port is closed!!!')
                return None
        else:
            print('Try to reconnect to Serialport')
    else:
        if df.Serialconnection == True:
            df.Serialconnection = False
            df.ser.close()
        print('Port is not invalible!!!')
    return None
