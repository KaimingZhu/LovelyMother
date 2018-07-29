using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using IdentityModel.OidcClient.Browser;

namespace LovelyMother.Uwp.Helpers
{

    internal class SystemBrowser : IBrowser
    {
        private static TaskCompletionSource<BrowserResult> _inFlightRequest;

        public Task<BrowserResult> InvokeAsync(BrowserOptions options)
        {
            _inFlightRequest?.TrySetCanceled();
            _inFlightRequest = new TaskCompletionSource<BrowserResult>();

            Launcher.LaunchUriAsync(new Uri(options.StartUrl));

            return _inFlightRequest.Task;
        }

        public static void ProcessResponse(Uri responseData)
        {
            var result = new BrowserResult
            {
                Response = responseData.OriginalString,
                ResultType = BrowserResultType.Success
            };

            _inFlightRequest.SetResult(result);
            _inFlightRequest = null;
        }
    }
}
