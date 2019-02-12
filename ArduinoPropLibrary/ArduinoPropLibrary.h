/*
 Name:		ArduinoPropLibrary.h
 Created:	2/12/2019 11:29:40 AM
 Author:	Brandon
 Editor:	http://www.visualmicro.com
*/

#ifndef _ArduinoPropLibrary_h
#define _ArduinoPropLibrary_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
	#include "ArduinoJson\ArduinoJson.h"
#else
	#include "WProgram.h"
#endif


class Prop 
{
public:
	Prop(HardwareSerial &serial);
	bool send(JsonObject &object);
	JsonObject createSensorObject(char sensorName[], int value);

};

#endif