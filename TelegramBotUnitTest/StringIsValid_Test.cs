using TestTelegramBot.Services;

namespace TelegramBotUnitTest
{
    public class Tests
    {

        [Test]
        public void StringIsValid_SingleInnIsValid_True()
        {
            //Arrange
            string inn = "1231231231";
            StringIValidationService stringIsValid = new();

            //Act and Assert
            Assert.IsTrue(stringIsValid.InnIsValid(inn));
        }
        [Test]
        public void StringIsValid_MultipleInnIsValid_True()
        {
            //Arrange
            string inn = "1231231231, 1231231231, 1231231231";
            StringIValidationService stringIsValid = new();

            //Act and Assert
            Assert.IsTrue(stringIsValid.InnIsValid(inn));
        }
        [Test]
        public void StringIsValid_SingleInnIsValid_HasLetter_False()
        {
            //Arrange
            string inn = "12312asd";
            StringIValidationService stringIsValid = new();

            //Act and Assert
            Assert.IsFalse(stringIsValid.InnIsValid(inn));
        }
        [Test]
        public void StringIsValid_InnIsValid_HasLetter_False()
        {
            //Arrange
            string inn = "1231231231, 123456789a";
            StringIValidationService stringIsValid = new();

            //Act and Assert
            Assert.IsFalse(stringIsValid.InnIsValid(inn));
        }
        public void StringIsValid_InnIsValid_DoesntHaveComma_False()
        {
            //Arrange
            string inn = "1231231231 1234567890";
            StringIValidationService stringIsValid = new();

            //Act and Assert
            Assert.IsFalse(stringIsValid.InnIsValid(inn));
        }
    }
}