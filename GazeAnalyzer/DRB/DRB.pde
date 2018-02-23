/*
  2017/10/24 fukuchi tsubasa
 */
ReadBackDetector rbd; //読み返し検出クラス
Table data; //視線データ
String user = ""; //被験者
int num = 0; //読み取り開始データ番号
//データ格納変数
float px, py;
String ts; 

void setup() {
  input_form(); //計算対象データを入力
  //fullScreen();
  rbd = new ReadBackDetector();
  data = load_gaze_data(user);
}

void draw() {
  for (int i=0; i<100; i++) {
    if (num >= data.getRowCount()-1) {
      println("check all data");
      rbd.get_counts();
      noLoop();
      break;
    }
    px = data.getFloat(num, 0);
    py = data.getFloat(num, 1);
    ts = data.getString(num, 2);
    rbd.update(px, py, ts);
    num++;
  }
  rbd.read_counter();
}

//視線データを読み込む関数
Table load_gaze_data(String user) {
  Table data = loadTable(user + ".csv", "header");
  return data;
}