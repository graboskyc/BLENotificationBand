#include <Adafruit_NeoPixel.h>
#include <avr/power.h>
#include <avr/sleep.h>
#define PIN        2
#define NUMPIXELS  8

Adafruit_NeoPixel pixels = Adafruit_NeoPixel(NUMPIXELS, PIN, NEO_GRB + NEO_KHZ800);

 String getValue(String data, char separator, int index)
{
  int found = 0;
  int strIndex[] = {0, -1};
  int maxIndex = data.length()-1;

  for(int i=0; i<=maxIndex && found<=index; i++){
    if(data.charAt(i)==separator || i==maxIndex){
        found++;
        strIndex[0] = strIndex[1]+1;
        strIndex[1] = (i == maxIndex) ? i+1 : i;
    }
  }

  return found>index ? data.substring(strIndex[0], strIndex[1]) : "";
}

void allOff() {
  for(int i=0;i<NUMPIXELS;i++) {
    pixels.setPixelColor(i,0);
  }
  pixels.show();
}

void allOn(String s) {
  int r = getValue(s, ',', 0).toInt();
  int g = getValue(s, ',', 1).toInt();
  int b = getValue(s, ',', 2).toInt();
  int d = getValue(s, ',', 3).toInt();

  Serial.print(s);
  
  for(int i=0;i<NUMPIXELS;i++) {
    pixels.setPixelColor(i,pixels.Color(r, g, b));
    pixels.show();
    delay(d);
  }
  
}

void setup() {
  // put your setup code here, to run once:
  pixels.begin();
  //Serial.begin(9600);
  Serial.begin(115200);
  allOff();
}

void loop() {
  // put your main code here, to run repeatedly:
  allOff();
  String readstring;
  while (Serial.available()) {
    //char c = Serial.read();
    readstring = Serial.readString();
  }
  readstring.trim();
  if(readstring.length() > 0) {
    allOn(readstring);
    //Serial.write(readstring);
    delay(1000);
    allOff();
    readstring = "";
  }
  
}
