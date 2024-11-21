using System;
using System.Collections.Generic;
using RestSharp;
using TechTalk.SpecFlow;
using Xunit;

namespace FakeStoreAPI.Tests
{
    [Binding]
    public class FakeStoreAPI_Steps
    {
        private RestResponse _response;
        private string _endpoint;
        private Dictionary<string, object> _productData;

        // รับค่า URL จาก Feature File
        [Given(@"I have the API endpoint ""(.*)""")]
        public void GivenIHaveTheAPIEndpoint(string endpoint)
        {
            _endpoint = endpoint; // เก็บ URL ไว้เพื่อใช้ในขั้นตอนถัดไป
        }

        [Given(@"I have the following product data")]
        public void GivenIHaveTheFollowingProductData(Table table)
        {
            // แปลง Table เป็น Dictionary เพื่อเก็บ Product Data
            _productData = new Dictionary<string, object>
            {
                { "title", table.Rows[0]["title"] },
                { "price", Convert.ToDecimal(table.Rows[0]["price"]) },
                { "description", table.Rows[0]["description"] },
                { "category", table.Rows[0]["category"] }
            };

            Console.WriteLine("Product Data:");
            foreach (var entry in _productData)
            {
                Console.WriteLine($"{entry.Key}: {entry.Value}");
            }
        }

        [Given(@"I have the following incomplete product data")]
        public void GivenIHaveTheFollowingIncompleteProductData(Table table)
        {
            _productData = new Dictionary<string, object>
            {
                { "title", table.Rows[0]["title"] }, // มีเฉพาะ title
                { "price", null }, // ขาด price
                { "description", null }, // ขาด description
                { "category", null } // ขาด category
            };

            Console.WriteLine("Incomplete Product Data:");
            foreach (var entry in _productData)
            {
                Console.WriteLine($"{entry.Key}: {entry.Value}");
            }
        }

        [Given(@"I have the following updated product data")]
        public void GivenIHaveTheFollowingUpdatedProductData(Table table)
        {
            // แปลง Table เป็น Dictionary เพื่อเก็บ Updated Product Data
            _productData = new Dictionary<string, object>
            {
                { "title", table.Rows[0]["title"] },
                { "price", Convert.ToDecimal(table.Rows[0]["price"]) },
                { "description", table.Rows[0]["description"] },
                { "category", table.Rows[0]["category"] }
            };

            Console.WriteLine("Updated Product Data:");
            foreach (var entry in _productData)
            {
                Console.WriteLine($"{entry.Key}: {entry.Value}");
            }
        }

        // ส่ง GET Request
        [When(@"I send a GET request to the endpoint")]
        public void WhenISendAGETRequestToTheEndpoint()
        {
            var client = new RestClient();
            var request = new RestRequest(_endpoint, Method.Get);
            _response = client.Execute(request);
        }

        // ส่ง POST Request
        [When(@"I send a POST request to the endpoint with the product data")]
        public void WhenISendAPostRequestToTheEndpointWithTheProductData()
        {
            var client = new RestClient();
            var request = new RestRequest(_endpoint, Method.Post);

            if (_productData != null)
            {
                request.AddJsonBody(_productData);
            }
            else
            {
                throw new Exception("Product data is missing. Make sure 'Given I have the following product data' has been executed.");
            }

            _response = client.Execute(request);

            Console.WriteLine($"POST Request Sent to: {_endpoint}");
            Console.WriteLine($"Response: {_response.Content}");
        }

        [When(@"I send a POST request to the endpoint with the incomplete product data")]
        public void WhenISendAPostRequestToTheEndpointWithTheIncompleteProductData()
        {
            var client = new RestClient();
            var request = new RestRequest(_endpoint, Method.Post);

            // เพิ่ม Header และตรวจสอบ Body
            request.AddHeader("Content-Type", "application/json");

            if (_productData != null)
            {
                request.AddJsonBody(_productData);
            }
            else
            {
                throw new Exception("Incomplete product data is missing.");
            }

            _response = client.Execute(request);

            // Debugging
            Console.WriteLine($"Request Body: {_productData}");
            Console.WriteLine($"Response Status: {_response.StatusCode}");
            Console.WriteLine($"Response Content: {_response.Content}");
        }

        // ส่ง PUT Request
        [When(@"I send a PUT request to the endpoint with the updated product data")]
        public void WhenISendAPutRequestToTheEndpointWithTheUpdatedProductData()
        {
            var client = new RestClient();
            var request = new RestRequest(_endpoint, Method.Put);

            if (_productData != null)
            {
                request.AddJsonBody(_productData);
            }
            else
            {
                throw new Exception("Product data is missing. Make sure 'Given I have the following updated product data' has been executed.");
            }

            _response = client.Execute(request);

            Console.WriteLine($"PUT Request Sent to: {_endpoint}");
            Console.WriteLine($"Response: {_response.Content}");
        }

        // ส่ง DELETE Request
        [When(@"I send a DELETE request to the endpoint")]
        public void WhenISendADeleteRequestToTheEndpoint()
        {
            var client = new RestClient();
            var request = new RestRequest(_endpoint, Method.Delete);
            _response = client.Execute(request);

            Console.WriteLine($"DELETE Request Sent to: {_endpoint}");
            Console.WriteLine($"Response: {_response.Content}");
        }

        // ตรวจสอบ Response Status Code
        [Then(@"the response status should be (.*)")]
        public void ThenTheResponseStatusShouldBe(int expectedStatusCode)
        {
            Assert.NotNull(_response); // ตรวจสอบว่าการตอบกลับไม่ใช่ null
            Assert.Equal(expectedStatusCode, (int)_response.StatusCode); // ตรวจสอบสถานะ
        }

        [Then(@"the response status should not be (.*)")]
        public void ThenTheResponseStatusShouldNotBe(int unexpectedStatusCode)
        {
            Assert.NotNull(_response);
            Assert.NotEqual(unexpectedStatusCode, (int)_response.StatusCode);
        }

        [Then(@"the response content should be null")]
        public void ThenTheResponseContentShouldBeNull()
        {
            Assert.Equal("null", _response.Content); // FakeStore API ส่ง 'null' เป็น string
        }

        [Then(@"the response content should be empty")]
        public void ThenTheResponseContentShouldBeEmpty()
        {
            Assert.True(string.IsNullOrWhiteSpace(_response.Content), $"Expected response content to be empty, but got: {_response.Content}");
            Console.WriteLine("Response Content is empty as expected.");
        }
    }
}
