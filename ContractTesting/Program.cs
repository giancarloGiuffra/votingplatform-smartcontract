using System;

namespace ContractTesting
{
    class MainClass
    {
        static void Main(string[] args)
        {
            var noparamAVM = System.IO.File.ReadAllBytes("/Users/gianca/Projects/junction/junction/bin/Debug/netstandard2.0/junction.avm");
            var str = Neo.Helper.ToHexString(noparamAVM);

            Neo.VM.ScriptBuilder sb = new Neo.VM.ScriptBuilder();
            sb.EmitPush(new byte[] { 0x13 });
            sb.EmitPush("fuck");
            var _params = sb.ToArray();
            var str2 = Neo.Helper.ToHexString(_params);

            Console.WriteLine("AVM=" + str2 + str);
            Console.ReadLine();
        }
    }
}
