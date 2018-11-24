using Neo.SmartContract.Framework.Services.Neo;
using System.Collections.Generic;
using Neo.SmartContract.Framework;

namespace test
{
    public class Class1 : SmartContract
    {
        static byte[] validVoters = "AK2nJJpJr6o664CWJKi1QRXjqeic2zRp8y".ToScriptHash();

        public static object Main(string operation, params object[] args)
        {
            switch (operation)
            {
                case "vote":
                    return true;//RegisterVote((byte[])args[0], (byte[])args[1], (byte[])args[2]);
                default:
                    return false;
            }
        }
    }
}
