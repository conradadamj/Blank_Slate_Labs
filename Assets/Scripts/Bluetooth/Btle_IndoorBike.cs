
/*** LITTLE ENDIAN
***
1st 2 Bytes
More Data bit 0 
Average Speed Present bit 1
Instananeous Cadence bit 2
Average Cadence present bit 3
Total distance present bit 4
Resistance Level Present bit 5 
Instantaneous Power Present bit 6 
Average Power Present bit 7 
Expended Energy Present bit 8
Instananeous Speed UINT16 byte 3-4


*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


public class Btle_IndoorBike{
byte[] rawData;
string rawString;
bool moreDataPresent;
bool averageSpeedPresent;
bool instananeousCadencePresent;

bool averageCadencePresent;
bool totalDistancePresent;
bool resistanceLevelPresent;

bool instananeousPowerPresent;
bool averagePowerPresent;
bool heartRatePresent;
double instaneousSpeed;
double instaneousPower; 
double instaneousCadence;


    public Btle_IndoorBike(){
        rawData = new byte[32];
    }
    public void initIndoorBike(byte[] data){
        
        if (data == null){
            Debug.Log("data array was empty return");
       
            return;
        }
        StringBuilder sb = new StringBuilder();
        
        sb.Append("ArrayLength: "+data.GetLength(0).ToString()+ "{");
        foreach (var b in data)
        {
           sb.Append(b + ", ");
        }
        sb.Append("}");
        this.rawString=sb.ToString();
        
        

        rawData=data;
        byte flags1  = data[0];
        byte flags2 = data[1];
        



        moreDataPresent = System.Convert.ToBoolean((flags1 & 1)) ;
        
        averageSpeedPresent= System.Convert.ToBoolean(((flags1 & 2) >> 1)); 
        instananeousCadencePresent = System.Convert.ToBoolean(((flags1 & 4) >> 2));
        averageCadencePresent = System.Convert.ToBoolean(((flags1 & 8) >> 3));
        totalDistancePresent = System.Convert.ToBoolean(((flags1 & 16) >> 4));
        resistanceLevelPresent = System.Convert.ToBoolean(((flags1 & 32) >> 5));
        instananeousPowerPresent = System.Convert.ToBoolean(((flags1 & 64) >> 6));
        averagePowerPresent = System.Convert.ToBoolean(((flags1 & 128) >> 7));
        heartRatePresent = System.Convert.ToBoolean(((flags2 & 2) >> 1));

        byte speed2 = data[2];
        byte speed1 = data[3];

        byte cadence2 = data[4];
        byte cadence1 = data[5];
        byte power2 = data[6];
        byte power1 = data[7];
       
        //Cadence
        //1/minute with a resolution of 0.5
        //revolution_per_minute


        //Power
        //Watts with a resolution of 1


        instaneousSpeed = calculateBikeAttribute(speed1, speed2, 100);
        
        instaneousPower = calculateBikeAttribute(power1, power2, 1);
        instaneousCadence = 1/(calculateBikeAttribute(cadence1, cadence2, 10000));
        instaneousCadence = Math.Round(instaneousCadence, 1);
    }

    private double calculateBikeAttribute(byte attr1, byte attr2, int divider){
        short attrWordShort = (short)(attr1 <<8 | attr2);
        
        double newAttr = (System.Convert.ToDouble(attrWordShort)/divider);
        return newAttr;
    }

    public byte[] getRawData(){
        return this.rawData;
    }
    public string getRawString(){
        return this.rawString;
    }
    public bool getMoreDataPresent(){
        return this.moreDataPresent;
    }
    public bool getAverageSpeedPresent(){
        return this.averageSpeedPresent;
    }
    public bool getInstaneousCadencePresent(){
        return this.instananeousCadencePresent;
    }

    public bool getAverageCadencePresent(){
        return this.averageCadencePresent;
    }
    public bool getTotalDistancePresent(){
        return this.totalDistancePresent;

    }
    public bool getResistanceLevelPresent(){
        return this.resistanceLevelPresent;
    }

    public bool getInstaneousPowerPresent(){
        return this.instananeousPowerPresent;
    }
    public bool getAveragePowerPresent(){
        return this.averagePowerPresent;
    }
    public double getInstaneousCadence(){
        return this.instaneousCadence;
    }
    public double getInstaneousPower(){
        return this.instaneousPower;
    }
    public double getInstaneousSpeed(){
        return  this.instaneousSpeed;
    }
    public void setMoreDataPresent(){
    }
    public void setAverageSpeedPresent(){}
    public void setInstaneousCadencePresent(){}

    public void setAverageCadencePresent(){}
    public void setTotalDistancePresent(){}
    public void setResistanceLevelPresent(){}

    public void setInstaneousPowerPresent(){}
    public void setAveragePowerPresent(){}
}

