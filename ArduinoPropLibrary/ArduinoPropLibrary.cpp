/*
 Name:		ArduinoPropLibrary.cpp
 Created:	2/12/2019 11:29:40 AM
 Author:	Brandon
 Editor:	http://www.visualmicro.com
*/

#include "ArduinoPropLibrary.h"
#include "ArduinoJson\ArduinoJson.h"

const int capacity = JSON_OBJECT_SIZE(10);
StaticJsonBuffer<capacity> jb;

Prop::Prop(HardwareSerial &serial) 
{
	
}
bool Prop::send(JsonObject &object) 
{
	return true;
}
JsonObject Prop::createSensorObject(char sensorName[], int value)
{
	 JsonObject &sensorData = jb.createObject();

}
