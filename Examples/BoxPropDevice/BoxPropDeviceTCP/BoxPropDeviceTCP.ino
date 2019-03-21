#include <ArduinoJson.hpp>
#include <ArduinoJson.h>
#include <SPI.h>
#include <WiFi101.h>
const int capacity = JSON_OBJECT_SIZE(10);
StaticJsonBuffer<capacity> jb;

const int pingPin = 12;
const int lightPin = A0;


char ssid[] = "EngSystems";          //  your network SSID (name) 
char pass[] = "engsystems3441";   // your network password

int status = WL_IDLE_STATUS;
IPAddress server(74,125,115,105);  // Google

// Initialize the client library
WiFiClient client;

void setup() {
  Serial.begin(9600);
  while (!Serial) {
    ; //wait for serial port to connect!
  }
  Serial.println("Attempting to connect to WPA network...");
  Serial.print("SSID: ");
  Serial.println(ssid);

  status = WiFi.begin(ssid, pass);
  if ( status != WL_CONNECTED) { 
    Serial.println("Couldn't get a wifi connection");
    // don't do anything else:
    while(true);
  } 
  else {
    Serial.println("Connected to wifi");
    Serial.println("\nStarting connection...");
    // if you get a connection, report back via serial:
    if (client.connect(server, 80)) {
      Serial.println("connected");
      // Make a HTTP request:
      client.println("GET /search?q=arduino HTTP/1.0");
      client.println();
    }
  }
}

void loop() {
  //char output[1064];
  //delay(100);
  //long distanceCm = getPingRangeCm();
  //long lightIntensity = analogRead(lightPin);
  //buildSensorData(output, sizeof(output), "Light", lightIntensity);
  //Serial.println(output);
}

void buildSensorData(char *dest, int destSize, char sensorName[], long value) {
  JsonObject& data = jb.createObject();
  data["Name"] = sensorName;
  data["SensorValue"] = value;
  data.printTo(dest, destSize);
  jb.clear();
}

long getPingRangeCm() {
  pinMode(pingPin, OUTPUT);
  digitalWrite(pingPin, LOW);
  delayMicroseconds(2);
  digitalWrite(pingPin, HIGH);
  delayMicroseconds(5);
  digitalWrite(pingPin, LOW);

  pinMode(pingPin, INPUT);
  long duration = pulseIn(pingPin, HIGH);
  return duration / 29 / 2;
}
