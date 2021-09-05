Serialconnection = False
ser = None
VAC = [0, 0, 0, 0, 0, 0]
IAC = [0, 0, 0]
FAC = [0, 0, 0, 0, 0, 0]
VAccuMayPhat = 0
Temp = [0, 0, 0]
VAccu1 = [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]
VAccu2 = [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]
VAccu3 = [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]
VAccu4 = [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]
VAccu5 = [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]
VAccu6 = [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]
VAccu7 = [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]
VAccu8 = [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]
VAccu9 = [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]
IDevice = [0, 0, 0]
VDC = 0
input = -1
inputEvent = -1
inputByte = [-1, -1, -1]
inputByteBin = [0] * 24
inputEventByte = [-1, -1, -1]
inputEventByteBin = [None] * 24
id_tram = 0
UpdateDatabaseCycle = 15
is_comu_busy = False
timewaiting = 0
pointWait = 0
updateStatusPending = False
ListColumeData = ['id_tram',
                  'thoigian',
                  'time_insert',
                  'nhietdo1',
                  'nhietdo2',
                  'nhietdo3',
                  'apxc1',
                  'apxc2',
                  'apxc3',
                  'apxc4',
                  'apxc5',
                  'apxc6',
                  'tansoxc1',
                  'tansoxc2',
                  'tansoxc3',
                  'tansoxc4',
                  'tansoxc5',
                  'tansoxc6',
                  'ap1c1']

ListColumeData2 = ['date_time',
                   'id_tram',
                   'I1',
                   'I2',
                   'I3',
                   'ITB1',
                   'ITB2',
                   'ITB3',
                   'INPUT',
                   'VolAccuMayno']

StationName = ''
numberOfAccuModule = 0
relay_config = [None, None, None, None, None, None, None, None, None, None]

RelaySet = [None, None, None, None, None, None, None, None, None, None]
RelayState = [None, None, None, None, None, None, None, None, None, None]
RelayControlFlag = [None, None, None, None, None, None, None, None, None, None]

accu_config = [None, None, None, None, None, None, None, None, None, None]
input_config = [None] * 24
myIP = None
GatewayIP = None
WebserverPort = 6969

platform = None

current_version = '4.0.8'

# mqtt
topic_data_IO_AC = "topic_data_IO_AC"
topic_data_accu = ["topic_data_accu1", "topic_data_accu2", "topic_data_accu3", "topic_data_accu4", "topic_data_accu5",
                   "topic_data_accu6", "topic_data_accu7", "topic_data_accu8", "topic_data_accu9", "topic_data_accu10"]
topic_data_control = "topic_data_control"
topic_receive_config = "topic_receive_config"
topic_request_config = "topic_request_config"
topic_update_current_version = "topic_update_current_version"
topic_update_ip_tram = "topic_update_ip_tram"

topic_update_relay_state = "topic_update_relay_state"
topic_update_relay_state_status = "topic_update_relay_state_status"

topic_request_relay_state = "topic_request_relay_state"
topic_receive_relay_state = "topic_receive_relay_state"

username = '10.39.246.105'
password = 'Hungyen@123'
# end mqtt
