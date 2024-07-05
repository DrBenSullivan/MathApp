using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (HttpContext context) =>
{
    int firstNumber = 0;
    int secondNumber = 0;
    string? operation = null;
    decimal? result = null;

    if (context.Request.Method == "GET" && context.Request.Path == "/")
    {
        var query = context.Request.Query;

        if (query.ContainsKey("firstNumber"))
        {
            string firstNumberString = query["firstNumber"][0];

            if (firstNumberString != null)
            {
                firstNumber = int.Parse(firstNumberString);
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("An invalid firstNumber was provided.\n");
                return;
            }
        }

        if (query.ContainsKey("secondNumber"))
        {
            string secondNumberString = query["secondNumber"][0];

            if (secondNumberString != null)
            {
                secondNumber = int.Parse(secondNumberString);
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("An invalid secondNumber was provided.\n");
                return;
            }
        }

        if (query.ContainsKey("operation"))
        {
            operation = query["operation"][0];

            if (operation == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("A null value for operation was provided.\n");
                return;
            }

            switch (operation)
            {
                case "add":
                    result = firstNumber + secondNumber;
                    break;
                case "minus":
                    result = firstNumber - secondNumber;
                    break;
                case "multiply":
                    result = firstNumber * secondNumber;
                    break;
                case "divide":
                    if (secondNumber == 0)
                    {
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync("Invalid request, cannot divide by zero.\n");
                        return;
                    }
                    result = (decimal)firstNumber / secondNumber;
                    break;
                case "modulo":
                    result = firstNumber % secondNumber;
                    break;
                default:
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync($"{operation} was an invalid operation value.\n");
                    return;
            }

            if (result != null)
            {
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync($"Result = {Math.Round((decimal)result, 2)}\n");
                return;
            }

            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An internal server error occurred.\n");
            return;
        }
    }
});

app.Run();
