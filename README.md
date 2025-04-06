# E-Commerce-Shop API Documentation

### Overview

The E-Commerce-Shop API currently provides three endpoints area. Employee authentication, product management and most important negotiation handling.

## Authentication

[TokenController](./ShopAPI/ShopAPI/Controllers/TokenController.cs) - handling employee login and token generation

### <span style="color:white;background-color:green;padding:6px;border-radius:10px">POST</span> `/api/v1/token`

Generates an access token for employee authentication.

##### Request Body:

```
{
  "username": "string",
  "email": "string"
}
```

#### Response:

-  <b style="color:#12CA93">200 OK:</b> Returns the generated access token.
-  <b style="color:#E95F6A">400 Bad Request:</b> If the body is null or invalid.

# Product management

[ProductController](./ShopAPI/ShopAPI/Controllers/ProductController.cs) - manages product creation, retrieval, updates and deletion.

### <span style="color:white;background-color:green;padding:6px;border-radius:10px">POST</span> `/api/v1/product`

Creates a new product. Requires **_Employee_** role.

##### Request Body:

```JSON
{
  "name": "string", // min length: 3
  "price": decimal  // positive number
}
```

#### Response:

-  <b style="color:#12CA93">201 Created:</b> Returns the created product.
-  <b style="color:#E95F6A">400 Bad Request:</b> If validation fails.

### <span style="color:white;background-color:#61AFEE;padding:6px;border-radius:10px">GET</span> `/api/v1/product`

Retreives all products.

#### Response:

-  <b style="color:#12CA93">200 OK:</b> Returns a list of products.

### <span style="color:white;background-color:#61AFEE;padding:6px;border-radius:10px">GET</span> `/api/v1/product/{productId}`

Retreives a product by its ID.

#### Response:

-  <b style="color:#12CA93">200 OK:</b> Returns requested product.
-  <b style="color:#E95F6A">404 Not Found:</b> If product not found in database.

### <span style="color:white;background-color:#FCB768;padding:6px;border-radius:10px">PATCH</span> `/api/v1/product/{productId}`

Updates an existing product. Requires **_Employee_** role.

##### Request Body:

```JSON
{
  "name": "string", // optional, min length: 3
  "price": decimal  // optional, positive number
}
```

#### Response:

-  <b style="color:#12CA93">200 OK:</b> Returns the updated product.
-  <b style="color:#E95F6A">400 Bad Request:</b> If validation fails.
-  <b style="color:#E95F6A">404 Not Found:</b> If product not found in database.

### <span style="color:white;background-color:#E95F6A;padding:6px;border-radius:10px">DELETE</span> `/api/v1/product/{productId}`

Deletes a product by its ID. Requires **_Employee_** Role.

#### Response:

-  <b style="color:#12CA93">204 No Content:</b> If product succesfully deleted.
-  <b style="color:#E95F6A">404 Not Found:</b> If product not found in database.

# Negotiation Management

[NegotiationController](./ShopAPI/ShopAPI/Controllers/NegotiationController.cs) - processes such as starting, responding, proposing new prices, retrieving negotiations and cancelling them.

### <span style="color:white;background-color:green;padding:6px;border-radius:10px">POST</span> `/api/v1/negotiation`

Starts a new negotiation for a product

##### Request Body:

```JSON
{
  "propsed_proce": "decimal", // positive number
  "product_id": integer       // positive number
}
```

#### Response:

-  <b style="color:#12CA93">201 Created:</b> Returns the created negotiation.
-  <b style="color:#E95F6A">400 Bad Request:</b> If validation fails.
-  <b style="color:#E95F6A">404 Not Found:</b> If product not found.
-  <b style="color:#E95F6A">409 Conflict:</b> If negotiation was already started.

### <span style="color:white;background-color:#61AFEE;padding:6px;border-radius:10px">GET</span> `/api/v1/negotiation`

Retreives all negotiations.

#### Response:

-  <b style="color:#12CA93">200 OK:</b> Returns a list of negotiations.

### <span style="color:white;background-color:#61AFEE;padding:6px;border-radius:10px">GET</span> `/api/v1/negotiation/{negotiationId}`

Retreives a negotiation by its ID.

#### Response:

-  <b style="color:#12CA93">200 OK:</b> Returns requested negotiation.
-  <b style="color:#E95F6A">404 Not Found:</b> If negotiation not found in database.

### <span style="color:white;background-color:#FCB768;padding:6px;border-radius:10px">PATCH</span> `/api/v1/negotiation/{negotiationId}/respond`

Responds to a negotiation (accept or reject). Requires **_Employee_** role.

##### Request Body:

```JSON
{
  "accept": boolean
}
```

#### Response:

-  <b style="color:#12CA93">200 OK:</b> Returns the respond message.
-  <b style="color:#E95F6A">400 Bad Request:</b> If validation fails.
-  <b style="color:#E95F6A">404 Not Found:</b> If negotiation not found in database.
-  <b style="color:#E95F6A">409 Conflict:</b> If negotiation was already beed responded to.
-  <b style="color:#E95F6A">410 Gone:</b> If negotiation has been canceled.

### <span style="color:white;background-color:#FCB768;padding:6px;border-radius:10px">PATCH</span> `/api/v1/negotiation/{negotiationId}/propose`

Proposes a new price for an ongoing negotiation.

##### Request Body:

```JSON
{
  "new_price": decimal // positive number
}
```

#### Response:

-  <b style="color:#12CA93">200 OK:</b> Returns the negotiation object.
-  <b style="color:#E95F6A">400 Bad Request:</b> If validation fails.
-  <b style="color:#E95F6A">404 Not Found:</b> If negotiation not found in database.
-  <b style="color:#E95F6A">409 Conflict:</b> If submited price waits for Employee respond.
-  <b style="color:#E95F6A">410 Gone:</b> If negotiation has been canceled or expired.
-  <b style="color:#E95F6A">422 Unprocessable Content:</b> If negotiation has been already accepted.

### <span style="color:white;background-color:#FCB768;padding:6px;border-radius:10px">PATCH</span> `/api/v1/negotiation/{negotiationId}`

Cancels an ongoin negotiation by its ID.

#### Response:

-  <b style="color:#12CA93">200 Ok:</b> Returns confirmation message.
-  <b style="color:#E95F6A">404 Not Found:</b> If negotiation not found in database.
-  <b style="color:#E95F6A">410 Gone:</b> If negotiation has already been canceled.
