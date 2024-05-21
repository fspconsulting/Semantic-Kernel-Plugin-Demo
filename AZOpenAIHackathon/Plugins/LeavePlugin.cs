using System.ComponentModel;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.SemanticKernel;

namespace Plugins;

public class LeavePlugin
{
    [KernelFunction]
    [Description("Get the amount annual leave remaining for a specific user.")]
    [return: Description("The amount of leave the specified user has remaining.")]
    public async Task<double> GetRemainingLeave(
        Kernel kernel,
        [Description("The name of the user")] string user
    )
    { // TODO add code here to change the response
        double leave = 5;
        return leave;
    }
}