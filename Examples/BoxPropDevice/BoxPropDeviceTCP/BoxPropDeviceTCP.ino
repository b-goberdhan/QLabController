#include <ArduinoJson.hpp>
#include <ArduinoJson.h>
#include <SPI.h>
#include <WiFi101.h>
const int capacity = JSON_OBJECT_SIZE(10);
StaticJsonBuffer<capacity> jb;



const int SERVER_PORT = 53005;

const int ERROR_NO_SHIELD_CODE = -1;
const int ERROR_AP_CODE = -2;
const int SUCCESS_CODE_HAS_SHIELD = 0;
const int SUCCESS_STARTED_SERVER = 1;
const int SUCCESS_CLIENT_CONNECTED = 2;

int statLed = LED_BUILTIN;
int status = WL_IDLE_STATUS;
char apSsid[] = "Prop";          //  your network SSID (name) 
WiFiServer server(SERVER_PORT);

void setup() {
  Serial.begin(9600);
  while(!Serial) {
    //wait for serial port to connect
  }
  
  if (WiFi.status() == WL_NO_SHIELD) {
    sendConfigData(ERROR_NO_SHIELD_CODE);
    // now in failed state you will have to restart arduino.
    while (true); 
  }
  status = WiFi.beginAP(apSsid);
  if (status != WL_AP_LISTENING) {
    sendConfigData(SUCCESS_CODE_HAS_SHIELD);
    while (true);
  }
  delay(10000);
  server.begin();

  sendConfigData(SUCCESS_CODE_HAS_SHIELD);
  delay(1000); //give some time 
  sendConfigData(WiFi.localIP());
  // now wait for the client to connect...
 
}

void loop() {
  // We only want to accept the technical prop client
  // since we want to send sensor data to it.
  if (status != WiFi.status()) {
    status = WiFi.status();
    if (status == WL_AP_CONNECTED) {
      sendConfigData(SUCCESS_CLIENT_CONNECTED);
    }
  }
  WiFiClient client = server.available();
  if (client) {
    while (client.connected()){
      //sendConfigData(SUCCESS_CLIENT_CONNECTED);
      if (true){
        delay(1000);
        char output[1064];
        sendConfigData(SUCCESS_CLIENT_CONNECTED);
        buildSensorData(output, sizeof(output), "TestSensor", random(100));
        client.println(output);
      }
    }
  }
}

void buildSensorData(char *dest, int destSize, char sensorName[], long value) {
  JsonObject& data = jb.createObject();
  data["Name"] = sensorName;
  data["SensorValue"] = value;
  data.printTo(dest, destSize);
  jb.clear();
}
void buildConfigData(char *dest, int destSize, long data) {
  JsonObject& dataObj = jb.createObject();
  dataObj["Data"] = data;
  dataObj.printTo(dest, destSize);
  jb.clear();
}

void sendConfigData(int data) {
  char output[1064];
  buildConfigData(output, sizeof(output), data);
  Serial.println(output);
}
