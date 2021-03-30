using unit_test_demo_1;
using System;
using Xunit;

namespace test_NumberFunctions
{
    public class TestNumberFunctions
    {
        [Fact]
        public void CanAddNumbers()
        {
            // arrange
            numberFunctions myNumberFunctions = new numberFunctions();
            int myFirstNumber = 50;
            int mySecondNumber = 100;

            // act
            int addResult = myNumberFunctions.addNumbers(myFirstNumber, mySecondNumber);

            // assert
            Assert.Equal(addResult, (myFirstNumber + mySecondNumber));
        }

        [Fact]
        public void CanSubtractNumbers()
        {
            // arrange
            numberFunctions myNumberFunctions = new numberFunctions();
            int myFirstNumber = 50;
            int mySecondNumber = 100;

            // act
            int subResult = myNumberFunctions.subtractNumbers(myFirstNumber, mySecondNumber);

            // assert
            Assert.Equal(subResult, (myFirstNumber - mySecondNumber));
        }

    }
}
