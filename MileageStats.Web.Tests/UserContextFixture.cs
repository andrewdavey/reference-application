/*  
Copyright Microsoft Corporation

Licensed under the Apache License, Version 2.0 (the "License"); you may not
use this file except in compliance with the License. You may obtain a copy of
the License at 

http://www.apache.org/licenses/LICENSE-2.0 

THIS CODE IS PROVIDED ON AN *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED 
WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, 
MERCHANTABLITY OR NON-INFRINGEMENT. 

See the Apache 2 License for the specific language governing permissions and
limitations under the License. */

using MileageStats.Domain.Models;

using Xunit;

namespace MileageStats.Web.Tests
{
    public class UserContextFixture
    {
        [Fact]
        public void WhenContextConvertedToString_ThenCanBeRecovered()
        {
            var userContext = new UserInfo()
                                  {
                                      ClaimsIdentifier = "TestClaimsIdentifier",
                                      DisplayName = "TestDisplayName",
                                      UserId = 55
                                  };

            UserInfo recoveredInfo = UserInfo.FromString(userContext.ToString());

            Assert.Equal(userContext.ClaimsIdentifier, recoveredInfo.ClaimsIdentifier);
            Assert.Equal(userContext.DisplayName, recoveredInfo.DisplayName);
            Assert.Equal(userContext.UserId, recoveredInfo.UserId);
        }
    }
}