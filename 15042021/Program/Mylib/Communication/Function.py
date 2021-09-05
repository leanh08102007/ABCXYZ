import json
import random

from Mylib.Communication import Comu
from Mylib.Communication import dataTransmission as dt
from Mylib.Communication import define as df
# mqtt
from Mylib.Debug import TermColor as cl
from paho.mqtt import client as mqtt_client
from serial import *

client_id = f'python-mqtt-{random.randint(0, 1000)}'
broker = '10.39.130.12'
port = 1883


def connect_mqtt():
    def on_connect(client, userdata, flags, rc):
        if rc == 0:
            print("Connected to MQTT Broker!")
        else:
            print("Failed to connect, return code %d\n", rc)

    client = mqtt_client.Client(client_id, False)
    # client = mqtt_client.Client(client_id)
    client.username_pw_set(df.username, df.password)
    client.on_connect = on_connect
    client.connect(broker, port)
    return client


def publish(client, topicName, topicValue):
    try:
        time.sleep(1)
        result = client.publish(topicName, topicValue)
        status = result[0]
        if status == 0:
            print(f"Send `{topicValue}` to topic `{topicName}`")
            return "1"
        else:
            print(f"Failed to send message to topic {topicName}")
    except:
        cl.ColorPrint(">Khong the ket noi den broker", cl.bcolors.FAIL)
    return "0"


# end mqtt

def combineData_IO_AC():
    CombineInput()
    CombineInputEvent()
    CompareInputEvent()
    values = "{" + "\"id_tram\"" + ":\"" + str(df.id_tram) + "\", " + "\"Temp1\"" + ":\"" + str(
        df.Temp[0]) + "\", " + "\"Temp2\"" + ":\"" + str(df.Temp[1]) + "\", " + "\"Temp3\"" + ":\"" + str(
        df.Temp[2]) + "\", " + "\"VAC1\"" + ":\"" + str(df.VAC[0]) + "\", " + "\"VAC2\"" + ":\"" + str(
        df.VAC[1]) + "\", " + "\"VAC3\"" + ":\"" + str(df.VAC[2]) + "\", " + "\"VAC4\"" + ":\"" + str(
        df.VAC[3]) + "\", " + "\"VAC5\"" + ":\"" + str(df.VAC[4]) + "\", " + "\"VAC6\"" + ":\"" + str(
        df.VAC[5]) + "\", " + "\"FAC1\"" + ":\"" + str(df.FAC[0]) + "\", " + "\"FAC2\"" + ":\"" + str(
        df.FAC[1]) + "\", " + "\"FAC3\"" + ":\"" + str(df.FAC[2]) + "\", " + "\"FAC4\"" + ":\"" + str(
        df.FAC[3]) + "\", " + "\"FAC5\"" + ":\"" + str(df.FAC[4]) + "\", " + "\"FAC6\"" + ":\"" + str(
        df.FAC[5]) + "\", " + "\"VDC\"" + ":\"" + str(df.VDC) + "\", " + "\"IAC1\"" + ":\"" + str(
        df.IAC[0]) + "\", " + "\"IAC2\"" + ":\"" + str(df.IAC[1]) + "\", " + "\"IAC3\"" + ":\"" + str(
        df.IAC[2]) + "\", " + "\"IDevice1\"" + ":\"" + str(df.IDevice[0]) + "\", " + "\"IDevice2\"" + ":\"" + str(
        df.IAC[1]) + "\", " + "\"IDevice3\"" + ":\"" + str(
        df.IDevice[2]) + "\", " + "\"input\"" + ":\"" + str(df.input) + "\", " + "\"VAccuMayPhat\"" + ":\"" + str(
        df.VAccuMayPhat) + "\"}"

    return values


def combineData_accu(num):
    values_header = "{" + "\"id_tram\":\"" + str(df.id_tram) + "\", \"id_to\":\"" + str(num + 1) + "\", "
    values = ""
    if num == 0:
        for i in range(24):
            values = values + "\"accu" + str(i) + "\":\"" + str(df.VAccu1[i]) + "\", "
    elif num == 1:
        for i in range(24):
            values = values + "\"accu" + str(i) + "\":\"" + str(df.VAccu2[i]) + "\", "
    elif num == 2:
        for i in range(24):
            values = values + "\"accu" + str(i) + "\":\"" + str(df.VAccu3[i]) + "\", "
    elif num == 3:
        for i in range(24):
            values = values + "\"accu" + str(i) + "\":\"" + str(df.VAccu4[i]) + "\", "
    elif num == 4:
        for i in range(24):
            values = values + "\"accu" + str(i) + "\":\"" + str(df.VAccu5[i]) + "\", "
    elif num == 5:
        for i in range(24):
            values = values + "\"accu" + str(i) + "\":\"" + str(df.VAccu6[i]) + "\", "
    elif num == 6:
        for i in range(24):
            values = values + "\"accu" + str(i) + "\":\"" + str(df.VAccu7[i]) + "\", "
    elif num == 7:
        for i in range(24):
            values = values + "\"accu" + str(i) + "\":\"" + str(df.VAccu8[i]) + "\", "
    elif num == 8:
        for i in range(24):
            values = values + "\"accu" + str(i) + "\":\"" + str(df.VAccu9[i]) + "\", "
    elif num == 9:
        for i in range(24):
            values = values + "\"accu" + str(i) + "\":\"" + str(df.VAccu10[i]) + "\", "
    values = values_header + values[0:-2] + "}"
    return values


def subscribe(client: mqtt_client):
    if df.id_tram == 0:
        client.subscribe(str(df.GatewayIP) + "_" + df.topic_receive_config)
    client.subscribe(df.topic_data_control)
    client.subscribe(df.topic_update_relay_state_status)

    client.subscribe(df.topic_receive_relay_state)
    client.on_message = on_message


def on_message(client, userdata, msg):
    df.ser.close()
    df.ser.open()
    msg_decode = msg.payload.decode()
    data_receive = json.loads(msg_decode)

    # Read config from server
    if msg.topic == str(df.GatewayIP) + "_" + df.topic_receive_config:
        read_config(data_receive)

    # Control relay
    elif msg.topic == df.topic_data_control and str(data_receive['id_tram']) == str(df.id_tram):
        df.is_comu_busy = True
        device = int(data_receive['device'])
        state = data_receive['state']
        print(f"Control relay `{device}` with state `{state}`")
        dt.control_relay(device, state)
        df.is_comu_busy = False

    # Update status return from server
    elif msg.topic == df.topic_update_relay_state_status and str(data_receive['id_tram']) == str(df.id_tram):
        update_status = int(data_receive['update_status'])
        if update_status == 1:
            cl.ColorPrint("Update status relay to server successfully!", cl.bcolors.OKBLUE)
        elif update_status == 0:
            df.updateStatusPending = True
            cl.ColorPrint("Update status relay to server not successfully, try again later!", cl.bcolors.FAIL)

    # Read state relay again
    elif msg.topic == df.topic_receive_relay_state and str(data_receive[0]['id_tram']) == str(df.id_tram):
        print(data_receive)
        # if "name" in data_receive:
        df.updateStatusPending = False
        for i in range(len(data_receive)):
            deviceNo = int(data_receive[i]["name"].replace("device", ""))
            if df.relay_config[deviceNo - 1] == 0:
                if int(data_receive[i]["status"]) == 1:
                    if Comu.DigitalWrite(deviceNo - 1, 0) is not None:
                        print("Controling relay" + str(deviceNo) + " ON")
                    else:
                        df.updateStatusPending = True
                elif int(data_receive[i]["status"]) == 0:
                    if Comu.DigitalWrite(deviceNo - 1, 1) is not None:
                        print("Controling relay" + str(deviceNo) + " OFF")
                    else:
                        df.updateStatusPending = True


def read_config(data_receive):
    cl.ColorPrint("Read config in server by MQTT!!!", cl.bcolors.OKGREEN)
    relay_name = ['RL1', "RL2", "RL3", "RL4", "RL5", "RL6", "RL7", "RL8", "RL9", "RL10"]
    accu_name = ["to_accu1", "to_accu2", "to_accu3", "to_accu4", "to_accu5", "to_accu6", "to_accu7", "to_accu8",
                 "to_accu9", "to_accu10"]
    relay_config = ["ON/OFF", "ON/OFF", "ON/OFF", "ON/OFF", "ON/OFF", "ON/OFF", "ON/OFF", "ON/OFF",
                    "ON/OFF",
                    "ON/OFF"]
    df.StationName = data_receive[0]["StationName"].strip()
    df.id_tram = data_receive[0]["id_tram"].strip()
    df.UpdateDatabaseCycle = int(data_receive[0]["time_put_infor"].strip())

    cl.ColorPrint(">Station name is: " + df.StationName, cl.bcolors.WARNING)
    cl.ColorPrint(">Station id is: " + df.id_tram, cl.bcolors.WARNING)
    cl.ColorPrint(">Time push data is: " + str(df.UpdateDatabaseCycle), cl.bcolors.WARNING)
    for i in range(10):
        tmp = data_receive[0][relay_name[i]]
        if tmp is not None:
            df.relay_config[i] = float(data_receive[0][relay_name[i]].strip())
            if df.relay_config[i] > 0:
                relay_config[i] = "Timing "
        else:
            df.relay_config[i] = None

    for i in range(10):
        tmp = data_receive[0][accu_name[i]]
        if tmp is not None:
            df.accu_config[i] = int(data_receive[0][accu_name[i]].strip())
        else:
            df.accu_config[i] = None
    print(relay_name)
    print(relay_config)
    print("Accu config: ")
    print(df.accu_config)

    ##################### Read config input #####################
    if "stt" in data_receive:
        if int(data_receive[1]["stt"]) < 11:
            df.input_config[int(data_receive[1]["stt"]) - 1] = int(data_receive[1]["cb_on"])
        else:
            df.input_config[int(data_receive[1]["stt"]) + 5] = int(data_receive[1]["cb_on"])
        print("Input config: ")
        print(df.input_config)

    ##################### Read relay state #####################
    if "name" in data_receive:
        if "stt" in data_receive:
            df.updateStatusPending = False
            for i in range(str(data_receive).count('{') - 2):
                deviceNo = int(data_receive[i + 2]["name"].replace("device", ""))
                if df.relay_config[deviceNo - 1] == 0:
                    if int(data_receive[i + 2]["status"]) == 1:
                        if Comu.DigitalWrite(deviceNo - 1, 0) is not None:
                            print("Controlling relay" + str(deviceNo) + " ON")
                        else:
                            df.updateStatusPending = True
        else:
            df.updateStatusPending = False
            for i in range(str(data_receive).count('{') - 1):
                deviceNo = int(data_receive[i + 1]["name"].replace("device", ""))
                if df.relay_config[deviceNo - 1] == 0:
                    if int(data_receive[i + 1]["status"]) == 1:
                        if Comu.DigitalWrite(deviceNo - 1, 0) is not None:
                            print("Controlling relay" + str(deviceNo) + " ON")
                        else:
                            df.updateStatusPending = True


def CombineInput():
    if df.inputByte[2] != -1 and df.inputByte[2] != None:
        df.input = df.inputByte[2]
    else:
        df.input = 0
    if df.inputByte[1] != -1 and df.inputByte[1] != None:
        df.input = (df.input << 8) + df.inputByte[1]
    else:
        df.input = (df.input << 8) + 0
    if df.inputByte[0] != -1 and df.inputByte[0] != None:
        df.input = (df.input << 8) + df.inputByte[0]
    else:
        df.input = (df.input << 8) + 0
    print(bin(df.input)[2:])
    bin_string = bin(df.input)[2:]
    bin_string = bin_string[::-1]
    index = 0
    for i in bin_string:
        df.inputByteBin[index] = int(i)
        index = index + 1
    print(df.inputByteBin)


def CombineInputEvent():
    print("Input event")
    for i in range(len(df.inputEventByteBin)):
        df.inputEventByteBin[i] = None
    if df.inputEventByte[2] != -1 and df.inputEventByte[2] != None:
        df.inputEvent = df.inputEventByte[2]
    else:
        df.inputEvent = 0
    if df.inputEventByte[1] != -1 and df.inputEventByte[1] != None:
        df.inputEvent = (df.inputEvent << 8) + df.inputEventByte[1]
    else:
        df.inputEvent = (df.inputEvent << 8) + 0
    if df.inputEventByte[0] != -1 and df.inputEventByte[0] != None:
        df.inputEvent = (df.inputEvent << 8) + df.inputEventByte[0]
    else:
        df.inputEvent = (df.inputEvent << 8) + 0
    print(bin(df.inputEvent)[2:])
    bin_string = bin(df.inputEvent)[2:]
    bin_string = bin_string[::-1]
    index = 0
    for i in bin_string:
        df.inputEventByteBin[index] = int(i)
        index = index + 1
    print(df.inputEventByteBin)


def CompareInputEvent():
    for i in range(24):
        if df.inputEventByteBin[i] == 1:
            if df.input_config[i] == 1:
                df.inputByteBin[i] = 0
            else:
                df.inputByteBin[i] = 1
    print("inputByteBin")
    print(df.inputByteBin)
    df.input = 0
    for i in range(len(df.inputByteBin)):
        df.input = (df.input << 1) + df.inputByteBin[len(df.inputByteBin) - i - 1]
    print("trang thai input")
    print(df.input)
    print("###########################")
