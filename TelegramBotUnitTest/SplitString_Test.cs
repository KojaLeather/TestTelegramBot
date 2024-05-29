using TestTelegramBot.Services;
namespace TelegramBotUnitTest
{
    public class SplitString_Test
    {
        [Test]
        public void SplitString_SplitInn_SplitOneInn_Equals()
        {
            //Arrange
            string inn = "1234567890";
            List<string> innExcepcted = new() { "1234567890" };

            //Act
            SplitStringsService splitStrings = new();
            List<string> innActual = splitStrings.SplitInn(inn);

            //Assert
            Assert.IsTrue(innExcepcted.SequenceEqual(innActual));
        }
        [Test]
        public void SplitString_SplitInn_SplitMultipleInn_Equals()
        {
            //Arrange
            string inn = "1234567890, 0987654321";
            List<string> innExcepcted = new() { "1234567890", "0987654321" };

            //Act
            SplitStringsService splitStrings = new();
            List<string> innActual = splitStrings.SplitInn(inn);

            //Assert
            Assert.IsTrue(innExcepcted.SequenceEqual(innActual));
        }
    }
}
