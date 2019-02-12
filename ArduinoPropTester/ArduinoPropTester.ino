#include <ArduinoJson.h>

const int capacity = JSON_OBJECT_SIZE(3);
StaticJsonBuffer<capacity> jb;


void setup() {
	// put your setup code here, to run once:
	Serial.begin(9600);
	while (!Serial) {
		; //wait for serial port to connect!
	}
}

void loop() {
	// put your main code here, to run repeatedly:
	JsonObject& obj = jb.createObject();
	obj["hello"] = "world";
	obj["hi"] = 1;
	char output[124];
	delay(1000);
	//obj.prettyPrintTo(Serial);
	obj.printTo(output, sizeof(output));
	Serial.println(output);
	jb.clear();

}