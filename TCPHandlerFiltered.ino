#include <ESP8266WiFi.h>
#include <Wire.h>
#include <MPU6050_tockn.h>

// Wifi Settings
const char* ssid = "OnePlus";
const char* password = "228337gs";

WiFiServer wifiServer(5045);

// Gyroscope + Accelerometer settings
MPU6050 mpu6050(Wire);

float angleX = 0;
float angleY = 0;
float angleZ = 0;
float dt = 0.01; // Задержка для комплементарного фильтра (10 мс)
float alpha = 0.98; // Коэффициент комплементарного фильтра

unsigned long previousTime = 0;

void setup() {
  //Init
  Serial.begin(9600);
  Wire.begin();
  mpu6050.begin();
  WiFi.begin(ssid, password);

  mpu6050.calcGyroOffsets(true); // Калибровка гироскопа

  delay(1000);
 
  while (WiFi.status() != WL_CONNECTED) { // Connect to wifi
    delay(1000);
    Serial.println("Connecting..");
  }
  Serial.print("Connected to WiFi. IP:");
  Serial.println(WiFi.localIP());
  wifiServer.begin();

}

void loop() 
{
  WiFiClient client = wifiServer.available();
  if (client) {
    while (client.connected()) {
      SendData(client);
      delay(50);
    }
  }
  client.stop();
}

void SendData(WiFiClient client)
{

    unsigned long currentTime = millis();
    dt = (currentTime - previousTime) / 1000.0;
    previousTime = currentTime;

    mpu6050.update();

    // Углы наклона по данным акселерометра
    float accelAngleX = atan(mpu6050.getAccY() / sqrt(pow(mpu6050.getAccX(), 2) + pow(mpu6050.getAccZ(), 2))) * 180/PI;
    float accelAngleY = atan(-1 * mpu6050.getAccX() / sqrt(pow(mpu6050.getAccY(), 2) + pow(mpu6050.getAccZ(), 2))) * 180/PI;

    // Угловая скорость по данным гироскопа
    float gyroRateX = mpu6050.getGyroX();
    float gyroRateY = mpu6050.getGyroY();

    // Комплементарный фильтр
    angleX = alpha * (angleX + gyroRateX * dt) + (1 - alpha) * accelAngleX;
    angleY = alpha * (angleY + gyroRateY * dt) + (1 - alpha) * accelAngleY;

    String data = String(angleX) + ",0," + String(angleY)+",";
    Serial.print(data);
    Serial.println("\t\t");
    client.print(data);

}