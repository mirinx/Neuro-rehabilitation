import socket
import time
import numpy as np
import tensorflow as tf

UDP_IP = "127.0.0.1" 
UDP_PORT = 5005       
MODEL_PATH = 'best_model_robust.h5' 

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
print(f"Connecting to Unity at {UDP_IP}:{UDP_PORT}...")

print(f"Loading Model from '{MODEL_PATH}'...")
try:
    model = tf.keras.models.load_model(MODEL_PATH)
    print("Model loaded successfully! Ready to control Avatar.")
except OSError:
    print(f"Error: Could not find file '{MODEL_PATH}'. Check the file name!")
    exit()

def get_mock_eeg_data():
    data = np.random.randn(1, 22, 751, 1)
    return data

action_names = ["Left Hand", "Right Hand", "Feet", "Tongue"]

try:
    while True:
        eeg_data = get_mock_eeg_data()
        
        prediction = model.predict(eeg_data, verbose=0)
        

        action_index = np.argmax(prediction) 
        confidence = np.max(prediction)     

        if confidence > 0.5:
            message = f"{action_index},{confidence:.2f}"
            msg = message.encode('utf-8')
            sock.sendto(msg, (UDP_IP, UDP_PORT))
            
            print(f"🚀 Action: {action_names[action_index]} \t(Conf: {confidence*100:.1f}%) -> Sent to Unity")
        else:
        
            msg = "-1,0.0".encode('utf-8')
            sock.sendto(msg, (UDP_IP, UDP_PORT))
            print(f"Waiting... (Noise) \t(Conf: {confidence*100:.1f}%)")

        time.sleep(0.1) 

except KeyboardInterrupt:
    print("\nProgram Stopped by User.")
    sock.close()