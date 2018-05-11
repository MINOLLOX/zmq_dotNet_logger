//Publishers

void FooFunction()
{
            UNICOORD.Sender snd = UNICOORD.Sender.GetInstance();

            snd.SendEntry(UNICOORD.UNI_TOPIC.INFO, "This is a test!");
            
 }
 
 
 //Subscriber
 
 
     class Logger_Server
    {
        static public void Main()
        {

            //var context = NetMQ.Context.Create();

            using (var server = new SubscriberSocket())
            //using (var server = new ResponseSocket())
            {
       
                Console.WriteLine("ZeroMQ Server is listenig...");

                //server.Bind("tcp://*:5555");
                server.Connect("tcp://localhost:5557");

                server.Subscribe("MAIN");

                while (true)
                {
                    var message = server.ReceiveFrameString();

                    Console.WriteLine("Received {0}", message);


                    if (message.Equals("STOP"))
                        break;

                    // processing the request
                    Thread.Sleep(100);

                    //Console.WriteLine("Sending World");
                    //server.SendFrame("World");

                }

                Console.WriteLine("Server closed!");
            }

        }
    }
