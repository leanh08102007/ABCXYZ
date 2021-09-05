import time

from Mylib.Communication import Function, Comu
from Mylib.Communication import define as df
from Mylib.Debug import TermColor as cl


def pushData():
    # Function.publish(Function.client, str(df.id_tram) + "_" + df.topic_data_IO_AC, Function.combineData_IO_AC())
    # for i in range(len(df.accu_config)):
    #     if df.accu_config[i] != 0 and df.accu_config[i] is not None:
    #         Function.publish(Function.client, str(df.id_tram) + "_" + df.topic_data_accu[i],
    #                          Function.combineData_accu(i))

    pub_data_io_ac_status = Function.publish(Function.client, df.topic_data_IO_AC, Function.combineData_IO_AC())
    if pub_data_io_ac_status == "0":
        return "0"
    for i in range(len(df.accu_config)):
        if df.accu_config[i] != 0 and df.accu_config[i] is not None:
            pub_data_accu_status = Function.publish(Function.client, df.topic_data_accu[i],
                                                    Function.combineData_accu(i))
            if pub_data_accu_status == "0":
                return "0"
    return "1"


def control_relay(device, state):
    try:
        if df.relay_config[device - 1] == 0 or df.relay_config[device - 1] is None:
            print("Control relay" + str(device) + " with state: " + state)
            if state == '1':
                if Comu.DigitalWrite(device - 1, 0) is None:
                    cl.ColorPrint("Controlling relay not successfully, try again later!", cl.bcolors.FAIL)
                    return
            elif state == '0':
                if Comu.DigitalWrite(device - 1, 1) is None:
                    cl.ColorPrint("Controlling relay not successfully, try again later!", cl.bcolors.FAIL)
                    return
            cl.ColorPrint("Controlling relay successfully!", cl.bcolors.OKBLUE)
            str_id_tram = str(df.id_tram)
            str_device = str(device)
            str_state = str(state)
            json_string = "{\"id_tram\":\"" + str_id_tram + "\", \"device\":\"" + str_device + "\", \"state\":\"" + str_state + "\"}"
            cl.ColorPrint(json_string, cl.bcolors.WARNING)
            Function.publish(Function.client, df.topic_update_relay_state, json_string)

        elif int(df.relay_config[device - 1]) >= 1:
            if Comu.DigitalWriteTiming(device - 1, int(df.relay_config[device - 1])) is not None:
                cl.ColorPrint(">Start control relay timer", cl.bcolors.OKGREEN)
                time.sleep(int(df.relay_config[device - 1]))
                cl.ColorPrint(">Stop control relay timer", cl.bcolors.OKGREEN)
            else:
                cl.ColorPrint(">Dieu khien relay timer khong thanh cong!!!", cl.bcolors.FAIL)
    except:
        cl.ColorPrint("Controlling relay not successfully, try again later! in except", cl.bcolors.FAIL)


def update_current_version_to_server():
    Function.publish(Function.client, df.topic_update_current_version,
                     "{\"id_tram\":\"" + str(df.id_tram) + "\", \"current_version\":\"" + df.current_version + "\"}")


def update_ip_tram_to_server():
    Function.publish(Function.client, df.topic_update_ip_tram,
                     "{\"id_tram\":\"" + str(df.id_tram) + "\", \"ip_tram\":\"" + str(df.myIP) + "\"}")
