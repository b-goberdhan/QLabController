#include <ArduinoJson.h>

const int capacity = JSON_OBJECT_SIZE(10);
StaticJsonBuffer<capacity> jb;

const int pingPin = 12;

void setup() {
	// put your setup code here, to run once:
	Serial.begin(9600);
	while (!Serial) {
		; //wait for serial port to connect!
	}
}

void loop() {

	char output[1064];
	delay(100);
	long distanceCm = getPingRangeCm();
	buildSensorData(output, sizeof(output), "Ping", distanceCm);
	Serial.println(output);
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

