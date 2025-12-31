import socket
import time

UDP_IP = "127.0.0.1"
UDP_PORT = 5005

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

print("Starting Body Check Mode")
print("sending commands to Unity every 2 seconds")
print("-" * 30)

actions = [
    (0, "Left Hand"),
    (1, "Right Hand"),
    (2, "Feet"),
    (3, "Tongue"),
    (-1, "Relax")
]

try:
    while True:
        for action_index, action_name in actions:
            
            confidence = 1.0
            
            if action_index == -1:
                confidence = 0.0

            msg = f"{action_index},{confidence}".encode('utf-8')
            sock.sendto(msg, (UDP_IP, UDP_PORT))
            
            print(f"Sending: {action_name}")
            
            time.sleep(2)

except KeyboardInterrupt:
    print("\nTest Stopped")
    sock.close()