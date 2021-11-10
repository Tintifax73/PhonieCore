using Unosquare.RaspberryIO.Peripherals;
using System.Text;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PhonieCore
{
    public class Rfid
    {     
        public delegate void NewCardDetectedHandler(string uid);
        public event NewCardDetectedHandler NewCardDetected;

        public delegate void CardDetectedHandler(string uid);
        public event CardDetectedHandler CardDetected;
        
        public delegate void CardReleasedHandler(string uid);
        public event CardReleasedHandler CardReleased;

        private const int AliveTime = 1;
        private DateTime CardLastSeen = DateTime.MinValue;

        public Rfid()
        {
            Task.Run(WatchRfid);
        }

        public void WatchRfid()
        {
            RFIDControllerMfrc522 _reader;
            try
            {
                _reader = new RFIDControllerMfrc522();           

                string lastUid = string.Empty;
                while (true)
                {
                    switch(_reader.DetectCard())
                    {
                        case RFIDControllerMfrc522.Status.AllOk:
                            {
                                var uidResponse = _reader.ReadCardUniqueId();

                                switch (uidResponse.Status)
                                {
                                    case  RFIDControllerMfrc522.Status.AllOk:
                                        {
                                            var cardUid = uidResponse.Data;
                                            _reader.SelectCardUniqueId(cardUid);

                                            string currentUid = ByteArrayToString(cardUid);
                                            if(currentUid != lastUid)
                                            {
                                                if (!string.IsNullOrEmpty(lastUid))
                                                {
                                                    CardReleased.Invoke(lastUid);
                                                }
                                                CardLastSeen = DateTime.Now;
                                                lastUid = currentUid;
                                                NewCardDetected.Invoke(currentUid);
                                            }
                                            else
                                            {
                                                CardLastSeen = DateTime.Now;    
                                                CardDetected.Invoke(currentUid);
                                            }
 
                                        }
                                        break;
                                        /*
                                    case  RFIDControllerMfrc522.Status.Error:
                                        {
                                            if (!string.IsNullOrEmpty(lastUid))
                                            {
                                                CardReleased.Invoke(lastUid);
                                                lastUid=null;
                                            }
                                        }        
                                        break;                            
                                        */
                                }
                            } 
                            break;
                        case RFIDControllerMfrc522.Status.Error:                            
                            {
                                if ((!string.IsNullOrEmpty(lastUid)) && (CardLastSeen.AddSeconds(AliveTime) < DateTime.Now))
                                {
                                    CardReleased.Invoke(lastUid);
                                    lastUid=null;
                                }
                            }
                            break;
                    }
            }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }
    }
}
