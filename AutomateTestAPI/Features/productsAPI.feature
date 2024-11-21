Feature: Test FakeStore API
  # Positive Scenarios

  Scenario: Get all products
    Given I have the API endpoint "https://fakestoreapi.com/products"
    When I send a GET request to the endpoint
    Then the response status should be 200

  Scenario: Create a new product
    Given I have the API endpoint "https://fakestoreapi.com/products"
    And I have the following product data
      | title          | price | description         | category    |
      | Test Product   | 29.99 | A sample product    | electronics |
    When I send a POST request to the endpoint with the product data
    Then the response status should be 200

  Scenario: Update a product
    Given I have the API endpoint "https://fakestoreapi.com/products/1"
    And I have the following updated product data
      | title           | price | description         | category    |
      | Updated Product | 39.99 | Updated description | electronics |
    When I send a PUT request to the endpoint with the updated product data
    Then the response status should be 200

  Scenario: Delete a product
    Given I have the API endpoint "https://fakestoreapi.com/products/1"
    When I send a DELETE request to the endpoint
    Then the response status should be 200

  Scenario: Get product categories
    Given I have the API endpoint "https://fakestoreapi.com/products/categories"
    When I send a GET request to the endpoint
    Then the response status should be 200

  Scenario: Get sorted products
    Given I have the API endpoint "https://fakestoreapi.com/products?sort=asc"
    When I send a GET request to the endpoint
    Then the response status should be 200


  # Negative Scenarios
  Scenario: Get all products with an invalid endpoint
    Given I have the API endpoint "https://fakestoreapi.com/products-invalid"
    When I send a GET request to the endpoint
    Then the response status should be 404

  Scenario: Create a product with missing data
    Given I have the API endpoint "https://fakestoreapi.com/products-invalid"
    And I have the following incomplete product data
      | title           | price | description  | category |
      | Nonexistent     | null  |    null      |   null   |
    When I send a POST request to the endpoint with the incomplete product data
    Then the response status should be 404

  Scenario: Update a product that does not exist
    Given I have the API endpoint "https://fakestoreapi.com/products/"
    And I have the following updated product data
      | title           | price | description         | category    |
      | Nonexistent     | 49.99 | This does not exist | electronics |
    When I send a PUT request to the endpoint with the updated product data
    Then the response status should be 404

  Scenario: Delete a product that does not exist
    Given I have the API endpoint "https://fakestoreapi.com/products/999"
    When I send a DELETE request to the endpoint
    Then the response status should be 200
    And the response content should be null

  Scenario: Get product categories from an invalid endpoint
    Given I have the API endpoint "https://fakestoreapi.com/products/categories-invalid"
    When I send a GET request to the endpoint
    Then the response status should be 200
    And the response content should be empty

  # Parameter not validated
  Scenario: Get sorted products with invalid parameters
    Given I have the API endpoint "https://fakestoreapi.com/products?sort=invalid"
    When I send a GET request to the endpoint
    Then the response status should be 200
