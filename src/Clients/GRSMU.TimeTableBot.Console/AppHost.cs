using GRSMU.TimeTableBot.Common.Telegram;

namespace GRSMU.TimeTableBot.ConsoleApp
{
    public class AppHost
    {
        private readonly TelegramClientRunner _telegramClientRunner;

        public AppHost(TelegramClientRunner telegramClientRunner)
        {
            _telegramClientRunner = telegramClientRunner ?? throw new ArgumentNullException(nameof(telegramClientRunner));
        }

        public void Run()
        {
            Console.WriteLine("Bot started");

            _telegramClientRunner.RunBot();

            Console.ReadKey();
        }
    }
}
