from serial.tools import list_ports
from serial import *
import serial
from threading import Thread
from flask import Flask,url_for
from flask import request
import socket
app = Flask(__name__)
@app.route("/<name>")
def index(name):
    print(request.full_path)
    # print(url_for('/~id_thietbi', next='/'))
    return "OK"
print(socket.gethostbyname(socket.getfqdn()))
hostIP= [l for l in ([ip for ip in socket.gethostbyname_ex(socket.gethostname())[2]
if not ip.startswith("127.")][:1], [[(s.connect(('8.8.8.8', 53)),
s.getsockname()[0], s.close()) for s in [socket.socket(socket.AF_INET,
socket.SOCK_DGRAM)]][0][1]]) if l][0][0]
print (hostIP)
class FlaskThread(Thread):
    def run(self):
        app.run(debug=True, host=hostIP, port=6969, use_debugger=True,use_reloader=False)


if __name__ == '__main__':
    server= FlaskThread()
    server.daemon=True
    server.start()
    print("Run")
    while True:
        # print("OK")
        time.sleep(1)
        print(time.time())