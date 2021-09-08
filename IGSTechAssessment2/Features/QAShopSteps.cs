using System;
using TechTalk.SpecFlow;
using RestSharp;
using FluentAssertions;
using RestSharp.Serialization.Json;
using System.Collections.Generic;


namespace IGSTechAssessment2.Features
{
    [Binding]
    public class QAShopSteps
    {
        // Api client url
        private const string BASE_URL = "http://localhost:5000/api";

        // Initialisers
        RestClient client;
        RestRequest request;
        IRestResponse response;
        JsonArray output;
        int id;
        int index;

        // Helper methods
        // Get a random product index
        public int GetRandomProduct()
        {
            var numberOfProducts = GetNumberOfProducts();
            Random rnd = new Random();
            int index = rnd.Next(0, numberOfProducts);
            return index;
        }

        // Get the number of products in the products list
        public int GetNumberOfProducts()
        {
            var productsRequest = new RestRequest("/products", Method.GET);
            response = client.Execute(productsRequest);
            output = DeserialiseProducts();
            return output.Count;
        }

        // Get a key for a product by passing the name
        public object GetProductKeyByName(string keyName, int index)
        {
            Dictionary<string, object> entries = (Dictionary<string, object>)output[index];

            var entryKey = entries[keyName];
            return entryKey;
        }

        // Deserialise the response.Content string
        public JsonArray DeserialiseProducts()
        {
            var deserialiser = new JsonDeserializer();
            output = deserialiser.Deserialize<JsonArray>(response);
            return output;
        }

        [Given(@"I am a shop owner with a shop inventory")]
        public void GivenIAmAShopOwnerWithAShopInventory()
        {
            client = new RestClient(BASE_URL);
            client.Timeout = -1;
        }
        
        [When(@"I have the ID for a product to be updated")]
        public void WhenIHaveTheIDForAProductToBeUpdated()
        {
            index = GetRandomProduct();
        }
        
        [Given(@"I have the ID for a product to be deleted")]
        public void GivenIHaveTheIDForAProductToBeDeleted()
        {
            index = GetRandomProduct();
        }
        
        [When(@"I request a list of products currently in stock")]
        public void WhenIRequestAListOfProductsCurrentlyInStock()
        {
            request = new RestRequest("/products", Method.GET);
            response = client.Execute(request);   
        }
        
        [When(@"I create a new product")]
        public void WhenICreateANewProduct()
        {
            request = new RestRequest("/product", Method.POST);
            request.AlwaysMultipartFormData = true;
        }
        
        [When(@"I give an ID")]
        public void WhenIGiveAnID()
        {
            // valid id needs to be the products length plus 1 for the new Id
            // need new request separate from the POST request
            id = GetNumberOfProducts() + 1;

            request.AddParameter("Id", id.ToString());
        }
        
        [When(@"I give a name")]
        public void WhenIGiveAName()
        {
            request.AddParameter("Name", "this is a test product");
        }

        [When(@"I give a price")]
        public void WhenIGiveAPrice()
        {
            var price = 0.99;
            request.AddParameter("Price", price.ToString());
        }

        [When(@"I add to the shop inventory")]
        public void WhenIAddToTheShopInventory()
        {
            response = client.Execute(request);
        }


        [When(@"I search for a product by ID")]
        public void WhenISearchForAProductByID()
        {
            // get a random product id
            index = GetRandomProduct();
            request = new RestRequest("/product/{id}", Method.GET);
            request.AddUrlSegment("id", index.ToString());
            response = client.Execute(request);
        }
        
        [When(@"I update the product")]
        public void WhenIUpdateTheProduct()
        {
            request = new RestRequest("/product/{id}", Method.PUT);
            request.AddUrlSegment("id", index.ToString());
            response = client.Execute(request);
        }
        
        [When(@"I delete the product")]
        public void WhenIDeleteTheProduct()
        {
            request = new RestRequest("/product/{id}", Method.DELETE);
            request.AddUrlSegment("id", index.ToString());
            response = client.Execute(request);
        }
        
        [Then(@"I am able to see a list of products")]
        public void ThenIAmAbleToSeeAListOfProducts()
        {
            //should be a json array with at least one item in it
            GetNumberOfProducts().Should().BeGreaterThan(0);
        }
        
        [Then(@"I can see an ID for each product")]
        public void ThenICanSeeAnIDForEachProduct()
        {
            var productCount = GetNumberOfProducts();
            for(int i = 0; i < productCount; i++)
            {
                var idKey = GetProductKeyByName("id", i);
                idKey.Should().NotBeNull();
            }
        }
        
        [Then(@"I can see a name for each product")]
        public void ThenICanSeeANameForEachProduct()
        {
            var productCount = GetNumberOfProducts();
            for (int i = 0; i < productCount; i++)
            {
                var nameKey = GetProductKeyByName("name", i);
                nameKey.Should().NotBeNull();
            }
        }
        
        [Then(@"I can see a price for each product")]
        public void ThenICanSeeAPriceForEachProduct()
        {
            var productCount = GetNumberOfProducts();
            for (int i = 0; i < productCount; i++)
            {
                var priceKey = GetProductKeyByName("price", i);
                priceKey.Should().NotBeNull();
            }
        }
        
        [Then(@"I am able to see the new product appear in the inventory")]
        public void ThenIAmAbleToSeeTheNewProductAppearInTheInventory()
        {
            var productRequest = new RestRequest("/product/{id}", Method.GET);
            productRequest.AddUrlSegment("id", id);
            response = client.Execute(productRequest);
            output = DeserialiseProducts();
            output.Count.Should().Equals(1);
        }
        
        [Then(@"I am able to see the ID")]
        public void ThenIAmAbleToSeeTheID()
        {
            var idKey = GetProductKeyByName("id", 0);
            idKey.Should().NotBeNull();
        }
        
        [Then(@"I am able to see the name")]
        public void ThenIAmAbleToSeeTheName()
        {
            var nameKey = GetProductKeyByName("name", 0);
            nameKey.Should().NotBeNull();
        }
        
        [Then(@"I am able to see the price")]
        public void ThenIAmAbleToSeeThePrice()
        {
            var priceKey = GetProductKeyByName("price", 0);
            priceKey.Should().NotBeNull();
        }
        
        [Then(@"I am able to see the product")]
        public void ThenIAmAbleToSeeTheProduct()
        {
            var productRequest = new RestRequest("/product/{id}", Method.GET);
            productRequest.AddUrlSegment("id", id);
            response = client.Execute(productRequest);
        }
        
        [Then(@"I am able to see the updated product in the list of products")]
        public void ThenIAmAbleToSeeTheUpdatedProductInTheListOfProducts()
        {
            var productRequest = new RestRequest("/product/{id}", Method.GET);
            productRequest.AddUrlSegment("id", id);
            response = client.Execute(productRequest);
            
        }
        
        [Then(@"I am unable to search for that product by ID")]
        public void ThenIAmUnableToSearchForThatProductByID()
        {
            var productRequest = new RestRequest("/product/{id}", Method.GET);
            productRequest.AddUrlSegment("id", id);
            response = client.Execute(productRequest);
            response.StatusCode.Should().Equals(404);
        }
    }
}
