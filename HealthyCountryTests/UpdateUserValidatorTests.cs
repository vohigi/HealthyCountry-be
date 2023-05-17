using System.Collections.Generic;
using System.Linq;
using HealthyCountry.DataModels;
using HealthyCountry.Services;
using Xunit;
using Xunit.Abstractions;

namespace HealthyCountryTests
{
    public class UpdateUserValidatorTests
    {
        private readonly IUserValidator _updateUserValidator;
        private readonly ITestOutputHelper _output;

        public UpdateUserValidatorTests(ITestOutputHelper output)
        {
            _output = output;
            _updateUserValidator = new UpdateUserRequestValidator();
        }

        
        
        
        [Theory]
        [MemberData(nameof(UpdateTestData))]
        public void TestUserUpdate(UserRequestDataModel userRequestMock, bool expectedResult, string expectedMessage)
        {
            //Arrange
           
            //Act
            var actualResult = _updateUserValidator.Validate(userRequestMock);
            //_output.WriteLine(JsonConvert.SerializeObject(actualResult));
            //Assert
            Assert.Equal(expectedResult, actualResult.IsValid);
            if (!string.IsNullOrEmpty(expectedMessage))
            {
                Assert.Contains(expectedMessage, actualResult.Errors.Select(x => x.ErrorMessage));
            }
        }

        public static IEnumerable<object[]> UpdateTestData()
        {
            yield return new object[] {new UserRequestDataModel(), false, "'First Name' не должно быть пусто."};
            yield return new object[]
                {new UserRequestDataModel {FirstName = "aaaa"}, false, "'Last Name' не должно быть пусто."};
            yield return new object[]
            {
                new UserRequestDataModel {FirstName = "aaaa", LastName = "aaaa"}, false,
                "'Gender' не должно быть пусто."
            };
            yield return new object[]
            {
                new UserRequestDataModel {FirstName = "aaaa", LastName = "aaaa", Gender = "MALE"}, false,
                "'Email' не должно быть пусто."
            };
            yield return new object[]
            {
                new UserRequestDataModel {FirstName = "aaaa", LastName = "aaaa", Gender = "MALE", Email = "aaa"}, false,
                "'Email' неверный email адрес."
            };
            yield return new object[]
            {
                new UserRequestDataModel
                    {FirstName = "aaaa", LastName = "aaaa", Gender = "MALE", Email = "vohigi@gmail.com"},
                true, ""
            };
            yield return new object[]
            {
                new UserRequestDataModel
                {
                    FirstName = "aaaa", LastName = "aaaa", Gender = "MALE", Email = "vohigi@gmail.com", Password = "ss"
                },
                false, "'Password' имеет неверный формат."
            };
            yield return new object[]
            {
                new UserRequestDataModel
                {
                    FirstName = "aaaa", LastName = "aaaa", Gender = "MALE", Email = "vohigi@gmail.com",
                    Password = "Sasha280920"
                },
                true, ""
            };
            yield return new object[]
            {
                new UserRequestDataModel
                {
                    FirstName = "aaaa", LastName = "aaaa", Gender = "MALE", Email = "vohigi@gmail.com",
                    Password = "Sasha280920", Role = "aa"
                },
                false, "'Role' имеет диапазон значений, который не содержит 'aa'."
            };
            yield return new object[]
            {
                new UserRequestDataModel
                {
                    FirstName = "aaaa", LastName = "aaaa", Gender = "MALE", Email = "vohigi@gmail.com",
                    Password = "Sasha280920", Role = "ADMIN"
                },
                true, ""
            };
        }
    }
}