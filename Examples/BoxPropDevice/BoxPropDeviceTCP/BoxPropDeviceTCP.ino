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
int statLed = LED_BUILTIN;
int status = WL_IDLE_STATUS;
char apSsid[] = "Prop";          //  your network SSID (name) 

// Initialize the client library
WiFiClient technicalPropClient;
WifiServer server(SERVER_PORT);
void setup() {
  Serial.begin(9600);
  while(!Serial) {
    //wait for serial port to connect
  }
  
  if (Wifi.status() == WL_NO_SHIELD) {
    Serial.println(SUCCESS_CODE_HAS_SHIELD);
    // now in failed state you will have to restart arduino.
    while (true); 
  }
  status = Wifi.beginAP(apSsid);
  if (status != WL_AP_LISTENING) {
    
  }
  delay(10000);
  server.begin();
  Serial.println(SUCCESS_CODE_HAS_SHIELD)
  // now wait for the client to connect...
  while (true) {
    if (status != Wifi.status()) {
    status = Wifi.status();
    if (status == WL_AP_CONNECTED) {
      // We only want to accept the technical prop client
      // since we want to send sensor data to it.
      technicalPropClient = server.available();
      break;
    }
  }
  }
}

void loop() {
  if (technicalPropClient.connected()) {
    // for now we focus on only sending data to the client
    technicalPropClient.println("helloWorld");
  }
}

void buildSensorData(char *dest, int destSize, char sensorName[], long value) {
  JsonObject& data = jb.createObject();
  data["Name"] = sensorName;
  data["SensorValue"] = value;
  data.printTo(dest, destSize);
  jb.clear();
}
