#include <ArduinoJson.hpp>
#include <ArduinoJson.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BNO055.h>
#include <SPI.h>
#include <WiFi101.h>
const int capacity = JSON_OBJECT_SIZE(200);
StaticJsonBuffer<capacity> jb;



const int SERVER_PORT = 53005;

const String DONE_STATUS = "Done";
const String ERROR_NO_SHEILD_STATUS = "NoSheild";
const String ERROR_WRONG_CREDENTIALS = "WrongCredentials";

int statLed = LED_BUILTIN;
int status = WL_IDLE_STATUS;

const int pingPin = 12;
const int lightPin = A1;
const int flexPin = A2;
Adafruit_BNO055 bno = Adafruit_BNO055(55);

String ssid = "";          //  your network SSID (name) 
String pass = "";
WiFiServer server(SERVER_PORT);
IPAddress deviceIp(192, 168, 1, 224);

JsonObject& buildConfig(String status) {
	JsonObject& configData = jb.createObject();
	configData["Status"] = status;
	configData["WifiCredentials"] = jb.createObject();
	configData["WifiCredentials"]["SSID"] = "";
	configData["WifiCredentials"]["Password"] = "";
	configData["DeviceIp"] = "";
	return configData;
}

void setup() {
	
	//Serial.println("configured sensor");
	Serial.begin(9600);
	while (!Serial) {
		//wait for serial port to connect
	}
	while (true) {
		if (Serial.available() > 0) {
			String configJson = Serial.readStringUntil('\n');
			JsonObject& config = jb.parseObject(configJson);
			if (config.success()) {
				ssid = config["WifiCredentials"]["SSID"].as<String>();
				pass = config["WifiCredentials"]["Password"].as<String>();
				break;
			}
		}
	}
	Serial.println("test");
	WiFi.config(deviceIp);
	if (WiFi.status() == WL_NO_SHIELD) {
		sendConfigData(buildConfig(ERROR_NO_SHEILD_STATUS));
		// now in failed state you will have to restart arduino.
		while (true);
	}
	

	status = WiFi.begin(ssid, pass);
	if (status != WL_CONNECTED) {
		sendConfigData(buildConfig(ERROR_WRONG_CREDENTIALS));
		while (true);
	}
	Serial.println("begun...");
	// now wait for the client to connect...
	pinMode(flexPin, INPUT);
	Serial.println("set pin success");
	bno.begin();
	bno.setExtCrystalUse(true);
	//Give enough time for amessage to be sent
	Serial.println("going to wait...");
	delay(10000);
	Serial.println("going done waiting...");
	JsonObject& done = buildConfig(DONE_STATUS);
	done["DeviceIp"] = "192.168.1.224";
	sendConfigData(done);
	server.begin();
	jb.clear();
	
}

JsonObject& buildLightSensorData(long value) {
	JsonObject& data = jb.createObject();
	data["Intensity"] = value;
	return data;
}
JsonObject& buildOrientationSensorData(float x, float y, float z) {
	JsonObject& data = jb.createObject();
	data["X"] = x;
	data["Y"] = y;
	data["Z"] = z;
	return data;
}
JsonObject& buildGravitySensorData(Adafruit_BNO055 bno) {
	JsonObject& data = jb.createObject();
	imu::Vector<3> gravity = bno.getVector(Adafruit_BNO055::VECTOR_GRAVITY);
	data["X"] = gravity.x();
	data["Y"] = gravity.y();
	data["Z"] = gravity.z();
	return data;
}
JsonObject& buildFlexSensorData(float flexResistence) {
	JsonObject& data = jb.createObject();
	data["FlexResistence"] = flexResistence;
	return data;
}


void loop() {
	// We only want to accept the technical prop client
	// since we want to send sensor data to it.
	WiFiClient client = server.available();
	if (client) {
		Serial.println("got client");
		while (client.connected()) {
			
			delay(100);
			char sensorDataOutput[4084];
			sensors_event_t event;
			bno.getEvent(&event);

			long lightIntensity = smoothAnalogData(lightPin);
			long flexADC = smoothAnalogData(flexPin);
			JsonObject& sensorsData = jb.createObject();
			sensorsData["LightSensor"] = buildLightSensorData(lightIntensity);
			sensorsData["OrientationSensor"] = buildOrientationSensorData(event.orientation.x, event.orientation.y, event.orientation.z);
			sensorsData["GravitySensor"] = buildGravitySensorData(bno);
			sensorsData["FlexSensor"] = buildFlexSensorData(flexADC);
			sensorsData.printTo(sensorDataOutput);
			client.write(sensorDataOutput);
			jb.clear();
			
		}
	}
}

long smoothAnalogData(int pin) {
	long value = 0;
	for (int i = 0; i < 100; i++) {
		value += analogRead(pin);
	}
	return round(value / 100);
}

void sendConfigData(JsonObject &object) {
	char output[1064];
	object.printTo(output, sizeof(output));
	Serial.println(output);
}