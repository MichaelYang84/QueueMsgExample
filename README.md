# Azure Service Bus Order Processor Sample

This sample demonstrates how to:

- Receive a message from an Azure Service Bus queue
- Deserialize JSON into an Order object
- Validate and process the order
- Handle malformed or invalid messages safely
- Write unit tests for valid and invalid input scenarios

## Order Model

- OrderId
- CustomerName
- TotalAmount
- Timestamp
