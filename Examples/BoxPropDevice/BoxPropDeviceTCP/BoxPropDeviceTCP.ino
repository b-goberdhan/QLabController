#include <ArduinoJson.hpp>
#include <ArduinoJson.h>
#include <SPI.h>
#include <WiFi101.h>
const int capacity = JSON_OBJECT_SIZE(10);
StaticJsonBuffer<capacity> jb;



const int SERVER_PORT = 53005;

const int ERROR_NO_SHIELD_CODE = -1;
const int ERROR_AP_CODE = -2;
const int ERROR_NOT_CONNECTED_CODE = -3;
const int SUCCESS_CODE_HAS_SHIELD = 0;
const int SUCCESS_STARTED_SERVER = 1;
const int SUCCESS_CLIENT_CONNECTED = 2;

int statLed = LED_BUILTIN;
int status = WL_IDLE_STATUS;
String ssid = "fablabx";          //  your network SSID (name) 
String pwd = "fabulabbing";
WiFiServer server(SERVER_PORT);

//Sensor info;
const int lightPin = A1;

void setup() {
  Serial.begin(9600);
  while(!Serial) {
    //wait for serial port to connect
  }
  while (Serial.available()) {
    
  }
  ssid = Serial.readStringUntil('\n');
  while (Serial.available()) {
    
  }
  pwd = Serial.readStringUntil('\n');
  
  if (WiFi.status() == WL_NO_SHIELD) {
    sendConfigData(ERROR_NO_SHIELD_CODE);
    // now in failed state you will have to restart arduino.
    while (true); 
  }
  status = WiFi.begin(ssid, pwd);
  if (status != WL_CONNECTED) {
    sendConfigData(ERROR_NOT_CONNECTED_CODE);
    while (true);
  }
  sendConfigData(SUCCESS_CODE_HAS_SHIELD);
  //Give enough time for amessage to be sent
  delay(10000);
  
  server.begin();

 
  delay(1000); //give some time 
  sendConfigData(WiFi.localIP());
  // now wait for the client to connect...
 
}

void loop() {
  // We only want to accept the technical prop client
  // since we want to send sensor data to it.
  WiFiClient client = server.available();
  if (client) {
    while (client.connected()){
      if (true){
        delay(100);
        char output[1064];
        long lightIntensity = analogRead(lightPin);
        buildSensorData(output, sizeof(output), "LightSensor", lightIntensity);
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

String recvMessage() {
  while (Serial.available()) {
    
  }
  return Serial.readStringUntil('\n');
}
