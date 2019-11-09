# Unit tests for a Azure function written in C#

This sample code has a http trigger function written in c# and unit tests written in xUnit.

## xUnit
xUnit is a free and open source unit testing tool developed for .NET framework. You can test code written in C#, F#, VB.NET and any other .NET language using xUnit testing tool. Nuget Package Manager has the latest xUnit library, you can easily add into your projects

## Unit test with xUnit
This is a very simple test method written in xUnit, it checkes whether the http response has the expected output.
```
  [Fact]
  public async void Http_trigger_should_return_known_string()
  {
    var request = CreateHttpRequest("name", "Hansamali");
    var response = (OkObjectResult)await HttpTriggerFunction.Run(request, logger);
    Assert.Equal("Hello, Hansamali", response.Value);
  }
```

## xUnit test with sample data
When you want to pass data to the xUnit tests, you have to create a TestFactory class and pass data to it like this,
```
 public class TestFactory
 {
        #region Test Data
        public static IEnumerable<object[]> Data()
        {
            return new List<object[]>
            {
                new object[] { "name", "Hansamali" },
                new object[] { "name", "Gamage" }
            };
        }
        #endregion
    }
```
In the test method, use the test data created in the previous step like this and check the function logic works with the random data
```
[Theory]
[MemberData(nameof(TestFactory.Data), MemberType = typeof(TestFactory))]
public async void Http_trigger_should_return_known_string_from_passed_data(string queryStringKey, 
      string queryStringValue)
{
  var request = CreateHttpRequest(queryStringKey, queryStringValue);
  var response = (OkObjectResult)await HttpTriggerFunction.Run(request, logger);
  Assert.Equal($"Hello, {queryStringValue}", response.Value);
}
```
