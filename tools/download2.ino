// download2.ino
// 作成日 2014/01/15 by たま吉さん
// I2C接続 シリアルEEPROM AT24C1024Bデータ書込みスケッチ var 2.0
// [履歴]
// 2014/01/22 修正：クロックモジュール上のEEPROMとアドレス干渉するため、A2をHIGHに変更
// 2014/01/25 修正：プロトタイプシールド化
// 2014/03/01 修正：ダンプリスト、I2Cテストのコマンド要求対応(これにより基板のスイッチ操作を廃止)
// 2014/11/27 修正：EEPROMのアドレスはdefineで設定するように変更
// 2014/11/27 修正：LCDの代わりにSoftwareSerialを利用に変更
// 2014/11/29 修正：動作安定のため、モニタ機能を削除(download.ino => dounload2.inoに版分け） 

#include <stdio.h>
#include <Wire.h>

#define DEVICE_ADDRESS 0x50    // AT24C1024B I2Cデバイスアドレス 
#define BAUDRATE 115200         // シリアル通信速度

//
// EEPROM 1バイト書込
// address: アドレス（19ビット 0x00000 - 0x1FFFF)
// 　　　　 [A2][A1][P0] + 16ビットメモリ空間
// data   : 書込データ
//
int write(unsigned long address, byte data)  {
  uint8_t   devaddr = (uint8_t)DEVICE_ADDRESS | (uint8_t)(address>>16);  // DEVICE_ADDRESS + [A2][A1][P0]
  uint16_t  addr    = address & 0xFFFF;
  int rc;

  Wire.beginTransmission(devaddr);
  Wire.write((byte)(addr >> 8));   // アドレス上位
  Wire.write((byte)(addr & 0xFF)); // アドレス下位
  Wire.write(data);
  rc = Wire.endTransmission();
  delay(6);
  return rc;
}

//
// EEPROM 1バイト読込
// address: アドレス（19ビット 0x00000 - 0x7FFFF)
// 　　　　 [A2][A1][P0] + 16ビットメモリ空間
//
byte read(unsigned long address)  {
  uint8_t   devaddr = (uint8_t)DEVICE_ADDRESS | (uint8_t)(address>>16);  // DEVICE_ADDRESS + [A2][A1][P0]
  uint16_t  addr    = address & 0xFFFF;
  byte data = 0xFF;
  int rc;

  Wire.beginTransmission(devaddr);
  Wire.write((byte)(addr >> 8));   // アドレス上位
  Wire.write((byte)(addr & 0xFF)); // アドレス下位
  rc = Wire.endTransmission();
  rc = Wire.requestFrom(devaddr,(uint8_t)1);
  data = Wire.read();
  return data;
}

//
// ホストPC上の専用アプリとシリアル接続してI2C接続EEPROMにデータを書き込む
//
int download2() {
   unsigned long cnt = 0;
   byte rcv[20];
   unsigned long  sadr=0;    // データ書き込み開始アドレス
   unsigned long  dsize=0;   // データサイズ
   unsigned short cksum =0;  // チェックサム
   char str[20];
   
   byte data;
   int rc=0;
   int n = 0;
   int rty =0;
 
   Serial.flush();
   int dm_cnt = 0; 
   int a_cnt = 0;
 
 while(1) {    
   // コマンド受信待ち
   while(Serial.available() <1);
   rcv[0] = Serial.read();
   
  switch (rcv[0]) {
  case  'h':  // Hello        
         Serial.write('a');
         break;

  case  'd':  // データ設定モード       
        // 書き込み開始アドレス(3バイト)、データサイズ受信
         while(Serial.available() < 6);
           // 6バイト長受信
         for (int i=0; i < 6;i++) {
           rcv[i] = Serial.read();
         }
         sadr=0;
         dsize=0;
         
         // 書き込み開始アドレス(3バイト)、データサイズ受信を変数に格納
         for (int i=0; i < 3; i++) {         
           sadr<<=8;
           dsize<<=8;
           sadr  += rcv[i];
           dsize += rcv[i+3];
         }

         Serial.write('a');
          
         // データの受信及び、EEPROMへの書き込み   
         while(1) {
           if (Serial.available()) {
               data = Serial.read();
               rty=0;
               for(int rty = 0; rty < 5;rty++) {
                 rc = write(sadr+cnt,data);
                 if (!rc) {
                   break;
                 }
               }
               
               if (rc != 0) {
                  Serial.write('x');
                  break;
               }
               
               cnt++;
               n++; 
               cksum += data;
               // データ書き込み完了チェック
               if (cnt >= dsize) {
                  Serial.write('o');                 
                  break;
               }
               // 応答送信
               Serial.write('.');               
               if (n == 1000) {                 
                  n = 0;
               }           
            }
          }
          break;         
    case  's':  // 直前のチェックサムを返す
          Serial.write(cksum & 0xff); 
          Serial.write(cksum>>8); 
          
    default: 
          return rc;
          break;
   }
 }
 return rc;
}

void setup () {
  int rc;  
   Serial.begin(BAUDRATE);        // ホストＰＣとの通信開始
   Wire.begin();                   // I2C開始

   download2();                    // 書込み処理実行
   while(1);
}

void loop() {
}
