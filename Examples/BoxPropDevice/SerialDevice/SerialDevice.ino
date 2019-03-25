#include <ArduinoJson.hpp>
#include <ArduinoJson.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BNO055.h>
#include <Wire.h>
const int capacity = JSON_OBJECT_SIZE(10);
StaticJsonBuffer<capacity> jb;

const int pingPin = 12;
const int lightPin = A1;

Adafruit_BNO055 bno = Adafruit_BNO055(55);

void setup() {
	// put your setup code here, to run once:
	Serial.begin(9600);
	while (!Serial) {
		; //wait for serial port to connect!
	}
	if (!bno.begin()) {
		//an error occured where
	}
	delay(1000);
	bno.setExtCrystalUse(true);
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

void loop() {
	delay(100);
	char sensorDataOutput[4084];
	sensors_event_t event;
	bno.getEvent(&event);
	
	long lightIntensity = analogRead(lightPin);

	JsonObject& sensorsData = jb.createObject();
	sensorsData["LightSensor"] = buildLightSensorData(lightIntensity);
	sensorsData["OrientationSensor"] = buildOrientationSensorData(event.orientation.x, event.orientation.y, event.orientation.z);
	sensorsData.printTo(sensorDataOutput);
	jb.clear();

	Serial.println(sensorDataOutput);
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
