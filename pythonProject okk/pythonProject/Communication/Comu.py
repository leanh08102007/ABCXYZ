#!/usr/bin/python
# -*- coding: utf-8 -*-
import time
import serial
import struct
import serial.tools.list_ports
from Communication import define as df
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
                print ('Serial port is openned!')
                df.Serialconnection = True
        except:
            print ('error to open serial port')


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

                print ('Port is closed!!!')
                return None
        else:
            print ('Try to reconnect to Serialport')
    else:
        if df.Serialconnection == True:
            df.Serialconnection = False
            df.ser.close()
        print ('Port is not invalible!!!')

    return None


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
                cmds[0] = 100
                cmds[1] = 1
                cmds[2] = 2
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

                print ('Port is closed!!!')
                return None
        else:
            print ('Try to reconnect to Serialport')
    else:
        if df.Serialconnection == True:
            df.Serialconnection = False
            df.ser.close()
        print ('Port is not invalible!!!')

    return None
def DigitalWrite(OutNo,State):
    print(ControlOutput("/dev/ttyUSB0", OutNo, 1, State, 1, 1))
def DigitalWriteTiming(OutNo,Time):
    print(ControlOutput("/dev/ttyUSB0", OutNo, 2, 1, Time, 1))
def Convertnumber(arg1,arg2,arg3):
    NumFloat=0
    NumFloat = arg1 << 8
    NumFloat=NumFloat+arg2
    for i in range(arg3):
        NumFloat=NumFloat/10
    return round(NumFloat,2)
def ReadACParameter():
    ReceiveFrame=[]
    ReceiveFrame=GetDataFrom("/dev/ttyUSB0", 2, 2, 1)
    # print(ReceiveFrame)
    df.VAC[0]=Convertnumber(ReceiveFrame[3],ReceiveFrame[4],ReceiveFrame[5])
    df.VAC[1] = Convertnumber(ReceiveFrame[6], ReceiveFrame[7], ReceiveFrame[8])
    df.VAC[2] = Convertnumber(ReceiveFrame[9], ReceiveFrame[10], ReceiveFrame[11])
    df.IAC[0]=Convertnumber(ReceiveFrame[12], ReceiveFrame[13], ReceiveFrame[14])
    df.IAC[1] = Convertnumber(ReceiveFrame[15], ReceiveFrame[16], ReceiveFrame[17])
    df.IAC[2] = Convertnumber(ReceiveFrame[18], ReceiveFrame[19], ReceiveFrame[20])
    df.FAC[0] = Convertnumber(ReceiveFrame[21], ReceiveFrame[22], ReceiveFrame[23])
    df.FAC[1] = Convertnumber(ReceiveFrame[24], ReceiveFrame[25], ReceiveFrame[26])
    df.FAC[2] = Convertnumber(ReceiveFrame[27], ReceiveFrame[28], ReceiveFrame[29])
    ReceiveFrame = GetDataFrom("/dev/ttyUSB0", 2, 1, 1)
    df.VAC[3] = Convertnumber(ReceiveFrame[3], ReceiveFrame[4], ReceiveFrame[5])
    df.VAC[4] = Convertnumber(ReceiveFrame[6], ReceiveFrame[7], ReceiveFrame[8])
    df.VAC[5] = Convertnumber(ReceiveFrame[9], ReceiveFrame[10], ReceiveFrame[11])
    df.VAccuMayPhat=Convertnumber(ReceiveFrame[12], ReceiveFrame[13], ReceiveFrame[14])
    df.FAC[3] = Convertnumber(ReceiveFrame[15], ReceiveFrame[16], ReceiveFrame[17])
    df.FAC[4] = Convertnumber(ReceiveFrame[18], ReceiveFrame[19], ReceiveFrame[20])
    df.FAC[5] = Convertnumber(ReceiveFrame[21], ReceiveFrame[22], ReceiveFrame[23])
    # print(ReceiveFrame)
    # print(" AC0 AC1 AC2 AC3 AC4 AC5")
    print(df.VAC)
    print(df.IAC)
    print(df.FAC)
    print(df.VAccuMayPhat)
def ReadDCParameter():
    ReceiveFrame = []
    ReceiveFrame = GetDataFrom("/dev/ttyUSB0", 100, 1, 1)
    # print(ReceiveFrame)
    df.Temp[0] = Convertnumber(ReceiveFrame[3], ReceiveFrame[4], ReceiveFrame[5])
    df.Temp[1] = Convertnumber(ReceiveFrame[6], ReceiveFrame[7], ReceiveFrame[8])
    df.Temp[2] = Convertnumber(ReceiveFrame[9], ReceiveFrame[10], ReceiveFrame[11])
    df.IDevice[0] = Convertnumber(ReceiveFrame[12], ReceiveFrame[13], ReceiveFrame[14])
    df.IDevice[1] = Convertnumber(ReceiveFrame[15], ReceiveFrame[16], ReceiveFrame[17])
    df.IDevice[2] = Convertnumber(ReceiveFrame[18], ReceiveFrame[19], ReceiveFrame[20])
    df.VDC = Convertnumber(ReceiveFrame[21], ReceiveFrame[22], ReceiveFrame[23])
    print(df.Temp)
    print(df.IDevice)
    print(df.VDC)
