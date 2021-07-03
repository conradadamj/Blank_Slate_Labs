using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLE_CSC {

public bool wheelRevolutionsPresent;
public bool crankRevolutionsPresent;
public int wheelRevolutions;
public int lastWheelEventTime;

public int crankRevolutions;
public int crankEventTime;

public int crankSpeed;

public byte[] rawData ;

public BTLE_CSC(){
    rawData=new byte[10];
}
public void initCSC(byte[] data){
       // rawDataText.text = sb.ToString();
                               

        byte flags  = data[0];
        this.wheelRevolutionsPresent = System.Convert.ToBoolean((flags & 0x01)) ;
        this.crankRevolutionsPresent= System.Convert.ToBoolean(((flags & 0x02) >> 1)); 
                               
        if (this.wheelRevolutionsPresent){
            int wheelRevolutions = System.BitConverter.ToInt32(new byte[4]{data[1]
                                                                ,data[2],data[3],data[4]},0);
                                    this.wheelRevolutions = wheelRevolutions;
                                    int lastWheelEventTime = System.BitConverter.ToInt32(new byte[2]{data[5],
                                    data[6]},0);
                                    this.lastWheelEventTime = lastWheelEventTime;
                               }
                               if (this.crankRevolutionsPresent){

                                    int crankRevolution = System.BitConverter.ToInt32(new byte[2]{data[7],data[8]},0);
                                    this.crankRevolutions = crankRevolution;
                                    this.crankEventTime = System.BitConverter.ToInt32(new byte[2]{data[9],data[10]},0);

                               }
                                this.rawData = data;
                                 
                               BluetoothLEHardwareInterface.Log("data: " + data.ToString());
}


}