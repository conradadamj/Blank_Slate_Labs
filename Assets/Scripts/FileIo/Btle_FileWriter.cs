using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;


public class Btle_FileWriter{
    public string filePath;
   
    public void writeString(string line){
       // string path = "Assets/Resources/test.txt";
         filePath = Application.persistentDataPath + "/settings.txt";
         Debug.Log("filePath = "+filePath);
        
        StreamWriter writer = new StreamWriter(filePath, true);
        writer.WriteLine(line);
        writer.Close();

        //Re-import the file to update the reference in the editor
      //  AssetDatabase.ImportAsset(path); 
        

        //Print the text from the file
        
    }
    public string getFilePath(){
        return filePath;
    }
    public void writeHeader(){
        StringBuilder sb  = new StringBuilder();
        sb.Append("Date Time Speed Cadence Power");
        writeString(sb.ToString());
    }

}
