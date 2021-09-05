union Myfloat
{
  float f;
  unsigned char byte[4];
}fl;
void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  fl.byte[0]=16;
  fl.byte[0]=249;
  fl.byte[0]=20;
  fl.byte[0]=72;
  Serial.println(fl.f, BIN);while(1);
}

void loop() {
  // put your main code here, to run repeatedly:

}
