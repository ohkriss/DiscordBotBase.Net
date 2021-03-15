using System;
using System.Threading.Tasks;

namespace DiscordBotBase
{
    class Program
    {
        public static Bot Bot { get; private set; }
        static Task Main(string[] args)
            => (Bot = new()).InitializeAsync(args);
    }
}
