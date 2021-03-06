//https://github.com/NickChungVietNam/HOANG_SA_LIBRARY_LCD5110_NOKIA.git
//http://arduino.vn/tutorial/1345-nokia5110-huong-dan-su-dung-va-chia-se-thu-vien-hoang-sa


#include "bmps.h"
#include "object.h"
#include "PCD8544_HOANGSA.h"
PCD8544 lcd(3,4,5,6);//RST,D/C, Din,CLK
void setup()   {
  lcd.ON();
  lcd.SET(50,0,0,0,4); 
}




//////----loading---

#include "Class_loading_1.h"

void step_1() {

  loading_1 rect[4];

  lcd.Asc_String(5, 5, Asc("Wait a sec..."), BLACK);
  for (byte counter = 0; counter <= 84; counter++) {
   lcd.drawline(counter, 46,counter,46,BLACK);
    for (byte i = 0; i < 4; i++) {
      if (rect[i].DENTA == -300) {
        rect[i].set_denta(i, 4 - i);
      }
      rect[i].action(i);
    }
    delay(10);
  }
  
}


#include "class_am_duong.h"
void step_2_0( byte r) {
  for ( byte x = 84; x > 42; x--) {
    lcd.drawline(x, 0, x, 47, BLACK);
    lcd.display();
    delay(20);
  }
  am_duong long_trang(r);
  am_duong long_den(r);
  for (byte x = 0; x < 42; x++) {
    long_den.ve_long_den(x, 29, BLACK);
    long_trang.ve_long_trang(84 - x, 18, WHITE);
    delay(20);

  }

}
void step_2_1( byte r) {
  am_duong hinh_am_duong(r);

  for ( int goc = 90; goc < 450; goc += 10) {
    hinh_am_duong.action(goc, BLACK);
    delay(20);
  }

}
void step_2_2( byte r) {
  am_duong hinh_am_duong(r);

  int denta = 1;
  
  for ( int goc = 3600 * 3 + 90; goc > 0; goc -= denta) {
    denta++;
    hinh_am_duong.action(goc, BLACK);
    delay(5);
  }
  //Serial.print(denta); //denta_max=148
  for ( int goc = 3600 * 3 + 90; goc > 0; goc -= denta) {
    denta--;
    hinh_am_duong.action(goc, BLACK);
    delay(5);
  }
  for ( int goc = 3600 * 3; goc > 0; goc -= denta) {
    denta++;
    hinh_am_duong.action(goc, BLACK);
    delay(5);
  }

}
void step_2_3( byte r) {
  am_duong hinh_am_duong(r);

  for ( int goc = 3600; goc > 0; goc -= 148) {
    hinh_am_duong.action(goc, BLACK);
    delay(5);
  }

}
void step_2_4() {
  int goc = 360;
  byte r0;
  while (goc > 0) {
    am_duong hinh_am_duong(r0);

    hinh_am_duong.action_2(goc, BLACK);
    delay(20);
    goc -= 10;
    r0++;
    if (goc == 20) {
      lcd.clear();
    }
  }

}

void step_2_5() {

}
void step_3( ) {
  const char arduino[] PROGMEM = "LCD 84x48   TEST LIBRARY";

  byte x1 = 0;
  byte x2 = 78;
  for (byte i = 0; i < 12; i++) {
    
    lcd.Asc_Char(x1, 10, arduino[i], BLACK);
    lcd.Asc_Char(x2, 20, arduino[23 - i], BLACK);
    lcd.display();
    x1 += 6;
    x2 -= 6;
    delay(100);
  }
}
void osillo_clear() {

  //x??a ki???u osilo
  for (byte width = 120; width > 2; width--) {
    lcd.analog( 0, 0, 83, 47, 0, BLACK );
    lcd.display();

  }
}

#include "class_nguyen_tu.h"
void step_4(byte x0, byte y0, byte a, byte r, bool xoa) {
  //ch??o ph???i
  byte R_hat_nhan;
  R_hat_nhan = a / 5;
  nguyen_tu ng_t1(a, r);
  nguyen_tu ng_t2(a, r);
  //ngang
  nguyen_tu ng_t3(a, r);
  nguyen_tu ng_t4(a, r);
  nguyen_tu ng_t5(a, r);
  // ch??o tr??i

  nguyen_tu ng_t6(a, r);
  nguyen_tu ng_t7(a, r);
  //nguyen_tu ng_t3( 60,30,30,2,45,30);
  for ( int goc_be = 0; goc_be <= 360; goc_be += 4) {
    ng_t1.action(60, goc_be, xoa);
    ng_t2.action(60, goc_be + 180, xoa);

    ng_t3.action(goc_be, 0, xoa);
    ng_t4.action(goc_be + 120, 0, xoa);
    ng_t5.action(goc_be + 240, 0, xoa);

    ng_t6.action(150, 360 - goc_be, xoa);
    ng_t7.action(150, 360 - goc_be - 180, xoa);
    lcd.circle(x0, y0, R_hat_nhan, BLACK);
    lcd.circle(x0, y0, R_hat_nhan + 1, WHITE);
    lcd.display();
    // delay(10);
  }
}
void line_spin_clear(byte color) {

  for (byte y1 = 0; y1 < 48; y1++) {
    lcd.drawline(0, y1, 84, 47 - y1, color);
    lcd.display();

  }
  for (byte x1 = 84; x1 > 0; x1--) {
    lcd.drawline(x1, 0, 84 - x1, 47, color);
    lcd.display();

  }
}
void rect_clear_A( byte color, byte denta) { // denta: chia m??n h??nh ra bao nhi??u ph???n
  byte  denta_x;
  byte denta_x2;
  byte xa_1;
  denta_x = 128 / denta;
  denta_x2 = denta_x * 2;
  byte xa_d, xa_d2;
  int ya;
  for ( int y = 0; y <= 65; y++) {
    xa_1 = 0;
    ya=47-y;
    for ( byte i = 0; i < denta / 2; i++) {
xa_d=xa_1+denta_x;
xa_d2=xa_1 + denta_x2;

      lcd.drawline(xa_1, y, xa_d, y, color);
      lcd.drawline(xa_d, ya, xa_d2, ya, color);


      xa_1 += denta_x2;
    }
    lcd.display();
  }
  lcd.drawline(0,0,127,0,color);
  lcd.display();
}

void step_5(int angle_start, int angle_stop, int denta) {
  // hi???u |angle_start-angle_stop| ph???i chia h???t cho denta


  int h;//chi???u cao
  // h_max=30;
  byte b;//b??n k??nh tr???c ?????ng
  int x[4];
  int y[4];
  int x0, y0, a;
  x0 = 42;
  y0 = 20;
  a = 25;
  for (int goc = angle_start; goc != angle_stop; goc += denta) {

    lcd.fillrect(17, 0, 51, 60, DELETE); // x??a ???nh c??
    b = map(goc, 0, 1080, 0, 30);
    h = 30 - b;
    for ( byte i = 0; i < 4; i++) {
      lcd.find_xy_elip(x0, y0, a, b, goc + 90 * i);
      x[i] = lcd.x_elip();
      y[i] = lcd.y_elip();
    }
    for (byte i = 0; i < 4; i++) {
      if ( i == 3) {

        lcd.drawline(x[3], y[3], x[0], y[0], BLACK);

        lcd.drawline(x[3], y[3] + h, x[0], y[0] + h, BLACK); //   v??? 4 c???nh m???t d?????i
      } else {

        lcd.drawline(x[i], y[i], x[i + 1], y[i + 1], BLACK); // v??? 4 c???nh m???t
        lcd.drawline(x[i], y[i] + h, x[i + 1], y[i + 1] + h, BLACK); //  //b???t ????u v??? 4 c???nh m???t d?????i
      }
      lcd.drawline(x[i], y[i], x[i], y[i] + h, BLACK); //  v??? 4 c???nh b??n

    }
    lcd.display();//hi???n th??? ra m??n h??nh
delay(15);
  }//????NG FOR
}

#include "class_test_draw.h"
void step_6() {
  test_draw circle_main( 63, 31, 25);

  test_draw circle_left( 30, 20, 20);
  test_draw circle_right( 90, 40, 20);
  test_draw circle_left2( 30, 50, 15);
  test_draw circle_right2( 100, 55, 10);
  test_draw circle_left3( 10, 10, 10);
  test_draw circle_right3( 80, 30, 5);
  for ( byte r = 0; r <= 25; r++) {

    circle_main.circle_action(r);
    circle_left.circle_action(r);
    circle_right.circle_action(r);
    circle_left2.circle_action(r);
    circle_right2.circle_action(r);
    circle_left3.circle_action(r);
    circle_right3.circle_action(r);
    lcd.display();
    delay(80);
  }
  for ( byte r = 25; r > 0; r--) {

    circle_main.circle_action(r);
    circle_left.circle_action(r);
    circle_right.circle_action(r);
    circle_left2.circle_action(r);
    circle_right2.circle_action(r);
    circle_left3.circle_action(r);
    circle_right3.circle_action(r);
    lcd.display();
    delay(80);
  }
  lcd.clear();
}
void step_7() {

  test_draw rect_main( 63, 31, 40, 40, 21);
  test_draw rect_left( 30, 40, 20, 40, 5);
  test_draw rect_right(90, 31, 40, 20, 10);
  test_draw rect_left2( 20, 20, 10, 10, 3);
  test_draw rect_right2(100, 40, 5, 10, 2);
  test_draw rect_left3( 10,10 , 20, 10, 3);
  test_draw rect_right3(80, 15, 10, 10, 2);
  for ( byte denta = 0; denta < 60; denta++) {
    rect_main.corner_action(denta);
    rect_left.corner_action(denta);
    rect_right.corner_action(denta);
    rect_left2.corner_action(denta);
    rect_right2.corner_action(denta);
    rect_left3.corner_action(denta);
    rect_right3.corner_action(denta);
    lcd.display();
    delay(40);
  }
  for ( byte denta = 60; denta > 0; denta--) {
    rect_main.corner_action(denta);
    rect_left.corner_action(denta);
    rect_right.corner_action(denta);
    rect_left2.corner_action(denta);
    rect_right2.corner_action(denta);
    rect_left3.corner_action(denta);
    rect_right3.corner_action(denta);
    lcd.display();
   delay(40);
  }
  lcd.clear();
}

void step_8() {
  test_draw tg_main( 63, 31, 40, 0);
  test_draw tg_left( 20, 20, 20, 90);
  test_draw tg_right( 80, 10, 10, 45);
  test_draw tg_left2( 20, 40, 15, 120);
  test_draw tg_right2( 80, 50, 5, 60);
  test_draw tg_left3( 100, 30, 15, 170);
  test_draw tg_right3( 5, 10, 5, 180);
  for (byte r = 0; r < 40; r++) {
    tg_main.triangle_action(r);
    tg_left.triangle_action(r);
    tg_right.triangle_action(r);
    tg_left2.triangle_action(r);
    tg_right2.triangle_action(r);
    tg_left3.triangle_action(r);
    tg_right3.triangle_action(r);
    lcd.display();
    delay(40);
  }
  for (byte r = 40; r > 0; r--) {
    tg_main.triangle_action(r);
    tg_left.triangle_action(r);
    tg_right.triangle_action(r);
    tg_left2.triangle_action(r);
    tg_right2.triangle_action(r);
    tg_left3.triangle_action(r);
    tg_right3.triangle_action(r);
    lcd.display();
    delay(40);
  }
  lcd.clear();
}
void step_9() {
  test_draw el_main(63, 31, 40, 30, 0, 0);
  test_draw el_left(20, 31, 20, 30, 0, 0);
  test_draw el_right(80, 50, 20, 10, 0, 0);
  test_draw el_left2(50, 10, 5, 20, 0, 0);
  test_draw el_right2(60, 20, 10, 40, 0, 0);
  test_draw el_left3(50, 30, 5, 20, 0, 0);
  test_draw el_right3(100, 20, 10, 40, 0, 0);
  for (byte r = 1; r < 40; r++) {
    el_main.ellipse_action(r);
    el_left.ellipse_action(r);
    el_right.ellipse_action(r);
    el_left2.ellipse_action(r);
    el_right2.ellipse_action(r);
    el_left3.ellipse_action(r);
    el_right3.ellipse_action(r);
    lcd.display();
    delay(40);
  }
  for (byte r = 40; r > 0; r--) {
    el_main.ellipse_action(r);
    el_left.ellipse_action(r);
    el_right.ellipse_action(r);
    el_left2.ellipse_action(r);
    el_right2.ellipse_action(r);
    el_left3.ellipse_action(r);
    el_right3.ellipse_action(r);
    lcd.display();
    delay(40);
  }
  lcd.clear();
}
void spin_char(const char text[] PROGMEM, byte n, int goc_max) {
  //n: s??? k?? t???
  // goc_max: g??c d???ng
  byte *x = new byte[n]; // c???p ph??t
  byte *y = new byte[n];
  byte denta_goc;
  denta_goc = 360 / n;
  for (int goc = goc_max; goc > 0; goc -= 5) {

    for (byte i = 0; i < n; i++) {
      lcd.fillrect(x[i], y[i], 6, 8, DELETE); //x??a ???nh c??


    }
    for (byte i = 0; i < n; i++) {
      lcd.find_xy_elip(42, 24, 40, 17, denta_goc * i + goc);
      x[i] = lcd.x_elip();
      y[i] = lcd.y_elip();

    }
    for (byte i = 0; i < n; i++) {
      lcd.Asc_Char(x[i], y[i], text[i], BLACK); // v??? m???i

    }
    lcd.display();
    delay(30);

  }
  delete[] x;
  delete[] y;
}

#include "class_spin_earth.h"
void step_10() {
  spin_earth the_earth(WHITE);
  for (byte part = 0; part < 55; part++) {
    the_earth.draw(86 - part, part % 18);

  }
  //   the_earth.draw(30,0);
  for (byte n = 0; n < 4; n++) {
    for (byte part = 0; part < 18; part++) {

      the_earth.draw(32, part);
    }
  }

  for (byte part = 0; part < 32; part++) {
    the_earth.draw(33 - part, part % 18);

  }

}
void step_11(byte x1, byte y1, byte x2, byte y2, byte x3, byte y3, byte x4, byte y4, const char *c) {
  byte x0;
  byte x[4] = {x1, x2, x3, x4};
  byte y[4] = {y1, y2, y3, y4};
  char next = ',';
  byte counter = 0;
#define XX0 70
  for ( byte i = 0; i < 4; i++) {
    x0 = XX0;
    lcd.rect(x[i] - 1, y[i] - 1, 3, 3, BLACK);
    lcd.drawline( x[i], y[i], x0 - 5, 5 + i * 16, BLACK);
    lcd.drawline( x[i], y[i] - 1, x0 - 5, 4 + i * 16, WHITE);
    lcd.display();
    for (byte n = 0; n < 10; n++) {
      if (*(c + counter) == next) {
        counter++;
        break;
      }
      lcd.Asc_Char(x0, 5 + i * 16, *(c + counter), BLACK);
      counter++;
      lcd.display();
      x0 += 6;
      delay(30);
    }
    delay(300);
  }
  delay(1000);
  x0 = XX0;
  for ( byte i = 0; i < 4; i++) {

    lcd.drawline( x[i], y[i], x0 - 5, 5 + i * 16, DELETE);
    lcd.fillrect(x0 - 1, 5 + i * 16, 54, 8, DELETE);

    lcd.display();
    delay(100);
  }
}
void step_12() {

  const char arduino_vn[] PROGMEM = "Viet Nam,Russia,China,Japan,";
  step_11(15, 32, 18, 10, 20, 20, 33, 21, arduino_vn);

}
void step_13() {

  const char arduino_vn[] PROGMEM = "Italy,India,France,Angola,";
  step_11(15, 17, 41, 34, 12, 15, 13, 38, arduino_vn);

}
void step_14() {

  const char arduino_vn[] PROGMEM = "USA,Brazil,Cuba,Canada,";
  step_11(12, 18, 23, 45, 17, 30, 15, 15, arduino_vn);

}
void step_15() {

  const char arduino_vn[] PROGMEM = "EastOcean,Australia,Indonesia,Philip,";
  step_11(12, 30, 15, 48, 15, 35, 18, 30, arduino_vn);

}
void step_7_2() {
  
  byte x, y, w, h;
  x = 95;
  y = 8;
  w = 57;
  h = 12;
  test_draw rect_1( x, y, w, h, 4);
  test_draw rect_2( x, y + 16, w, h, 4);
  test_draw rect_3( x, y + 32, w, h, 4);
  test_draw rect_4( x, y + 48, w, h, 4);
  spin_earth the_earth(WHITE);
  //xoay laij
  for (byte part = 0; part < 3; part++) {

    the_earth.draw(1, part);
  }

  for ( byte denta = 0; denta < 62; denta++) {
    rect_1.corner_action(denta);
    rect_2.corner_action(denta);
    rect_3.corner_action(denta);
    rect_4.corner_action(denta);
    lcd.display();
  }

  step_12();

  for (byte part = 2; part < 7; part++) {

    the_earth.draw(1, part);
  }
  step_13();

  for (byte part = 7; part < 12; part++) {

    the_earth.draw(1, part);
  }
  step_14();

  for (byte part = 12; part < 18; part++) {

    the_earth.draw(1, part);
  }
  for (byte part = 0; part < 2; part++) {

    the_earth.draw(1, part);
  }

  step_15();
  for (byte part = 0; part < 64; part++) {
    the_earth.draw(0 - part, part % 18);
    if (part == 60) {
      break;
    }
  }
  for ( byte denta = 62; denta > 0; denta--) {
    rect_1.corner_action(denta);
    rect_2.corner_action(denta);
    rect_3.corner_action(denta);
    rect_4.corner_action(denta);
    lcd.display();
    delay(10);
  }
  lcd.clear();

}
void loop() {

  /////////b1/////////
  step_1();
  lcd.clear();
  step_3();
  delay(2000);
  ////XOA////
  osillo_clear();

  step_6();
  step_7();
  step_8();
  step_9();


  /////////b2////////
  byte *r = new byte(7); // c???p ph??t b??n k??nh ban ?????u =7
  step_2_0(*r);
  while (*r < 15) {
    step_2_1(*r);
    *r += 1;
  }
  step_2_2(*r);
  while (*r > 2) {
    step_2_3(*r);
    *r -= 1;
  }
  step_2_4();
  step_2_5();
  delete r;
  delay(2000);
  ////////b4/////////
  rect_clear_A( BLACK, 8);
  rect_clear_A( WHITE, 8);
  ////XOA////
//////////B4
  bool *xoa = new bool;
  for (byte a; a < 60; a += 5) {
    if (a == 50) {
      *xoa = false;
    } else {
      *xoa = true;
    }
    if (a == 55) {
      a -= 5;
      step_4(42, 24, a, a / 15, *xoa);
      break;
    }
    step_4(42, 24, a, a / 15, *xoa);
  }
  delete xoa;
  delay(2000);
  rect_clear_A( BLACK, 8);
  rect_clear_A( WHITE, 8);
  step_5(1080, 0, -5);
  step_5(0, 1080, 5);
  
  line_spin_clear(BLACK);
  line_spin_clear(WHITE);
  step_10();
  step_7_2();
  delay(1000);
  ////the end

  spin_char("ARDUINO.VN", 10, 360);
  lcd.clear();
  spin_char("BY THAI SON", 11, 720);
  lcd.clear();
  spin_char("Thanks for watching", 19, 1080);

  
lcd.bitmap(0,0,84,48,image_glcd_bmp,BLACK);
lcd.display();
delay(10000);
lcd.clear();
}



