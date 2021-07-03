
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;


public class btle_controller : MonoBehaviour
{

    // -----------------------------------------------------------------
    // change these to match the bluetooth device you're connecting to:
    // -----------------------------------------------------------------
    // private string _FullUID = "713d****-503e-4c75-ba94-3148f18d941e"; // redbear module pattern
    private string _FullUID = "0000****-0000-1000-8000-00805f9b34fb";     // BLE-CC41a module pattern
    //private string _FullUID = "C2:97:42:A2:FC:42";
    
    
    //1816 Cycling Speed and Cadence
    //1826 Fitness Machine
    private string _serviceUUID = "1826";
    private string _readCscCharacteristicUUID = "2A5B";
   // private string _readIndoorBikeCharacteristicUUID= "2AD2";
   private string _readIndoorBikeCharacteristicUUID = "2AD2";
    private string _writeCharacteristicUUID = "ffe1";
    private string deviceToConnectTo = "IC Bike";

    public bool isConnected = false;
    private bool _readFound = false;
    private bool _writeFound = false;
    private string _connectedID = null;

    private Dictionary<string, string> _peripheralList;
    private float _subscribingTimeout = 0f;

    public Text txtDebug;
    public Text filePath;
    public Text instaneousSpeedPresentText;
    public Text instSpeedText;
  
    public Text instaneousPowerPresentText;

    public Text instPowerText;
    public Text instaneousCadencePresentText;

    public Text instCadenceText;

    
    public Text fieldText;


   

    public Text moreDataText;

    public Text rawDataText;
    public Button btnSend;

    public static BTLE_CSC cadenceObj =new BTLE_CSC(); 
    public static Btle_IndoorBike bike = new Btle_IndoorBike();
    public static Btle_FileWriter btle_FileWriter = new Btle_FileWriter();
    // Use this for initialization
    void Start()
    {
        Debug.Log("We are at least calling the script");
        
        txtDebug.text += "\nInitialising bluetooth \n";
        //receiveText(cadenceObj);
        btle_FileWriter.writeHeader();
        BluetoothLEHardwareInterface.Initialize(true, false, () => { },
                                      (error) => { }
        );
        
        txtDebug.text = "\n Initialized bluetooth \n";
        Invoke("scan", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (_readFound )
        {
            _readFound = false;
           
            _subscribingTimeout = 1.0f;
        }

        if (_subscribingTimeout > 0f)
        {
            _subscribingTimeout -= Time.deltaTime;
            if (_subscribingTimeout <= 0f)
            {
                _subscribingTimeout = 0f;
                txtDebug.text += "Subcribing to Indoor Bike Char:" + _readIndoorBikeCharacteristicUUID;
                BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(
                   _connectedID, FullUUID(_serviceUUID), FullUUID(_readIndoorBikeCharacteristicUUID),
                                                                  
                   (deviceAddress, notification) => {
                   },
                   (deviceAddress2, characteristic, data) => {
                       BluetoothLEHardwareInterface.Log("id: " + _connectedID);
                       if (deviceAddress2.CompareTo(_connectedID) == 0)
                       {
                           BluetoothLEHardwareInterface.Log(string.Format("data length: {0}", data.Length));
                           if (data.Length == 0)
                           {
                            txtDebug.text += "nothing";
                            return;
                           // do nothing
                           }
                           else
                           {
                              
                               bike.initIndoorBike(data);
                               receiveText(bike);
                               recordAttributes(bike);
                           }
                       }
                   });

            }
        }
    }
    public void recordAttributes(Btle_IndoorBike bike)
    {
        System.DateTime theTime = System.DateTime.Now;
        string date = theTime.Year + "-" + theTime.Month + "-" + theTime.Day;
        string time = theTime.Hour + ":" + theTime.Minute + ":" + theTime.Second;
        double speed = bike.getInstaneousSpeed();
        double cadence = bike.getInstaneousCadence();
        double power = bike.getInstaneousPower();
       
        StringBuilder sb = new StringBuilder();
        sb.Append(date+" "+time+ " "+ speed +" "+ cadence+ " "+power);
        btle_FileWriter.writeString(sb.ToString());


    }

    public void receiveText(Btle_IndoorBike obj)
    {
       rawDataText.text = obj.getRawString();
      
       instaneousSpeedPresentText.text = "InstaneousSpeed: "+obj.getMoreDataPresent().ToString();
       instaneousPowerPresentText.text  = "InstaneousPower: "+obj.getInstaneousPowerPresent().ToString();
       instaneousCadencePresentText.text = "IntstaneousCadence:"+obj.getInstaneousCadencePresent().ToString();

       instCadenceText.text = "InstaneousCadence:" + obj.getInstaneousCadence();
       instSpeedText.text = "InstaneousSpeed:" + obj.getInstaneousSpeed();
       instPowerText.text = "InstaneousPower:" + obj.getInstaneousPower();
        filePath.text = "File Path: " + btle_FileWriter.getFilePath();

        //averagePowerText.text = obj.getAveragePowerPresent();
       // rawDataText.text = s;
      //  wheelSpeedText.text="wheelSpeed: "+obj.lastWheelEventTime.ToString();
       // crankSpeedText.text="crankSpeed: "+obj.crankEventTime.ToString();
        //wheelSupportText.text= "wheelSupport: "+obj.wheelRevolutionsPresent.ToString();
        //crankSupportText.text="crankSupport: "+obj.crankRevolutionsPresent.ToString();
    }

   

    void sendBytesBluetooth(byte[] data)
    {
        BluetoothLEHardwareInterface.Log(string.Format("data length: {0} uuid {1}", data.Length.ToString(), FullUUID(_writeCharacteristicUUID)));
        BluetoothLEHardwareInterface.WriteCharacteristic(_connectedID, FullUUID(_serviceUUID), FullUUID(_writeCharacteristicUUID),
           data, data.Length, true, (characteristicUUID) => {
               BluetoothLEHardwareInterface.Log("Write succeeded");
           }
        );
    }


    void scan()
    {

        // the first callback will only get called the first time this device is seen
        // this is because it gets added to a list in the BluetoothDeviceScript
        // after that only the second callback will get called and only if there is
        // advertising data available
        txtDebug.text += ("Starting scan \r\n");
        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) => {
            AddPeripheral(name, address);
        }, (address, name, rssi, advertisingInfo) => { });

    }

    void AddPeripheral(string name, string address)
    {
        

        if (_peripheralList == null)
        {
            _peripheralList = new Dictionary<string, string>();
        }
        if (!_peripheralList.ContainsKey(address))
        {
            _peripheralList[address] = name;
            if (name.Trim().ToLower() == deviceToConnectTo.Trim().ToLower())
            {
                //txtDebug.text += "Found our device, stop scanning \n";
               // BluetoothLEHardwareInterface.StopScan ();
                txtDebug.text += ("Found " + name + " \r\n");
                txtDebug.text += "Connecting to " + address + "\n";
                connectBluetooth(address);
            }
           
        }
       
    }

    void connectBluetooth(string addr)
    {
        BluetoothLEHardwareInterface.ConnectToPeripheral(addr, (address) => {
        },
           (address, serviceUUID) => {
           },
           (address, serviceUUID, characteristicUUID) => {

             // discovered characteristic
             if (IsEqual(serviceUUID, _serviceUUID))
               {
                   _connectedID = address;
                   isConnected = true;

                   if (IsEqual(characteristicUUID, _readIndoorBikeCharacteristicUUID))
                   {
                       _readFound = true;
                   }
                  

                   txtDebug.text += "Connected";
                   BluetoothLEHardwareInterface.StopScan();
                  

               }
           }, (address) => {

             // this will get called when the device disconnects
             // be aware that this will also get called when the disconnect
             // is called above. both methods get call for the same action
             // this is for backwards compatibility
             isConnected = false;
           });

    }


    

    // -------------------------------------------------------
    // some helper functions for handling connection strings
    // -------------------------------------------------------
    string FullUUID(string uuid)
    {
        return _FullUID.Replace("****", uuid);
    }

    bool IsEqual(string uuid1, string uuid2)
    {
        if (uuid1.Length == 4)
        {
            uuid1 = FullUUID(uuid1);
        }
        if (uuid2.Length == 4)
        {
            uuid2 = FullUUID(uuid2);
        }
        return (uuid1.ToUpper().CompareTo(uuid2.ToUpper()) == 0);
    }

}

