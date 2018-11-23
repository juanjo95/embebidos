int pulsador=12;
int potenciometro1=A0;
int potenciometro2=A1;

int presionado;
int vPoten1;
int vPoten2;

void setup() {  
  Serial.begin(9600);
  pinMode(pulsador,INPUT);   
  pinMode(potenciometro1,INPUT);
  pinMode(potenciometro2,INPUT);  

  presionado=0;
  vPoten1=analogRead(potenciometro1);
  vPoten2=analogRead(potenciometro2);
}

void loop() {  
  solucion3();
}

void solucion3(){

  presionado=digitalRead(pulsador);
  vPoten1=map(analogRead(potenciometro1),0,1023,0,50);
  
  vPoten2=map(analogRead(potenciometro2),0,1023,0,50);

  Serial.flush();

  Serial.print(presionado);
  Serial.print(",");
  Serial.print(vPoten1);
  Serial.print(",");
  Serial.print(vPoten2);
  Serial.println();  
  delay(50);
}

void solucion1(){
  if(presionado != digitalRead(pulsador)){    
    presionado=digitalRead(pulsador);
    if(presionado == 1){
      Serial.println("Presionado");  
      Serial.write(1);  
      Serial.flush();
    }
  }  

  if(vPoten1 != analogRead(potenciometro1)){    
    vPoten1=analogRead(potenciometro1);    
      Serial.print("Potenciometro1: ");    
      Serial.println(vPoten1);        
      Serial.write(vPoten1);
      Serial.flush();
  }

  if(vPoten2 != analogRead(potenciometro2)){    
    vPoten2=analogRead(potenciometro2);    
      Serial.print("Potenciometro2: ");    
      Serial.println(vPoten2);        
      Serial.write(vPoten2);
      Serial.flush();
  }
  delay(20);
}

void solucion2(){
  if(digitalRead(pulsador)==1){
      Serial.write(1);  
      Serial.flush();
      delay(20);
  }else{
    Serial.write(0);   
    Serial.flush();
    delay(20);
  }  
}
