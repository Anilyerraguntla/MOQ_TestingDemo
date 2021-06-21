using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;

namespace CreditCardApplications.Tests
{
    public class CreditCardApplicationEvaluatorTest
    {
        //[Fact]
        //[Trait("Category", "01 Before adding dependency")]
        //public void AcceptHighIncomeApplications()
        //{
        //    var sut = new CreditCardApplicationEvaluator();

        //    var application = new CreditCardApplication { GrossAnnualIncome = 100_000 };

        //    CreditCardApplicationDecision decision = sut.Evaluate(application);

        //    Assert.Equal(CreditCardApplicationDecision.AutoAccepted, decision);
        //}

        //[Fact]
        //[Trait("Category", "01 Before adding dependency")]
        //public void ReferYoungApplications()
        //{
        //    var sut = new CreditCardApplicationEvaluator();

        //    var application = new CreditCardApplication { Age = 19 };

        //    CreditCardApplicationDecision decision = sut.Evaluate(application);

        //    Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        //}



        //[Fact]
        //[Trait("02", "After adding dependency")]
        //public void AcceptHighIncomeApplications02()
        //{
        //    var sut = new CreditCardApplicationEvaluator(null);

        //    var application = new CreditCardApplication { GrossAnnualIncome = 100_000 };

        //    CreditCardApplicationDecision decision = sut.Evaluate(application);

        //    Assert.Equal(CreditCardApplicationDecision.AutoAccepted, decision);
        //}

        //[Fact]
        //[Trait("02", "After adding dependency")]
        //public void ReferYoungApplications02()
        //{
        //    var sut = new CreditCardApplicationEvaluator(null);

        //    var application = new CreditCardApplication { Age = 19 };

        //    CreditCardApplicationDecision decision = sut.Evaluate(application);

        //    Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        //}



        [Fact]
        [Trait("03", "Initializing MOQ")]
        public void AcceptHighIncomeApplications03()
        {
            // Adding mock version of the object which we have been passing as null

            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication { GrossAnnualIncome = 100_000 };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.AutoAccepted, decision);
        }

        [Fact]
        [Trait("03", "Initializing MOQ")]
        public void ReferYoungApplications03()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication { Age = 19 };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }


        [Fact]
        [Trait("04", "Configuring MOQ object method to return values")]
        public void DeclineLowIncomeApplications04()
        {
            // Here trying to access the method which is after the validator method
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            // mocking the method to return the expected value
            mockValidator.Setup(x => x.IsValid("x")).Returns(true);

            //--> Argument matching in mocked methods 

            // --> Any value is accepted interms of the innput param
            //mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            // --> Return value when a specific condition is met
            //mockValidator.Setup(x => x.IsValid(It.Is<string>(num => num.StartsWith("x")))).Returns(true);

            //--> Return value if the input is in specifed range
            //mockValidator.Setup(x => x.IsValid(It.IsInRange("a", "z", Moq.Range.Inclusive))).Returns(true);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication
            {
                GrossAnnualIncome = 19_999,
                Age = 42,
                FrequentFlyerNumber = "x"
            };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);
        }

        //// Types of Mock

        [Fact]
        [Trait("05", "Strict Mock Example")]
        public void ReferInvalidFrequentFlyerApplications()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>(MockBehavior.Strict);

            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication();

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);

        }


        [Fact]
        [Trait("06", "Behaviour Testing Example 1")]
        // Verifying a method is called
        public void ValidateFrequentFlyerNumberForLowIncomeApplictions()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication();

            sut.Evaluate(application);

            // here we are testing wheather Isvalid method of the frequent flyer is been getting called
            // Passing null because we didnt mention frequent flyer number in the ccapplication object

            mockValidator.Verify(x => x.IsValid(null));

        }

        [Fact]
        [Trait("07", "Behaviour Testing Example 2")]
        // Verifying a method is not called
        public void NotValidateFrequentFlyerNumberForHighIncomeApps()
        {

            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication
            {
                GrossAnnualIncome = 110_000
            };

            sut.Evaluate(application);

            mockValidator.Verify(x => x.IsValid(It.IsAny<string>()), Times.Never);

        }

        // Here we can also specify how many times a method can be called .. used when theory attribute and passing diffrent no of arguments


        // All the above mocking can be done on properties as well


    }
}
