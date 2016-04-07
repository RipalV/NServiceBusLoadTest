# NServiceBusLoadTest
This is a web application to test NServiceBus under load while using AzureServiceBus

Web application provider a test endpoint as below:
> http://localhost:8888/status/test/{optional number of messages}/{optional handler sleep time in seconds}

To get this application working, update below files:
1) Update Web.config to set Azure ServiceBus connection string value for app setting `Messaging.AzureServiceBus.ConnectionString`
2) Update `License.xml` under `NServiceBusTest\Messaging\License` to store valid NServiceBus license
