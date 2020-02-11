using ServiceStack;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MadXchange.Connector.Helpers
{
    public static class ToolKit
    {
        internal static byte[] Convert(string message)
        {
            return Encoding.UTF8.GetBytes(message + '\n');//need \n to be endOfLine
            //var header = BitConverter.GetBytes(body.Length);
            //return header.Concat(body).ToArray();
        }

        internal static byte[] ObjectToByteArray<T>(T obj)
        {
            if (obj == null)
            {
                return null;
            }
            return Encoding.UTF8.GetBytes(obj.SerializeToString() + '\n');
        }

        public static bool IsConnect(Socket client)
        {
            var pollResult = client.Poll(250, SelectMode.SelectRead);
            var availableResult = (client.Available == 0);
            if (pollResult && availableResult)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool Retry(int maxLoop, Func<Task> action)
        {
            for (int i = 0; i < maxLoop; i++)
            {
                Thread.Sleep(100);
                try
                {
                    action?.Invoke().Wait();
                    return true;
                }
                catch
                {
                    if (i < maxLoop)
                    {
                        continue;
                    }
                    break;
                }
            }
            return false;
        }
        public static async Task<bool> RetryAsync(int maxLoop, Func<Task> action)
        {
            for (int i = 0; i < maxLoop; i++)
            {
                await Task.Delay(100).ConfigureAwait(false);
                try
                {
                    await action?.Invoke();
                    return true;
                }
                catch
                {
                    if (i < maxLoop)
                    {
                        continue;
                    }
                    break;
                }
            }
            return false;
        }
    }
}
