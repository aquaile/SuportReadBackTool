/*
 視線の生データからわかる一連の属性を保存するモデルクラス
 */
class DataAttributeModel {
  //内部変数
  float degree; //角度
  float norm;  //大きさ
  float velocity;  //速度 
  
  //コンストラクタ（x方向、y方向、角度、大きさ、速度）
  DataAttributeModel(float deg, float nor, float vel) {
    degree = deg;
    norm = nor;
    velocity = vel;
  }
}