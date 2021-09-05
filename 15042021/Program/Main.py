import re
import shlex
import socket
import subprocess
from threading import Thread

from Mylib.Communication import Comu
from Mylib.Communication import Function
from Mylib.Communication import SqlQuery
from Mylib.Communication import dataTransmission as dt
from Mylib.Communication import define as df
from Mylib.Debug import TermColor as cl
from flask import Flask
from flask import request
from serial import *

Comu.ConnectSerialPort("/dev/ttyUSB0")
app = Flask(__name__)


@app.route("/<name>")
def index(name):
    rcvUlr = request.full_path
    deviceNoStr = None
    deviceNoInt = None
    deviceSt = None
    cl.ColorPrint(rcvUlr, cl.bcolors.OKBLUE)

    if 'DC_adjust_request' in rcvUlr:
        heso = rcvUlr.split('?')[1]
        print(heso)
        str_ret_adjust_DC = Comu.Adjust48VDC("/dev/ttyUSB0", heso, 1)
        if str_ret_adjust_DC is not None:
            return '1'
        return '0'

    if 'AC_luoi_adjust_request' in rcvUlr:
        heso = rcvUlr.split('?')[1]
        print(heso)
        str_ret_adjust_AC_luoi = Comu.AdjustLuoiAC("/dev/ttyUSB0", heso, 1)
        if str_ret_adjust_AC_luoi is not None:
            return '1'
        return '0'

    if 'AC_mayno_adjust_request' in rcvUlr:
        heso = rcvUlr.split('?')[1]
        print(heso)
        str_ret_adjust_AC_mayno = Comu.AdjustMayNoAC("/dev/ttyUSB0", heso, 1)
        if str_ret_adjust_AC_mayno is not None:
            return '1'
        return '0'

    if 'accu_adjust_request' in rcvUlr:
        module_id = rcvUlr.split('?')[1]
        type = rcvUlr.split('?')[2]
        accu_no = rcvUlr.split('?')[3]
        vol_accu = rcvUlr.split('?')[4]
        print(module_id)
        print(type)
        print(accu_no)
        print(vol_accu)
        str_ret_adjust_accu = Comu.AdjustAccu("/dev/ttyUSB0", int(module_id), int(type), int(accu_no), vol_accu, 1)
        return '1'

    if 'data_adjust_request' in rcvUlr:
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

    if 'accu_request_value' in rcvUlr:
        print(rcvUlr)
        str_num = rcvUlr.split('?')[1]
        num = int(str_num)
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

    if 'restart' in rcvUlr:
        print("restart")
        os._exit(0)
        return "Restart"
    if 'readconfig' in rcvUlr:
        SqlQuery.ReadConfigFromServer()
        return "1"
    if '~id_thietbi' in rcvUlr:
        device = rcvUlr.split('?')
        deviceNoStr = re.sub(r'\D', "", device[1])
        if deviceNoStr != '':
            deviceNoInt = int(deviceNoStr)
            if deviceNoInt in range(1, 11):
                deviceSt = device[1].split(deviceNoStr)[1]
                df.RelayControlFlag[deviceNoInt - 1] = True
                if 'on' in deviceSt:
                    df.RelaySet[deviceNoInt - 1] = "ON"
                    print("set ON")
                if 'off' in deviceSt:
                    df.RelaySet[deviceNoInt - 1] = "OFF"
                    print("set OFF")
                return "Control device " + deviceNoStr + " " + deviceSt
            else:
                return "Thiet bi khong ton tai"
        else:
            return "Sai cu phap dieu khien"
    else:
        return "Lenh khong dung"

    # print(url_for('/~id_thietbi', next='/'))


class FlaskThread(Thread):
    def run(self):
        app.run(debug=True, host=df.myIP, port=df.WebserverPort, use_debugger=True, use_reloader=False)


###################
df.myIP = [l for l in ([ip for ip in socket.gethostbyname_ex(socket.gethostname())[2]
                        if not ip.startswith("127.")][:1], [[(s.connect(('8.8.8.8', 53)),
                                                              s.getsockname()[0], s.close()) for s in
                                                             [socket.socket(socket.AF_INET,
                                                                            socket.SOCK_DGRAM)]][0][1]]) if l][0][0]
cl.ColorPrint(">Host IP is:" + df.myIP, cl.bcolors.OKBLUE)
df.platform = sys.platform


def getIPGateway():
    gateway = None
    if df.platform == "linux":
        strs = subprocess.check_output(shlex.split('ip r l'))
        afterSplit = strs.decode("utf-8").split("\n")
        gateway = afterSplit[0].split('default via')[-1].split()[0]
    return str(gateway)


df.GatewayIP = getIPGateway()
cl.ColorPrint(">Gateway at:" + df.GatewayIP, cl.bcolors.OKBLUE)
df.username = df.GatewayIP


# cl.ColorPrint(">Username is:" + df.username, cl.bcolors.OKGREEN)
# test code here
################


def main():
    # mqtt
    connect_network()
    # Function.client = Function.connect_mqtt()
    # Function.client.loop_start()
    # Function.subscribe(Function.client)
    if df.GatewayIP == 'None' or df.GatewayIP == '':
        print("Gateway does not found...")
        return
    while df.id_tram is None or df.id_tram == 0:
        cl.ColorPrint("Getting config from server...", cl.bcolors.BOLD)
        Function.publish(Function.client, df.topic_request_config, "{\"gateway\":\"" + df.GatewayIP + "\"}")
        time.sleep(2)
    # end mqtt
    dt.update_current_version_to_server()
    dt.update_ip_tram_to_server()

    Function.publish(Function.client, df.topic_request_relay_state,
                     "{\"id_tram\":\"" + str(df.id_tram) + "\"}")

    cl.ColorPrint(">in main", cl.bcolors.BOLD)
    start_time = time.time()

    while True:
        if not df.is_comu_busy:
            if time.time() - start_time >= df.UpdateDatabaseCycle:
                cl.ColorPrint(str(time.time() - start_time), cl.bcolors.FAIL)
                start_time = time.time()
                cl.ColorPrint("############ AC ##############", cl.bcolors.WARNING)
                # Comu.ReadACParameter()
                cl.ColorPrint("############ DC ##############", cl.bcolors.WARNING)
                # Comu.ReadDCParameter()
                cl.ColorPrint("############ Accu ##############", cl.bcolors.WARNING)
                Comu.ReadAccuParameter()
                # cl.ColorPrint("############ Test adjust DC ##############", cl.bcolors.WARNING)
                # Comu.AdjustLuoiAC("/dev/ttyUSB0", '435', 1)
                # Comu.Adjust48VDC("/dev/ttyUSB0", '22.2350', 1)
                # Comu.AdjustAccu("/dev/ttyUSB0", 10, 1, 2, '6.333', 1)
                pub_status = dt.pushData()
                if pub_status == "0":
                    connect_network()
                if df.updateStatusPending:
                    Function.publish(Function.client, df.topic_request_relay_state,
                                     "{\"id_tram\":\"" + str(df.id_tram) + "\"}")


# def is_started():
#     full_path = "/home/idr/Desktop/LocalProgram/Program/"
#     print(os.path.exists(full_path + "Main.py"))
#     if not os.path.exists(full_path + "Main.py"):
#         return "1"
#     return "0"


def connect_network():
    Function.client = Function.connect_mqtt()
    Function.client.loop_start()
    Function.subscribe(Function.client)


if __name__ == '__main__':
    print(sys.platform)
    if df.platform == 'linux':
        server = FlaskThread()
        server.daemon = True
        server.start()  # start web server
        print(">Run")
        main()
