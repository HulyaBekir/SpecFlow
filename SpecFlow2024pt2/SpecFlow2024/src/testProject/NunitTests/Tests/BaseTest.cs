using Newtonsoft.Json;
using NunitTests.Core.Models;
using NunitTests.Core.Utils;
using System.Reflection.Metadata;
using System.Text;

namespace NunitTests.Tests
{
    [TestFixture]
    public class BaseTests
    {
        private HttpClientHelper _handler;

        [SetUp]
        public void Setup()
        {
            _handler = new HttpClientHelper();
        }

        [Test]
        public async Task GetAllUser()
        {
            var responseFromHelper = await _handler.GET("users");
            //Assert the response
            Assert.AreEqual("OK", response.StatusCode.ToString());

            var users = JsonConvert.DeserializeObject<List<UserResponse>>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(users);
            Assert.IsTrue(users.Count > 0);
        }

        [Test]
        public async Task CreateUser()
        {
            var userName = new Guid().ToString();
            // call CreateUserRequestBody()
            var requestBody = _handler.CreateUserRequestBody(userName);

            // call POST method
            var response = await _handler.POST("users", requestBody);
            Assert.AreEqual("Created", response.StatusCode.ToString());

            var createdUser = JsonConvert.DeserializeObject<UserResponse>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(createdUser.Id);
            Assert.AreEqual(userName, createdUser.Name);
            //Asserts
            // Id should not be null or empty
            // Name should be equal to the name of the user

        }

        [Test]
        public async Task GetSpecificUser()
        {

            var userName = Guid.NewGuid().ToString();
            var requestBody = _handler.CreateUserRequestBody(userName);

            var postResponse = await _handler.POST("users", requestBody);
            Assert.AreEqual("Created", postResponse.StatusCode.ToString());

            var createdUser = JsonConvert.DeserializeObject<UserResponse>(await postResponse.Content.ReadAsStringAsync());

            var getResponse = await _handler.GET($"users/{createdUser.Id}");
            Assert.AreEqual("OK", getResponse.StatusCode.ToString());

            var retrievedUser = JsonConvert.DeserializeObject<UserResponse>(await getResponse.Content.ReadAsStringAsync());

            Assert.AreEqual(createdUser.Id, retrievedUser.Id);
            Assert.AreEqual(createdUser.Name, retrievedUser.Name);
            Assert.AreEqual(createdUser.Email, retrievedUser.Email);
            Assert.AreEqual(createdUser.Gender, retrievedUser.Gender);
            Assert.AreEqual(createdUser.Status, retrievedUser.Status);

            //POST - to create a new user
            //GET - users/{user.ID}
            //Assert the response
        }

        [Test]
        public async Task UpdateUser()
        {
            var userName = Guid.NewGuid().ToString();
            var requestBody = _handler.CreateUserRequestBody(userName);

            var postResponse = await _handler.POST("users", requestBody);
            Assert.AreEqual("Created", postResponse.StatusCode.ToString());

            var createdUser = JsonConvert.DeserializeObject<UserResponse>(await postResponse.Content.ReadAsStringAsync());

            var updateRequestBody = new UserRequest
            {
                Name = "UpdatedName",
                Email = createdUser.Email,
                Gender = createdUser.Gender,
                Status = createdUser.Status
            };
            var updateContent = new StringContent(JsonConvert.SerializeObject(updateRequestBody), System.Text.Encoding.UTF8, "application/json");
            var putResponse = await _handler.PUT($"users/{createdUser.Id}", updateContent);
            Assert.AreEqual("OK", putResponse.StatusCode.ToString());

            //  GET the updated user
            var getResponse = await _handler.GET($"users/{createdUser.Id}");
            Assert.AreEqual("OK", getResponse.StatusCode.ToString());

            var updatedUser = JsonConvert.DeserializeObject<UserResponse>(await getResponse.Content.ReadAsStringAsync());

            //  Assert the response
            Assert.AreEqual(createdUser.Id, updatedUser.Id);
            Assert.AreEqual("UpdatedName", updatedUser.Name);
            Assert.AreEqual(createdUser.Email, updatedUser.Email);
            Assert.AreEqual(createdUser.Gender, updatedUser.Gender);
            Assert.AreEqual(createdUser.Status, updatedUser.Status);
        }



        //POST - to create a new user
        // HttpClientHelper.CreateUserRequestBody()
        // PUT - users/{user.ID}
        //Assert the response
    }

        [Test]
        public async Task DeleteUser()
        {
        //POST - to create a new user
        //DELETE - users/{user.ID}
        //Assert the response
        var userName = Guid.NewGuid().ToString();
        var requestBody = _handler.CreateUserRequestBody(userName);

        var postResponse = await _handler.POST("users", requestBody);
        Assert.AreEqual("Created", postResponse.StatusCode.ToString());

        var createdUser = JsonConvert.DeserializeObject<UserResponse>(await postResponse.Content.ReadAsStringAsync());

        // Delete the user
        var deleteResponse = await _handler.DELETE($"users/{createdUser.Id}");
        Assert.AreEqual("NoContent", deleteResponse.StatusCode.ToString());

        //  Verify the user is deleted
        var getResponse = await _handler.GET($"users/{createdUser.Id}");
        Assert.AreEqual("NotFound", getResponse.StatusCode.ToString());
    }
    }
}