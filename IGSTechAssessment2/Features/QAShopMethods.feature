Feature: QA Shop
	Shop for products with inventory that can have 
	products created, retrieved, updated and deleted

@mytag
Scenario: Get Products
	Given I am a shop owner with a shop inventory
	When I request a list of products currently in stock
	Then I am able to see a list of products
	And I can see an ID for each product
	And I can see a name for each product
	And I can see a price for each product
 
Scenario: Create Products
	Given I am a shop owner with a shop inventory
	When I create a new product
	And I give an ID
	And I give a name 
	And I give a price
	And I add to the shop inventory
	Then I am able to see the new product appear in the inventory
	And I am able to see the ID
	And I am able to see the name
	And I am able to see the price

Scenario: Get Product By ID
	Given I am a shop owner with a shop inventory
	When I search for a product by ID
	Then I am able to see the product
	And I am able to see the ID
	And I am able to see the name
	And I am able to see the price

Scenario: Update product
	Given I am a shop owner with a shop inventory
	When I update the product
	And I have the ID for a product to be updated
	And I give a name 
	And I give a price
	Then I am able to see the updated product in the list of products
	And I am able to see the ID
	And I am able to see the name
	And I am able to see the price

Scenario: Delete product
	Given I am a shop owner with a shop inventory
	And I have the ID for a product to be deleted
	When I delete the product
	Then I am unable to search for that product by ID
