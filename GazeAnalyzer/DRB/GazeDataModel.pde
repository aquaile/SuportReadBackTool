/*
 視線検出器から得られた視線データを保存するモデルクラス
 */
class GazeDataModel {
  private ArrayList<Gaze> data; //受信した視線データを格納する変数

  //コンストラクタ
  GazeDataModel() {
    data = new ArrayList<Gaze>();
  }

  //視線データを受信したら値を変数に追加する関数
  void update(float px, float py, String ts) {
    data.add(new Gaze(px, py, ts));
  }

  //データの要請があった場合に返す関数（取り出す値の個数、開始点）
  ArrayList<Gaze> get_data(int num, int pivot) {
    ArrayList<Gaze> gaze = new ArrayList<Gaze>(); //返送するデータを格納
    //求める個数に満たなかった場合はnullを返す
    if (data.size()<pivot+num) {
      //num = data.size() - pivot; //デバッグ用：取り出す個数をデータ全てにする
      return null;
    }
    for (int i=num; i<pivot+num; i++) {
      float px = data.get(i).posX; //i番目のx座標
      float py = data.get(i).posY; //i番目のy座標
      float ts = data.get(i).timestamp; //i番目のタイムスタンプ
      gaze.add(new Gaze(px, py, ts));
    }
    return gaze;
  }
}