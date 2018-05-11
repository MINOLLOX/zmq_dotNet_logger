using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;

namespace UNICOORD
{
    public enum UNI_TOPIC
    {
        INFO = 0,
        ERROR,
        KS1,
        KS2

    }



    public class Receiver : IDisposable
    {

        private string sEndpoint = "tcp://localhost:5555";

        private string sLogfile = @"C:\temp\unicoord.log";

        System.IO.StreamWriter file = null;


        private SubscriberSocket subscriber;


        public Receiver(string sFile = "")
        {
            if (sFile.Equals("") == false)
                sLogfile = sFile;

            subscriber = new SubscriberSocket();

            subscriber.Connect(sEndpoint);

            subscriber.SubscribeToAnyTopic();

            file = new System.IO.StreamWriter(sLogfile);

            file.AutoFlush = true;

        }


        public void Start()
        {
            Console.WriteLine("Sarting subscriber!");
            while (true)
            {
                var message = subscriber.ReceiveFrameString();

                Console.WriteLine("Received {0}", message);
                file.WriteLine($"[{DateTime.Now}] {message}");

            }
        }






        public void Dispose()
        {
            Console.WriteLine("Flushing and closing the file");
            file.Flush();
            file.Close();
        }
    }




    public class Sender
    {

        private static Sender instance = null;


        private string sEndpoint = "tcp://localhost:5555";


        private PublisherSocket publisher;

        private List<UNI_TOPIC> lstTopics;

        private Sender()
        {

            publisher = new PublisherSocket();

            publisher.Bind(sEndpoint);


            lstTopics = new List<UNI_TOPIC>()
            {
                UNI_TOPIC.ERROR,
                UNI_TOPIC.KS1,
                UNI_TOPIC.KS2
            };


        }


        public static Sender GetInstance(string sFile = "")
        {

            if (instance == null)
            {
                instance = new Sender();

                instance.SendEntry(UNI_TOPIC.INFO, "Starting a new log");

            }


            return instance;
        }

        public void SendEntry(UNI_TOPIC topic, string sMsg)
        {
            string sMessage = $"{topic.ToString()}:{sMsg}";

            publisher.TrySendFrame(sMessage);
        }

    }
}
