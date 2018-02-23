/*
 生の視線データを加工し、扱いやすいデータへの変換と
 データから視線の属性を抽出するクラス
 */
class GazeCalcurator {
  ArrayList<Gaze> proc_data; //加工したデータを格納する変数
  ArrayList<DataAttributeModel> attr_data; //データ属性を格納する変数
  float TEXT_SIZE = 10.5 * 0.75; //文字サイズ
  float FIX_DIST = 2*TEXT_SIZE; //固視微動の判定領域

  //コンストラクタ
  GazeCalcurator() {
  }

  //処理済みデータを取得する関数
  ArrayList<Gaze> get_proc_data(ArrayList<Gaze> raw_data) {
    fixation(raw_data);
    return proc_data;
  }

  //データ属性を取得する関数
  ArrayList<DataAttributeModel> get_attr_data() {
    attr_data = new ArrayList<DataAttributeModel>();
    if (proc_data == null) {
      return null;
    }
    calcuration();
    return attr_data;
  }

  //属性値を計算する関数
  void calcuration() {
    //注目点
    float cx, cy, cts;
    //比較点
    float nx, ny, nts;
    for (int i=0; i<proc_data.size()-1; i++) {
      cx = proc_data.get(i).posX; //注目点x
      cy = proc_data.get(i).posY; //注目点y
      cts = proc_data.get(i).timestamp; //注目点ts
      nx = proc_data.get(i+1).posX; //比較点x
      ny = proc_data.get(i+1).posY; //比較点y
      nts = proc_data.get(i+1).timestamp; //注目点ts
      float norm = get_norm(cx, cy, nx, ny);
      float velocity = get_velocity(cx, cy, nx, ny, cts, nts);
      float degree = get_degree(cx, cy, nx, ny);
      attr_data.add(new DataAttributeModel(degree, norm, velocity));
    }
  }

  //移動量を計算
  float get_norm(float cx, float cy, float nx, float ny) {
    float norm = dist(cx, cy, nx, ny);
    return norm;
  }

  //移動速度を計算
  float get_velocity(float cx, float cy, float nx, float ny, float cts, float nts) {
    float velocity = dist(cx, cy, nx, ny)/(abs(nts-cts)*100.0);
    return velocity;
  }

  //移動方向の計算
  float get_degree(float cx, float cy, float nx, float ny) {
    float vx = (cx-nx)/dist(cx, cy, nx, ny); //方向x
    float vy = (cy-ny)/dist(cx, cy, nx, ny); //方向y
    //基準となる単位ベクトル
    float bx = 0.0;
    float by = 1.0;
    //角度の計算
    float degree;
    if (vx<0) {
      //角度180度以下
      degree = acos(bx*vy+vx*by)*(180/PI) - 180;
    } else {
      //角度180度以上
      degree = acos(bx*vy+vx*by)*(180/PI);
    }
    return degree;
  }

  //視線データから固始微動を除く関数
  void fixation(ArrayList<Gaze> raw_data) {
    proc_data = new ArrayList<Gaze>();
    for (int i=0; i<raw_data.size()-1; i++) {
      float cx = raw_data.get(i).posX; //注目点x
      float cy = raw_data.get(i).posY; //注目点y
      float cts = raw_data.get(i).timestamp; //注目点ts
      float nx = raw_data.get(i+1).posX; //比較点x
      float ny = raw_data.get(i+1).posY; //比較点x
      float nts = raw_data.get(i+1).timestamp; //比較点ts
      float px; //処理済みx
      float py; //処理済みy
      float ts; //処理済みts
      if (proc_data.size()<1) { //処理データがまだない場合
        if (dist(cx, cy, nx, ny)<FIX_DIST) {
          px = (cx + nx)/2.0; //2点の平均値をx座標にする
          py = (cy + ny)/2.0; //2点の平均値をy座標にする
          ts = nts; //比較点ntsをtsとする
          proc_data.add(new Gaze(px, py, ts));
        } else {
          px = cx; //注目点cxをx座標にする
          py = cy; //注目点cyをy座標にする
          ts = cts; //注目点ctsをtsとする
          proc_data.add(new Gaze(px, py, ts));
        }
      } else { //処理データがある場合
        float tempX = proc_data.get(proc_data.size()-1).posX; //処理済みデータの最後尾x
        float tempY = proc_data.get(proc_data.size()-1).posY; //処理済みデータの最後尾y
        if (dist(cx, cy, tempX, tempY)<FIX_DIST) {
          px = (cx + 2*tempX)/3.0;
          py = (cy + 2*tempY)/3.0;
          Gaze temp = new Gaze(px, py, cts);
          proc_data.set(proc_data.size()-1, temp);
        } else {
          px = cx; //注目点cxをx座標にする
          py = cy; //注目点cyをy座標にする
          ts = cts; //注目点ctsをtsとする
          proc_data.add(new Gaze(px, py, ts));
        }
      }
    }
  }
}