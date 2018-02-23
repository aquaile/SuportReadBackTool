/*
 視線データクラス
 */
class Gaze {
  float posX;
  float posY;
  float timestamp;

  //コンストラクタ（ x座標、y座標, タイムスタンプ<文字列> ）
  Gaze(float px, float py, String ts) {
    posX = px;
    posY = py;
    timestamp = str2sec(ts);
  }

  //コンストラクタ（ x座標、y座標, タイムスタンプ<浮動小数> ）
  Gaze(float px, float py, float ts) {
    posX = px;
    posY = py;
    timestamp = ts;
  }

  //String型のタイムスタンプをFloat型に変換する
  float str2sec(String str) {
    String ts = str.substring(4); //日付データを取り除いたタイムスタンプを取得
    float h = float(ts.substring(0, 2))*60*60; //時間
    float m = float(ts.substring(2, 4))*60; //分
    float s = float(ts.substring(4, 6)); //秒
    float ms = float(ts.substring(6, 8))/100.0; //ミリ秒を有効数字3桁表現
    return h + m + s + ms;
  }
}