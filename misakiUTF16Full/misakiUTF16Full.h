// 
// 美咲フォントドライバ v1.0 by たま吉さん 2016/08/05
// EEPROMバージョン
//

#ifndef misakiUTF16Full_h
#define misakiUTF16Full_h

#include <arduino.h>

#define FTABLESIZE     7120      // フォントテーブルデータサイズ
#define FONT_LEN       8         // 1フォントのバイト数

void font_init();
int findcode(uint16_t  ucode);
boolean getFontDataByUTF16(byte* fontdata, uint16_t utf16);    // UTF16に対応する美咲フォントデータ8バイトを取得
uint16_t hkana2kana(uint16_t ucode);                           // 半角カナを全角に変換
uint16_t utf16_HantoZen(uint16_t utf16);                       // UTF16半角コードをUTF16全角コードに変換
byte charUFT8toUTF16(uint16_t *pUTF16, char *pUTF8);           // UTF8文字(1～3バイ)をUTF16に変換
byte Utf8ToUtf16(uint16_t* pUTF16, char *pUTF8);               // UTF8文字列をUTF16文字列に変換
char* getFontData(byte* fontdata,char *pUTF8,bool h2z=false);  // 指定したUTF8文字列の先頭のフォントデータの取得

#endif
