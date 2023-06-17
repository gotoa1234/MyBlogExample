using System.ComponentModel;

namespace WebSiteConfigReplaceExample
{
    public class HairDryerPlug : IPluging
    {
        public void Connect()
        {
            Console.WriteLine("吹風機連接成功!");
        }
    }

    public class Socket
    {
        private IPluging _iPlug;

        public Socket(IPluging input)
        {
            _iPlug = input;
        }

        public void Connection()
        {
            _iPlug.Connect();
        }
    }

    public interface IPluging
    {
        public void Connect();
    }
}


//namespace WebSiteConfigReplaceExample
//{
//    public class InversionOfControl
//    {
//    }

//    public class HairDryerPlug : IElectricalPlug
//    {
//        public void Connect()
//        {
//            Console.WriteLine("HairDryerPlug connected!\n");
//        }
//    }

//    public class Socket
//    {
//        private readonly IElectricalPlug _plug;

//        // 建構子注入
//        public Socket(IElectricalPlug plug)
//        {
//            if (plug == null)
//                throw new ArgumentNullException("plug 為空值");

//            this._plug = plug;
//        }

//        public void SendPower()
//        {
//            this._plug.Connect();
//        }
//    }

//    public interface IElectricalPlug
//    {
//        void Connect();
//    }
//}
