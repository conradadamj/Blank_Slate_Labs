using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
namespace Tests{
    public class BtleControllerTest
    {
         private byte[] data;
         Btle_IndoorBike bike;
         // A Test behaves as an ordinary method
         [SetUp] 
         public void Init(){
             bike = new Btle_IndoorBike();
             //data = new byte[]{0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0XFF, 0XFF, 0XFF, 0XFF};
             data = new byte[]{68,2, 38, 7,34,0,39,0,0};
         }
         [Test]
        public void TestParseData1(){
               

                bike.initIndoorBike(data);
                Debug.Log("instaneous power "+bike.getInstaneousPower());
                Debug.Log("instaneous speed "+bike.getInstaneousSpeed());
                Debug.Log("instaneous cadence "+bike.getInstaneousCadence());
                
                
        }
      //   [Test]
        public void TestParseData()
        {
            bike.initIndoorBike(data);
            
            Debug.Log("Data Length = "+ data.GetLength(0));
            Assert.AreEqual(bike.getMoreDataPresent(),true);
            Assert.AreEqual(bike.getAverageCadencePresent(), true);
            Assert.AreEqual(bike.getAverageSpeedPresent(), true);
         
            Assert.AreEqual(bike.getInstaneousCadencePresent(), true);
            Assert.AreEqual(bike.getTotalDistancePresent(), true);
            Assert.AreEqual(bike.getInstaneousPowerPresent(), true);
            Assert.AreEqual(bike.getAveragePowerPresent(), true);
            //Assert.AreEqual(bike.getRawString(), "{0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0XFF, 0XFF, 0XFF}" );
            
        
        // Use the Assert class to test conditions
        }
       
        public void temp(){
           
        }

    }
}