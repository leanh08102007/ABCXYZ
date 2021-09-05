import pyodbc
import time
from Mylib.Communication import define as df
from Mylib.Debug import TermColor as cl
from Mylib.Communication import Comu

# server ='10.39.56.62'
# database = 'master'
# username= 'gsnt'
# password = '123456'
server = '10.39.130.12'
database = 'OMC_LOG_V3'
username = 'sa'
password = 'Vnpt@2015'

sqlDriver = ''
cnxn = None
cur = None


def ListUpDriver():
    for driver in pyodbc.drivers():
        print(driver)


def ExecuteQuery(query):
    row = None
    if df.platform == 'linux':
        sqlDriver = 'FreeTDS'
    elif df.platform == 'win32':
        sqlDriver = 'ODBC Driver 17 for SQL Server'
    conStr = 'DRIVER=' + sqlDriver + ';' \
                                     'SERVER=' + server + ';' \
                                                          'PORT=1433;' \
                                                          'UID=' + username + ';' \
                                                                              'PWD=' + password
    # print (conStr)
    try:
        cnxn = pyodbc.connect(conStr)
        cur = cnxn.cursor()
        queryStr = query
        cur.execute(queryStr)
        row = cur.fetchall()
        cur.close()
        cnxn.close()
    except pyodbc.Error as ex:
        print(ex)
        cl.ColorPrint(">Khong the ke noi den server hoac khong tim thay database", cl.bcolors.FAIL)
    return row


def UpdateDataToDB1():
    insert_query = "INSERT INTO templog_v3 "
    colunme = "("
    for i in range(len(df.ListColumeData)):
        if i == 0:
            colunme = colunme + df.ListColumeData[i]
        else:
            colunme = colunme + ', ' + df.ListColumeData[i]
    colunme = colunme + ')'
    insert_query = insert_query + colunme + " VALUES("
    values = ('38', 'SYSDATETIME()', 'SYSDATETIME()',
              df.Temp[0], df.Temp[1], df.Temp[2],
              df.VAC[0], df.VAC[1], df.VAC[2], df.VAC[3], df.VAC[4], df.VAC[5],
              df.FAC[0], df.FAC[1], df.FAC[2], df.FAC[3], df.FAC[4], df.FAC[5],
              df.VDC)

    for i in range(len(values)):
        if i == 0:
            insert_query = insert_query + '%s'
        else:
            insert_query = insert_query + ', %s'
    insert_query = insert_query + ");"
    print(insert_query)
    if df.platform == 'linux':
        sqlDriver = 'FreeTDS'
    elif df.platform == 'win32':
        sqlDriver = 'ODBC Driver 17 for SQL Server'
    conStr = 'DRIVER=' + sqlDriver + ';' \
                                     'SERVER=' + server + ';' \
                                                          'PORT=1433;' \
                                                          'DATABASE=' + database + ';' \
                                                                                   'UID=' + username + ';' \
                                                                                                       'PWD=' + password
    # print (conStr)
    try:
        cnxn = pyodbc.connect(conStr)
        cur = cnxn.cursor()
        cur.execute(insert_query % values)
        cnxn.commit()
        cur.close()
        cnxn.close()
    except:
        cl.ColorPrint(">Khong the ke noi den server hoac khong tim thay database", cl.bcolors.FAIL)
    # print(ExecuteQuery("SELECT * from Config where Gateway IN ('10.39.56.62')"))


def UpdateDataToDB2():
    insert_query = "INSERT INTO data_value2 "
    colunme = "("
    for i in range(len(df.ListColumeData2)):
        if i == 0:
            colunme = colunme + df.ListColumeData2[i]
        else:
            colunme = colunme + ', ' + df.ListColumeData2[i]
    colunme = colunme + ')'
    insert_query = insert_query + colunme + " VALUES("
    values = ('SYSDATETIME()', '38',
              df.IAC[0], df.IAC[1], df.IAC[2],
              df.IDevice[0], df.IDevice[1], df.IDevice[2],
              df.input,
              df.VAccuMayPhat)
    for i in range(len(values)):
        if i == 0:
            insert_query = insert_query + '%s'
        else:
            insert_query = insert_query + ', %s'
    insert_query = insert_query + ");"
    print(insert_query)
    if df.platform == 'linux':
        sqlDriver = 'FreeTDS'
    elif df.platform == 'win32':
        sqlDriver = 'ODBC Driver 17 for SQL Server'
    conStr = 'DRIVER=' + sqlDriver + ';' \
                                     'SERVER=' + server + ';' \
                                                          'PORT=1433;' \
                                                          'DATABASE=' + database + ';' \
                                                                                   'UID=' + username + ';' \
                                                                                                       'PWD=' + password
    # print (conStr)
    try:
        cnxn = pyodbc.connect(conStr)
        cur = cnxn.cursor()
        cur.execute(insert_query % values)
        cnxn.commit()
        cur.close()
        cnxn.close()
    except:
        cl.ColorPrint(">Khong the ke noi den server hoac khong tim thay database", cl.bcolors.FAIL)


def ReadConfigFromServer():
    retData = None
    sqlData = None
    nameDevice = ['Device1', "device2", "device3", "device4", "device5", "device6", "device7", "device8", "device9",
                  "device10"]
    sttDevice = ['ON/OFF ', "ON/OFF ", "ON/OFF ", "ON/OFF ", "ON/OFF ", "ON/OFF ", "ON/OFF ", "ON/OFF ", "ON/OFF ",
                 "ON/OFF "]
    print("> Kiem tra cau hinh phan cung tu server")
    if df.GatewayIP != 'None':
        retData = ExecuteQuery(
            "SELECT * from OMC_LOG_V3.dbo.Config where Gateway IN ('" + df.GatewayIP.replace(" ", "") + "')")
    else:
        retData = ExecuteQuery("SELECT * from OMC_LOG_V3.dbo.Config where Gateway IN ('10.39.253.113')")
        # retData = ExecuteQuery("SELECT * FROM OMC_LOG_V3.dbo.Config")
    if retData != None:
        if len(retData) == 1:
            sqlData = retData[0]
            print(sqlData)
            df.StationName = sqlData[2].strip()
            df.id_tram = sqlData[3].strip()
            df.UpdateDatabaseCycle = int(sqlData[26].strip())
            cl.ColorPrint(">Ten Tram: " + df.StationName, cl.bcolors.WARNING)
            cl.ColorPrint(">Id tram: " + df.id_tram, cl.bcolors.WARNING)
            cl.ColorPrint(">time put info: " + str(df.UpdateDatabaseCycle), cl.bcolors.WARNING)
            for i in range(10):
                tmp = sqlData[i + 4]
                if tmp != None:
                    df.relay_config[i] = float(sqlData[i + 4].strip())
                    if df.relay_config[i] > 0:
                        sttDevice[i] = "Timing "
                else:
                    df.relay_config[i] = None

            for i in range(9):
                tmp = sqlData[i + 16]
                if tmp != None:
                    df.accu_config[i] = int(sqlData[i + 16].strip())
                else:
                    df.accu_config[i] = None
            print(nameDevice)
            print(sttDevice)
            print("Cau hinh accu: ")
            print(df.accu_config)
            return True
        elif len(retData) > 1:
            cl.ColorPrint(">Trung IP gateway tai cac index", cl.bcolors.FAIL)
            for row in retData:
                print(row)
            return False
        elif len(retData) == 0:
            cl.ColorPrint(">Khong tim thay Gateway cua tram tren server", cl.bcolors.FAIL)
            cl.ColorPrint(">Kiem tra lai cau hinh IP cua moderm", cl.bcolors.FAIL)
            return False
    else:
        cl.ColorPrint(">Read config fail: Khong the ket noi den server", cl.bcolors.FAIL)
        return False


def ReadRelayStateFromServer():
    print("Cap nhat trang thai relay tu server")
    retData = None
    if df.id_tram != 0:
        retData = ExecuteQuery(
            "SELECT [name], [status] from [OMC_LOG_V3].[dbo].[thietbi_tram] where id_tram = ('" + df.id_tram.replace(
                " ", "") + "')")
    else:
        retData = ExecuteQuery("SELECT [name], [status] from [OMC_LOG_V3].[dbo].[thietbi_tram] where id_tram = 149 ")
    # print(retData[0][0].replace("device",""))
    df.updateStatusPending = False
    for device in retData:
        # print(type(device[1]))
        deviceNo = int(device[0].replace("device", ""))
        if df.relay_config[deviceNo - 1] == 0:
            if device[1] == 1:
                if Comu.DigitalWrite(deviceNo - 1, 0) != None:
                    print("Dang dieu khien relay" + str(deviceNo) + " ON")
                else:
                    df.updateStatusPending = True
            # else:
            #     Comu.DigitalWrite(deviceNo - 1, 1)
            # Comu.DigitalWrite(int(device[0].replace("device","")) -1, state)

        # retData = ExecuteQuery("SELECT * FROM OMC_LOG_V3.dbo.Config")


def ReadConfigInput():
    print("Read config input")
    retData = None
    if df.id_tram != 0:
        retData = ExecuteQuery(
            "SELECT [stt], [cb_on] from [OMC_LOG_V3].[dbo].[canhbao_tram_current] where id_tram = ('" + df.id_tram.replace(
                " ", "") + "')")
    else:
        retData = ExecuteQuery(
            "SELECT [stt], [cb_on] from [OMC_LOG_V3].[dbo].[canhbao_tram_current] where id_tram = 2213 ")
    if retData != []:
        print(retData)
        for Input in retData:
            if Input[0] < 11:
                df.input_config[int(Input[0]) - 1] = int(Input[1])
            else:
                df.input_config[int(Input[0]) + 5] = int(Input[1])  #
        print(df.input_config)


def UpdateDataToDBTamplogv3():
    insert_query = "exec OMC_LOG_V3.dbo.insert_log_tram_v5 "
    values = (df.id_tram, '',
              df.Temp[0], df.Temp[1], df.Temp[2], '',
              df.VAC[0], df.VAC[1], df.VAC[2], df.VAC[3], df.VAC[4], df.VAC[5],
              df.FAC[0], df.FAC[1], df.FAC[2], df.FAC[3], df.FAC[4], df.FAC[5],
              df.VDC, '', '',
              '', '', '', '', '', '')
    for i in range(len(values)):
        if i == 0:
            insert_query = insert_query + "'" + str(values[i]) + "'"
        else:
            insert_query = insert_query + ",'" + str(values[i]) + "'"
    print(insert_query)
    if df.platform == 'linux':
        sqlDriver = 'FreeTDS'
    elif df.platform == 'win32':
        sqlDriver = 'ODBC Driver 17 for SQL Server'
    conStr = 'DRIVER=' + sqlDriver + ';' \
                                     'SERVER=' + server + ';' \
                                                          'PORT=1433;' \
                                                          'UID=' + username + ';' \
                                                                              'PWD=' + password
    # print (conStr)
    try:
        cnxn = pyodbc.connect(conStr)
        cur = cnxn.cursor()
        cur.execute(insert_query)
        cnxn.commit()
        cur.close()
        cnxn.close()
    except:
        cl.ColorPrint(">Khong the ke noi den server hoac khong tim thay database", cl.bcolors.FAIL)
        cur.close()
        cnxn.close()


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


def UpdateDataToDBdatavalue2():
    insert_query = "exec OMC_LOG_V3.dbo.insert_data_value2 "

    CombineInput()
    CombineInputEvent()
    CompareInputEvent()
    values = (df.id_tram,
              df.IAC[0], df.IAC[1], df.IAC[2],
              df.IDevice[0], df.IDevice[1], df.IDevice[2],
              df.input,
              df.VAccuMayPhat)
    for i in range(len(values)):
        if i == 0:
            insert_query = insert_query + "'" + str(values[i]) + "'"
        else:
            insert_query = insert_query + ",'" + str(values[i]) + "'"
    print(insert_query)
    if df.platform == 'linux':
        sqlDriver = 'FreeTDS'
    elif df.platform == 'win32':
        sqlDriver = 'ODBC Driver 17 for SQL Server'
    conStr = 'DRIVER=' + sqlDriver + ';' \
                                     'SERVER=' + server + ';' \
                                                          'PORT=1433;' \
                                                          'UID=' + username + ';' \
                                                                              'PWD=' + password
    # print (conStr)
    try:
        cnxn = pyodbc.connect(conStr)
        cur = cnxn.cursor()
        cur.execute(insert_query)
        cnxn.commit()
        cur.close()
        cnxn.close()
    except:
        cl.ColorPrint(">Khong the ke noi den server hoac khong tim thay database", cl.bcolors.FAIL)
        cur.close()
        cnxn.close()


def UpdateDeviceStatus(id_tram, device, stt):
    queryStr = "update [OMC_LOG_V3].[dbo].[thietbi_tram] set status =" + str(stt) + " where id_tram = " + str(
        id_tram) + " and name = 'device" + str(device) + "'"
    print(queryStr)
    if df.platform == 'linux':
        sqlDriver = 'FreeTDS'
    elif df.platform == 'win32':
        sqlDriver = 'ODBC Driver 17 for SQL Server'
    conStr = 'DRIVER=' + sqlDriver + ';' \
                                     'SERVER=' + server + ';' \
                                                          'PORT=1433;' \
                                                          'UID=' + username + ';' \
                                                                              'PWD=' + password
    # print (conStr)
    try:
        cnxn = pyodbc.connect(conStr)
        cur = cnxn.cursor()
        cur.execute(queryStr)
        cnxn.commit()
        cur.close()
        cnxn.close()
    except:
        cl.ColorPrint(">Khong the ke noi den server hoac khong tim thay database", cl.bcolors.FAIL)


def UpdateIPtram():
    queryStr = "update [OMC_LOG_V3].[dbo].[ip_tram] set ip1_tb ='" + str(df.myIP) + "' where id_tram = " + str(
        df.id_tram)
    print(queryStr)
    if df.platform == 'linux':
        sqlDriver = 'FreeTDS'
    elif df.platform == 'win32':
        sqlDriver = 'ODBC Driver 17 for SQL Server'
    conStr = 'DRIVER=' + sqlDriver + ';' \
                                     'SERVER=' + server + ';' \
                                                          'PORT=1433;' \
                                                          'UID=' + username + ';' \
                                                                              'PWD=' + password
    # print (conStr)
    try:
        cnxn = pyodbc.connect(conStr)
        cur = cnxn.cursor()
        cur.execute(queryStr)
        cnxn.commit()
        cur.close()
        cnxn.close()
    except:
        cl.ColorPrint(">Khong the ke noi den server hoac khong tim thay database", cl.bcolors.FAIL)


def ExecuteStore(storeProceduce, values):  # dung cho update accu
    insert_query = storeProceduce
    for i in range(len(values)):
        if i == 0:
            insert_query = insert_query + "'" + str(values[i]) + "'"
        elif i == 1:
            insert_query = insert_query + "," + str(values[i]) + ""
        else:
            insert_query = insert_query + ",'" + str(values[i]) + "'"
    print(insert_query)
    if df.platform == 'linux':
        sqlDriver = 'FreeTDS'
    elif df.platform == 'win32':
        sqlDriver = 'ODBC Driver 17 for SQL Server'
    conStr = 'DRIVER=' + sqlDriver + ';' \
                                     'SERVER=' + server + ';' \
                                                          'PORT=1433;' \
                                                          'UID=' + username + ';' \
                                                                              'PWD=' + password
    # print (conStr)
    try:
        cnxn = pyodbc.connect(conStr)
        cur = cnxn.cursor()
        cur.execute(insert_query)
        cnxn.commit()
        cur.close()
        cnxn.close()
    except:
        cl.ColorPrint(">Khong the ke noi den server hoac khong tim thay database", cl.bcolors.FAIL)
        cur.close()
        cnxn.close()


def UpdateDataAccuToDB():
    for i in range(len(df.accu_config)):
        values = (df.id_tram, i + 1)
        if df.accu_config[i] != 0 and df.accu_config[i] != None:
            if i == 0:
                for j in range(24):
                    values = values + (df.VAccu1[j],)
            elif i == 1:
                for j in range(24):
                    values = values + (df.VAccu2[j],)
            elif i == 2:
                for j in range(24):
                    values = values + (df.VAccu3[j],)
            elif i == 3:
                for j in range(24):
                    values = values + (df.VAccu4[j],)
            elif i == 4:
                for j in range(24):
                    values = values + (df.VAccu5[j],)
            elif i == 5:
                for j in range(24):
                    values = values + (df.VAccu6[j],)
            elif i == 6:
                for j in range(24):
                    values = values + (df.VAccu7[j],)
            elif i == 7:
                for j in range(24):
                    values = values + (df.VAccu8[j],)
            elif i == 8:
                for j in range(24):
                    values = values + (df.VAccu9[j],)
            ExecuteStore("exec OMC_LOG_V3.dbo.insert_data_au ", values)


def UpdateCurrentVersionToServer():
    queryStr = "update [OMC_LOG_V3].[dbo].[Config] set currentVersion ='" + str(
        df.current_version) + "' where id_tram = " + str(df.id_tram)
    print(queryStr)
    if df.platform == 'linux':
        sqlDriver = 'FreeTDS'
    elif df.platform == 'win32':
        sqlDriver = 'ODBC Driver 17 for SQL Server'
    conStr = 'DRIVER=' + sqlDriver + ';' \
                                     'SERVER=' + server + ';' \
                                                          'PORT=1433;' \
                                                          'UID=' + username + ';' \
                                                                              'PWD=' + password
    # print (conStr)
    try:
        cnxn = pyodbc.connect(conStr)
        cur = cnxn.cursor()
        cur.execute(queryStr)
        cnxn.commit()
        cur.close()
        cnxn.close()
    except:
        cl.ColorPrint(">Khong the ke noi den server hoac khong tim thay database", cl.bcolors.FAIL)
